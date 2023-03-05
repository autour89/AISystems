using CommunityToolkit.Maui.Markup;
using Laba1.Models;
using Microsoft.Maui.Controls.Shapes;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace Laba1.Views;

public class TableDataTemplate : DataTemplate
{
    public TableDataTemplate()
        : base(CreateView) { }

    static IView CreateView()
    {
        return new StackLayout
        {
            Children = { new Border
                {
                    StrokeThickness = 2,
                    Stroke = Colors.DarkGray,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    Content = new Grid
                    {
                        ColumnSpacing = 2,
                        ColumnDefinitions = Columns.Define(
                            GridLength.Star,
                            GridLength.Star,
                            GridLength.Star,
                            GridLength.Star
                        ),
                        Children =
                        {
                            new Label { LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 }
                                .Center()
                                .Column(0)
                                .Font(size: 16)
                                .Bind(
                                    Label.TextProperty,
                                    static (Table table) => table.Name,
                                    mode: BindingMode.OneTime
                                ),
                            new Label { LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 }
                                .Center()
                                .Column(1)
                                .Font(size: 16)
                                .Bind(
                                    Label.TextProperty,
                                    static (Table table) => table.Size,
                                    mode: BindingMode.OneTime
                                ),
                            new Label { LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 }
                                .Center()
                                .Column(2)
                                .Font(size: 16)
                                .Bind(
                                    Label.TextProperty,
                                    static (Table table) => table.Priority,
                                    mode: BindingMode.OneTime
                                ),
                            new Label { LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 }
                                .Center()
                                .Column(3)
                                .Font(size: 16)
                                .Bind(
                                    Label.TextProperty,
                                    static (Table table) => table.IsClean,
                                    convert: (bool isClean) => isClean ? "CLean" : "Dirty",
                                    mode: BindingMode.OneWay
                                )
                        }
                    }.Margin(20)
                }.Margin(3).Bind(Border.BackgroundColorProperty, static (Table table) => table.IsClean, convert: (bool isClean) => isClean ? Colors.Transparent : Colors.Blue.WithAlpha(.4f), mode: BindingMode.OneWay) }
        };
    }
}
