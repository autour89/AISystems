using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laba2.Models;
using Laba2.Resources.Languages;
using Laba2.Services;
using Microsoft.Extensions.Localization;

namespace Laba2.ViewModels;

public sealed partial class MainViewModel : BaseViewModel
{
    readonly IGeneticAlgorithm geneticAlgorithm;
    readonly IStringLocalizer<Locals> stringLocalizer;

    public MainViewModel(IGeneticAlgorithm geneticAlgorithm, IStringLocalizer<Locals> stringLocalizer)
    {
        this.geneticAlgorithm = geneticAlgorithm;
        this.stringLocalizer = stringLocalizer;
        Initialise();
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanRun))]
    SizeModel? selectedPopulationNumber;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanRun))]
    SizeModel? selectedGenerationNumber;

    [ObservableProperty]
    IReadOnlyList<SizeModel>? populationNumber;

    [ObservableProperty]
    IReadOnlyList<SizeModel>? generationNumber;

    [ObservableProperty]
    bool isTournament;

    [ObservableProperty]
    bool isRoulette = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowEmpty)), NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    bool isBusy;

    public bool ShowEmpty => !IsBusy && !IsNotEmpty;

    public bool IsNotEmpty => Generations?.Any() ?? false;

    public bool CanRun => !RunCommand.IsRunning && SelectedPopulationNumber is not null && SelectedGenerationNumber is not null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowEmpty)), NotifyPropertyChangedFor(nameof(IsNotEmpty))]
    ObservableCollection<Chromosome>? generations;

    [RelayCommand]
    async Task Run()
    {
        try
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                Generations?.Clear();
                int populationSize = SelectedPopulationNumber?.Value ?? 2;
                int numGenerations = SelectedGenerationNumber?.Value ?? 2;

                // Run the genetic algorithm and get the best solution
                geneticAlgorithm.Run(populationSize, numGenerations, IsTournament);
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Error while calculating the algorithm, please try again later.", "Ok");

            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    void Initialise()
    {
        geneticAlgorithm.OnNextGeneration = NextGeneration;
        Generations = new();
        var numbers = Enumerable.Range(1, 100).ToList();
        PopulationNumber = numbers.Select(x => new SizeModel { Value = x }).ToList();
        GenerationNumber = numbers.Select(x => new SizeModel { Value = x }).ToList();
    }

    void NextGeneration(List<Chromosome> generation)
    {
        Generations = generation.ToObservableCollection();
    }
}