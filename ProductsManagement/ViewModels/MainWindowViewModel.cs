using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Storage;
using ProductsManagement.Views;

namespace ProductsManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Header { get; } = "Управление товарами";
        
        public ObservableCollection<Product> ProductsPage { get; set; }
        
        private ProductsTable _productsTable;

        private readonly int _firstPageNumber = 1;
        public int FirstPageNumber
        {
            get => _firstPageNumber;
            private init => SetProperty(ref _firstPageNumber, value);
        }

        private int _selectedPageNumber = 1;
        public int SelectedPageNumber
        {
            get => _selectedPageNumber;
            private set => SetProperty(ref _selectedPageNumber, value);
        }

        private int _lastPageNumber = 1;
        public int LastPageNumber
        {
            get => _lastPageNumber;
            private set => SetProperty(ref _lastPageNumber, value);
        }
        
        private int ProductsPerPage { get; set; }

        private bool _isNextPageEnabled = false;
        public bool IsNextPageEnabled
        {
            get => _isNextPageEnabled;
            private set => SetProperty(ref _isNextPageEnabled, value);
        }

        private bool _isPreviousPageEnabled = false;
        public bool IsPreviousPageEnabled
        {
            get => _isPreviousPageEnabled;
            private set => SetProperty(ref _isPreviousPageEnabled, value);
        }

        private int _comboboxSelectedIndex = 1;
        public int ComboboxSelectedIndex 
        {
            get => _comboboxSelectedIndex;
            set
            {
                SetProperty(ref _comboboxSelectedIndex, value);
                ComboboxNewItemSelected();
            }
        }

        public RelayCommand AddProductCommand { get; }
        
        public RelayCommand NextPageCommand { get; }
        
        public RelayCommand PreviousPageCommand { get; }
        
        public RelayCommand ComboboxNewItemCommand { get; }

        public List<string> ComboboxItems { get; } = ["5", "10", "15", "20"];

        private Dictionary<int, int> ProductsPerPageDictionary { get; } = new Dictionary<int, int>()
        {
            {0, 5},
            {1, 10},
            {2, 15},
            {3, 20}
        };

        public MainWindowViewModel()
        {
            _productsTable = new ProductsTable();
            ProductsPage = new ObservableCollection<Product>();
            AddProductCommand = new RelayCommand(AddProduct);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ComboboxNewItemCommand = new RelayCommand(ComboboxNewItemSelected);
            
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            RebuildTable();
        }

        private void RebuildTable()
        {
            LastPageNumber = (int)Math.Ceiling((double)_productsTable.Products.Count / ProductsPerPage);

            ProductsPage.Clear();
            int itemsCountOnPage = Math.Min(_productsTable.Products.Count - (SelectedPageNumber - 1) * ProductsPerPage,
                ProductsPerPage);
            List<Product> currentSelection = _productsTable.Products.ToList()
                .GetRange((SelectedPageNumber - 1) * ProductsPerPage, itemsCountOnPage);

            foreach (Product product in currentSelection) ProductsPage.Add(product);

            IsNextPageEnabled = SelectedPageNumber != LastPageNumber;
            IsPreviousPageEnabled = SelectedPageNumber != FirstPageNumber;
        }

        private void AddProduct()
        {
            AddProductWindow addProductWindow = new AddProductWindow
            {
                DataContext = new AddProductViewModel(_productsTable.Products)
            };
            addProductWindow.Show();
        }

        private void NextPage()
        {
            if (SelectedPageNumber >= LastPageNumber) return;
            SelectedPageNumber++;
            RebuildTable();
        }

        private void PreviousPage()
        {
            if (SelectedPageNumber <= FirstPageNumber) return;
            SelectedPageNumber--;
            RebuildTable();
        }

        private void ComboboxNewItemSelected()
        {
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            SelectedPageNumber = FirstPageNumber;
            RebuildTable();
        }

        private void SwitchToNewTable()
        {
            
        }
    }
}
