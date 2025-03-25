using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Model;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ExampleData data = new ExampleData();
            this.DataContext = new MainWindowViewModel(this);
            Debug.WriteLine(this.DataContext != null ? "DataContext is set." : "DataContext is null.");
        }

        private async void OpenFileDialog(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                await mainWindowViewModel.OpenFileAsync(this);
            }
        }

        private async void SaveFileDialog(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                await mainWindowViewModel.SaveFileAsync(this);
            }
        }
    }
}