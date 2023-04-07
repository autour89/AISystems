using System;
using Laba2.ViewModels;

namespace Laba2.Views;

public abstract class BaseContentPage<T> : ContentPage where T : BaseViewModel
{
    protected BaseContentPage(T viewModel, string pageTitle)
    {
        base.BindingContext = viewModel;

        Title = pageTitle;
    }

    protected new T BindingContext => (T)base.BindingContext;
}