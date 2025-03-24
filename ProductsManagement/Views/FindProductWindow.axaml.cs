using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views;

public partial class FindProductWindow : Window
{
    public FindProductWindow()
    {
        InitializeComponent();
    }

    private void TextBoxOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (this.DataContext is FindProductViewModel findProductViewModel)
        {
            findProductViewModel.Find();
        }
    }
}