using Laba2.Helpers;
using Laba2.ViewModels;

namespace Laba2.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}