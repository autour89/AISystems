using System;
namespace Laba2.Helpers;

public class ServiceHelper
{
    public static TService? GetService<TService>() => Current.GetService<TService>();

    public static IServiceProvider Current =>
#if IOS || MACCATALYST
        MauiUIApplicationDelegate.Current.Services;
#elif ANDROID
        MauiApplication.Current.Services;
#else
        null;
#endif
}

