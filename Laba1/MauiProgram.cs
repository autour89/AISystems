using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Diagnostics;
using Laba1.Services;
using Laba1.Views;
using CommunityToolkit.Maui.Markup;
using System.Globalization;

namespace Laba1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterPages()
            .UseMauiCommunityToolkit()
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
        mauiAppBuilder.Services.AddLocalization();

        mauiAppBuilder.Services.AddSingleton<ICleanupService, CleanupService>();

        return mauiAppBuilder;
    }

    static MauiAppBuilder RegisterPages(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPageMarkup>();

        return mauiAppBuilder;
    }
}