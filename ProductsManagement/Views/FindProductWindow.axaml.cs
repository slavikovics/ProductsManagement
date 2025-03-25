using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views;

public partial class FindProductWindow : Window
{
    public FindProductWindow(ProductsTable table, bool isDeletionEnabled = false)
    {
        InitializeComponent();
        this.DataContext = new FindProductViewModel(table, isDeletionEnabled);
    }

    private void TextBoxOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (this.DataContext is FindProductViewModel findProductViewModel)
        {
            findProductViewModel.Find();
        }
    }
}