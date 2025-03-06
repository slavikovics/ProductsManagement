using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public ObservableCollection<Product> ProductsTable { get; set; }
        
        public ObservableCollection<Product> ProductsPage { get; set; }

        public int FirstPageNumber { get; private set; }

        public int SelectedPageNumber { get; private set; }

        public int LastPageNumber { get; private set; }
        
        public int ProductsPerPage { get; private set; }

        public bool IsNextPageEnabled { get; private set; } = true;
        
        public bool IsPreviousPageEnabled { get; private set; } = true;

        public RelayCommand AddProductCommand { get; }
        
        public RelayCommand NextPageCommand { get; }
        
        public RelayCommand PreviousPageCommand { get; }

        private readonly ProductsContext _context;

        public MainWindowViewModel()
        {
            _context = new ProductsContext();
            ProductsTable = new ObservableCollection<Product>(_context.Products);
            
            ProductsPage = new ObservableCollection<Product>();
            ProductsTable.CollectionChanged += UpdateContext;
            AddProductCommand = new RelayCommand(AddProduct);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            
            FirstPageNumber = 1;
            SelectedPageNumber = 1;
            ProductsPerPage = 10;
            LastPageNumber = (int)Math.Ceiling((double)ProductsTable.Count / ProductsPerPage);
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
                RebuildTable();
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Product oldItem in e.OldItems)
                {
                    _context.Products.Remove(oldItem);
                }
                RebuildTable();
            }

            _context.SaveChanges();
        }

        private void RebuildTable()
        {
            ProductsPage.Clear();
            int itemsCountOnPage = Math.Min(ProductsTable.Count - (SelectedPageNumber - 1) * ProductsPerPage, ProductsPerPage);
            List<Product> currentSelection = ProductsTable.ToList()
                .GetRange((SelectedPageNumber - 1) * ProductsPerPage, itemsCountOnPage);
            
            foreach (Product product in currentSelection) ProductsPage.Add(product);
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
    }
}
