using System;

namespace Laba1.Resources.Styles;

public class AppStyles : ResourceDictionary
{
    public AppStyles() { }

    public static Color PreferredControlColor { get; } =
        App.Current?.RequestedTheme is AppTheme.Dark ? Colors.White : Colors.Gray;
}
