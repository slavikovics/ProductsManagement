using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views;

public partial class AddProductWindow : Window
{
    public AddProductWindow()
    {
        InitializeComponent();
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (this.DataContext is AddProductViewModel addProductViewModel)
        {
            addProductViewModel.Validate();
        }
    }
}