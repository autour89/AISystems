using CommunityToolkit.Maui.Markup;
using Laba1.ViewModels;
using Microsoft.Maui.Controls.Shapes;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace Laba1.Views;

public class MainPageMarkup : BaseContentPage<MainViewModel>
{
    public MainPageMarkup(MainViewModel viewModel)
        : base(viewModel, viewModel.StringLocalizer["CleaningAgentTitle"])
    {
        Content = new Grid
        {
            RowDefinitions = Rows.Define(GridLength.Auto, GridLength.Auto, GridLength.Star),
            Children =
            {
                CreateHeaderView(),
                new CollectionView()
                    .Row(2)
                    .Margin(10, 20)
                    .BackgroundColor(Colors.Transparent)
                    .ItemTemplate(new TableDataTemplate())
                    .Bind(CollectionView.ItemsSourceProperty, (MainViewModel vm) => vm.Tables),
                new Label { Text = viewModel.StringLocalizer["NoTablesTitle"] }
                    .Row(2)
                    .Center()
                    .FontSize(21)
                    .Bind(ActivityIndicator.IsVisibleProperty, (MainViewModel vm) => vm.IsEmpty),
                new Grid
                {
                    RowDefinitions = Rows.Define(GridLength.Star),
                    ColumnDefinitions = Columns.Define(
                        GridLength.Star,
                        GridLength.Star,
                        GridLength.Star
                    ),
                    Children =
                    {
                        new Button { Text = viewModel.StringLocalizer["MakeOrdersTitle"] , Command = viewModel.MakeOrdersCommand, }
                            .Center()
                            .Row(0)
                            .Column(0)
                            .Font(bold: true),
                        new Button { Text = viewModel.StringLocalizer["CleanTablesTitle"], Command = viewModel.CleanupCommand, }
                            .Center()
                            .Row(0)
                            .Column(1)
                            .Font(bold: true),
                        new Button { Text = viewModel.StringLocalizer["ResetTitle"], Command = viewModel.ResetCommand, }
                            .Center()
                            .Row(0)
                            .Column(2)
                            .Font(bold: true),
                    }
                }.Row(0).Padding(20),
            }
        };
    }

    IView CreateHeaderView()
    {
        return new Grid
        {
            Children = { new Border
                {
                    BackgroundColor = Colors.DarkRed.WithAlpha(.3f),
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
                        Children = { new Label
                            {
                                Text = BindingContext.StringLocalizer["TableNumberTitle"],
                                LineBreakMode = LineBreakMode.TailTruncation,
                                MaxLines = 1
                            }.Center().Column(0).Font(size: 16), new Label
                            {
                                Text = BindingContext.StringLocalizer["SizeTitle"],
                                LineBreakMode = LineBreakMode.TailTruncation,
                                MaxLines = 1
                            }.Center().Column(1).Font(size: 16), new Label
                            {
                                Text = BindingContext.StringLocalizer["PriorityTitle"],
                                LineBreakMode = LineBreakMode.TailTruncation,
                                MaxLines = 1
                            }.Center().Column(2).Font(size: 16), new Label
                            {
                                Text = $"{BindingContext.StringLocalizer["CleanTitle"]}?",
                                LineBreakMode = LineBreakMode.TailTruncation,
                                MaxLines = 1
                            }.Center().Column(3).Font(size: 16), }
                    }
                }.Padding(20).Margin(10) }
        }.Row(1).Bind(ActivityIndicator.IsVisibleProperty, (MainViewModel vm) => !vm.IsEmpty);
    }
}
