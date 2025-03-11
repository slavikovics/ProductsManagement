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
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Header { get; } = "Управление товарами";

        private ObservableCollection<Product> ProductsTable { get; set; }
        
        public ObservableCollection<Product> ProductsPage { get; set; }

        private readonly int _firstPageNumber;
        public int FirstPageNumber
        {
            get => _firstPageNumber;
            private init => SetProperty(ref _firstPageNumber, value);
        }

        private int _selectedPageNumber;
        public int SelectedPageNumber
        {
            get => _selectedPageNumber;
            private set => SetProperty(ref _selectedPageNumber, value);
        }

        private int _lastPageNumber;
        public int LastPageNumber
        {
            get => _lastPageNumber;
            private set => SetProperty(ref _lastPageNumber, value);
        }
        
        private int ProductsPerPage { get; set; }

        private bool _isNextPageEnabled = true;
        public bool IsNextPageEnabled
        {
            get => _isNextPageEnabled;
            private set => SetProperty(ref _isNextPageEnabled, value);
        }

        private bool _isPreviousPageEnabled = true;
        public bool IsPreviousPageEnabled
        {
            get => _isNextPageEnabled;
            private set => SetProperty(ref _isPreviousPageEnabled, value);
        }

        private int _comboboxSelectedIndex;
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

        private readonly ProductsContext _context;

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
            _context = new ProductsContext();
            ProductsTable = new ObservableCollection<Product>(_context.Products);
            
            ProductsPage = new ObservableCollection<Product>();
            ProductsTable.CollectionChanged += UpdateContext;
            AddProductCommand = new RelayCommand(AddProduct);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ComboboxNewItemCommand = new RelayCommand(ComboboxNewItemSelected);
            
            FirstPageNumber = 1;
            SelectedPageNumber = 1;
            ComboboxSelectedIndex = 1;
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            RebuildTable();
        }
        
        private void UpdateContext(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Product newItem in e.NewItems)
                {
                    _context.Products.Add(newItem);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Product oldItem in e.OldItems)
                {
                    _context.Products.Remove(oldItem);
                }
            }

            RebuildTable();
            _context.SaveChanges();
        }

        private void RebuildTable()
        {
            LastPageNumber = (int) Math.Ceiling((double)ProductsTable.Count / ProductsPerPage);
            if (ProductsPage.Count != ProductsPerPage) SelectedPageNumber = 1;
            
            ProductsPage.Clear();
            int itemsCountOnPage = Math.Min(ProductsTable.Count - (SelectedPageNumber - 1) * ProductsPerPage, ProductsPerPage);
            List<Product> currentSelection = ProductsTable.ToList()
                .GetRange((SelectedPageNumber - 1) * ProductsPerPage, itemsCountOnPage);
            
            foreach (Product product in currentSelection) ProductsPage.Add(product);
            
            if (SelectedPageNumber == LastPageNumber) IsNextPageEnabled = false;
            if (SelectedPageNumber == FirstPageNumber) IsPreviousPageEnabled = false;
        }

        private void AddProduct()
        {
            AddProductWindow addProductWindow = new AddProductWindow
            {
                DataContext = new AddProductViewModel(ProductsTable)
            };
            addProductWindow.Show();
        }

        private void NextPage()
        {
            if (SelectedPageNumber >= LastPageNumber) return;
            
            SelectedPageNumber++;
            if (SelectedPageNumber == LastPageNumber) IsNextPageEnabled = false;
            if (SelectedPageNumber == FirstPageNumber + 1) IsPreviousPageEnabled = true;
            RebuildTable();
        }

        private void PreviousPage()
        {
            if (SelectedPageNumber <= FirstPageNumber) return;
            
            SelectedPageNumber--;
            if (SelectedPageNumber == FirstPageNumber) IsPreviousPageEnabled = false;
            if (SelectedPageNumber == LastPageNumber - 1) IsNextPageEnabled = true;
            RebuildTable();
        }

        private void ComboboxNewItemSelected()
        {
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            RebuildTable();
        }
    }
}
