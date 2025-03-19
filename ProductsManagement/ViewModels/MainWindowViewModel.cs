﻿using System;
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
            if (file != null)
            {
                // TODO implement file loading
            }
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

        private void SwitchToNewTable()
        {
            
        }
    }
}
