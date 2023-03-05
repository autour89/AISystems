using System;
using Laba1.ViewModels;

namespace Laba1.Views;

public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel
{
    protected BaseContentPage(T viewModel, string pageTitle)
    {
        base.BindingContext = viewModel;

        Title = pageTitle;
    }

    protected new T BindingContext => (T)base.BindingContext;
}