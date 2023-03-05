using System.Diagnostics;
using CommunityToolkit.Maui;
using Laba2.Services;
using Laba2.ViewModels;
using Laba2.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Laba2;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterPages()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        AddLogging(builder);

        return builder.Build();
    }

    [Conditional("DEBUG")]
    static void AddLogging(MauiAppBuilder builder)
    {
        builder.Logging.AddDebug();
    }

    static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddScoped<ViewModels.MainViewModel>();

        return mauiAppBuilder;
    }

    static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IGeneticAlgorithm, GeneticAlgorithm>();

        mauiAppBuilder.Services.AddLocalization();

        return mauiAppBuilder;
    }

    static MauiAppBuilder RegisterPages(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();

        return mauiAppBuilder;
    }
}
