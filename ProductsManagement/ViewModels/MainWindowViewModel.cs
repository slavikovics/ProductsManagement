using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client.Extensions.Msal;
using ProductsManagement.Views;

namespace ProductsManagement.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Header { get; } = "Управление товарами";
        
        public ObservableCollection<Product> ProductsPage { get; set; }
        
        private ProductsTable _productsTable;
        
        private int ProductsPerPage { get; set; }

        [ObservableProperty]
        private int _productsCount;
        
        [ObservableProperty] 
        private int _firstPageNumber = 1;

        [ObservableProperty]
        private int _selectedPageNumber = 1;

        [ObservableProperty]
        private int _lastPageNumber = 1;

        [ObservableProperty]
        private bool _isNextPageEnabled = false;

        [ObservableProperty]
        private bool _isPreviousPageEnabled = false;
        
        [ObservableProperty]
        private bool _isTreeViewSelected = false;
        
        [ObservableProperty]
        private bool _isTableSelected = true;
        
        private int _comboboxSelectedIndex = 1;
        public int ComboboxSelectedIndex 
        {
            get => _comboboxSelectedIndex;
            set
            {
                SetProperty(ref _comboboxSelectedIndex, value);
                ComboboxSelectNewItem();
            }
        }

        private HierarchicalTreeDataGridSource<HierarchicalTreeDataGridItem> _treeDataGrid;
        public HierarchicalTreeDataGridSource<HierarchicalTreeDataGridItem> HierarchicalTreeDataGridSource
        {
            get => _treeDataGrid;
            set => SetProperty(ref _treeDataGrid, value);
        }
        
        private readonly FilePickerService _filePickerService = new FilePickerService();

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
            _productsTable.Products.CollectionChanged += (sender, args) => RebuildTable();
            ProductsPage = new ObservableCollection<Product>();
            
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            RebuildTable();
            InitializeHierarchicalTreeDataGrid();
        }

        [RelayCommand]
        public void SelectTreeView()
        {
            IsTreeViewSelected = true;
            IsTableSelected = false;
        }

        [RelayCommand]
        public void SelectTableView()
        {
            IsTreeViewSelected = false;
            IsTableSelected = true;
        }
        
        [RelayCommand]
        public async Task OpenFileAsync(Window parent)
        {
            var file = await _filePickerService.OpenFileAsync(parent);
            if (file == null) return;
            
            _productsTable = new ProductsTable(file.Path.ToString());
            RebuildTable();
        }

        [RelayCommand]
        public async Task SaveFileAsync(Window parent)
        {
            await _filePickerService.SaveFileAsync(parent, _productsTable);
        }

        private void RebuildTable()
        {
            ProductsCount = _productsTable.Products.Count;
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

        [RelayCommand]
        public void AddProduct()
        {
            AddProductWindow addProductWindow = new AddProductWindow
            {
                DataContext = new AddProductViewModel(_productsTable.Products)
            };
            addProductWindow.Show();
        }

        [RelayCommand]
        public void NextPage()
        {
            if (SelectedPageNumber >= LastPageNumber) return;
            SelectedPageNumber++;
            RebuildTable();
        }

        [RelayCommand]
        public void PreviousPage()
        {
            if (SelectedPageNumber <= FirstPageNumber) return;
            SelectedPageNumber--;
            RebuildTable();
        }
        
        private void ComboboxSelectNewItem()
        {
            ProductsPerPage = ProductsPerPageDictionary[ComboboxSelectedIndex];
            SelectedPageNumber = FirstPageNumber;
            RebuildTable();
        }
    }
}
