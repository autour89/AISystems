using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laba1.Models;
using Laba1.Resources.Languages;
using Laba1.Services;
using Microsoft.Extensions.Localization;

namespace Laba1.ViewModels;

public sealed partial class MainViewModel : BaseViewModel
{
    readonly ICleanupService cleanupService;

    public IStringLocalizer<Locals> StringLocalizer { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEmpty))]
    ObservableCollection<Table> tables;

    public bool IsEmpty => !Tables.Any();

    public MainViewModel(ICleanupService cleanupService, IStringLocalizer<Locals> stringLocalizer)
    {
        this.cleanupService = cleanupService;
        StringLocalizer = stringLocalizer;
        this.cleanupService.OnFinish = OnClaenFinish;
        tables = new();
    }

    [RelayCommand]
    async Task MakeOrders()
    {
        try
        {
            var tables = await cleanupService.MakeOrders();

            Tables = tables.ToObservableCollection();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "An error has occurred while making orders.",
                "Ok"
            );
        }
    }

    [RelayCommand]
    async Task Cleanup()
    {
        try
        {
            await Task.Run(() =>
            {
                Tables = Tables
                    .OrderBy(x => x.IsClean)
                    .ThenByDescending(y => y.Priority)
                    .ToObservableCollection();
            });

            await cleanupService.Cleanup();
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                "An error has occurred while making a cleaning up.",
                "Ok"
            );
        }
    }

    [RelayCommand]
    async Task Reset()
    {
        await Task.Run(() =>
        {
            cleanupService.Reset();

            Tables = cleanupService.Tables.ToObservableCollection();
        });
    }

    void OnClaenFinish(bool isFinished)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.DisplayAlert("Finished", "All tables are cleaned up.", "Ok");
        });
    }
}
