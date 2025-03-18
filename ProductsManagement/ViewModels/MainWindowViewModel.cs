using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
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
        
        private readonly ProductsTable _productsTable;

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
        
        private bool _isTreeViewSelected = false;
        public bool IsTreeViewSelected
        {
            get => _isTreeViewSelected;
            private set => SetProperty(ref _isTreeViewSelected, value);
        }
        
        private bool _isTableSelected = true;
        public bool IsTableSelected
        {
            get => _isTableSelected;
            private set => SetProperty(ref _isTableSelected, value);
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

        private HierarchicalTreeDataGridSource<HierarchicalTreeDataGridItem> _treeDataGrid;
        public HierarchicalTreeDataGridSource<HierarchicalTreeDataGridItem> HierarchicalTreeDataGridSource
        {
            get => _treeDataGrid;
            set => SetProperty(ref _treeDataGrid, value);
        }

        public RelayCommand AddProductCommand { get; }
        
        public RelayCommand NextPageCommand { get; }
        
        public RelayCommand PreviousPageCommand { get; }
        
        public RelayCommand ComboboxNewItemCommand { get; }
        
        public RelayCommand SelectTreeViewCommand { get; }
        
        public RelayCommand SelectTableCommand { get; }

        public List<string> ComboboxItems { get; } = ["5", "10", "15", "20"];

        private Dictionary<int, int> ProductsPerPageDictionary { get; } = new Dictionary<int, int>()
        {
            {0, 5},
            {1, 10},
            {2, 15},
            {3, 20}
        };

        private List<HierarchicalTreeDataGridItem> MakeHierarchicalTreeDataGrid(List<Product> products)
        {
            List<HierarchicalTreeDataGridItem> hierarchicalTreeDataGridItems = new List<HierarchicalTreeDataGridItem>();
            foreach (var product in products)
            {
                hierarchicalTreeDataGridItems.Add(new HierarchicalTreeDataGridItem(product));
            }

            return hierarchicalTreeDataGridItems;
        }

        private void InitializeHierarchicalTreeDataGrid()
        {
            var items = MakeHierarchicalTreeDataGrid(ProductsPage.ToList());
            HierarchicalTreeDataGridSource = new HierarchicalTreeDataGridSource<HierarchicalTreeDataGridItem>(items)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<HierarchicalTreeDataGridItem>(new TextColumn<HierarchicalTreeDataGridItem, string>("Название товара", item => item.Name), 
                        item => item.Children)
                }
            };
        }

        public MainWindowViewModel()
        {
            _productsTable = new ProductsTable();
            ProductsPage = new ObservableCollection<Product>();
            AddProductCommand = new RelayCommand(AddProduct);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ComboboxNewItemCommand = new RelayCommand(ComboboxNewItemSelected);
            SelectTableCommand = new RelayCommand(SelectTableView);
            SelectTreeViewCommand = new RelayCommand(SelectTreeView);
            
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            RebuildTable();
            InitializeHierarchicalTreeDataGrid();
        }

        private void SelectTreeView()
        {
            IsTreeViewSelected = true;
            IsTableSelected = false;
        }

        private void SelectTableView()
        {
            IsTreeViewSelected = false;
            IsTableSelected = true;
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
            
            InitializeHierarchicalTreeDataGrid();
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
