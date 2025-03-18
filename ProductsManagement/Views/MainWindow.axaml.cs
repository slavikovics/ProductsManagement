using System.Diagnostics;
using Avalonia.Controls;
using ProductsManagement.ViewModels;

namespace ProductsManagement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
            Debug.WriteLine(this.DataContext != null ? "DataContext is set." : "DataContext is null.");
        }
    }
}