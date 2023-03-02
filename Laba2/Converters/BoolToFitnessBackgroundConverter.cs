using System;
using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace Laba2.Converters
{
    public class BoolToFitnessBackgroundConverter : BaseConverterOneWay<bool, Color>
    {
        public override Color DefaultConvertReturnValue { get; set; } = Colors.White;

        public override Color ConvertFrom(bool value, CultureInfo? culture)
        {
            ArgumentNullException.ThrowIfNull(value);

            return value ? Colors.DarkRed.WithAlpha(0.4f) : Colors.White;
        }
    }
}