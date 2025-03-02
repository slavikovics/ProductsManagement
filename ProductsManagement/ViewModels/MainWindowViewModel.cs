﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Storage;
using ProductsManagement.Views;

namespace ProductsManagement.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Header { get; } = "Управление товарами";

        public ObservableCollection<Product> Products { get; set; }

        public RelayCommand AddProductCommand { get; }

        private readonly ProductsContext _context;

        public MainWindowViewModel()
        {
            _context = new ProductsContext();
            Products = new ObservableCollection<Product>(_context.Products);
            Products.CollectionChanged += UpdateContext;
            AddProductCommand = new RelayCommand(AddProduct);
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

            _context.SaveChanges();
        }

        private void AddProduct()
        {
            AddProductWindow addProductWindow = new AddProductWindow
            {
                DataContext = new AddProductViewModel(Products)
            };
            addProductWindow.Show();
        }
    }
}
