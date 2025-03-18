using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            Debug.WriteLine(this.DataContext != null ? "DataContext is set." : "DataContext is null.");
        }

        private async void OpenFileDialog(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowViewModel)
            {
                await mainWindowViewModel.OpenFileAsync(this);
            }
        }
    }
}