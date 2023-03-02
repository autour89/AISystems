using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Diagnostics;

namespace Laba1;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
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
}