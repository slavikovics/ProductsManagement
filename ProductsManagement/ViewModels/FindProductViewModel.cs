using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using ProductsManagement.Assets;

namespace ProductsManagement.ViewModels;

public partial class FindProductViewModel: ViewModelBase
{
    [ObservableProperty] private string _header = Resources.FindProducts;
    
    [ObservableProperty] private string _totalProducts = Resources.TotalProducts;
    
    [ObservableProperty] private string _nameContent = Resources.Name;
        
    [ObservableProperty] private string _manufacturerNameContent = Resources.ManufacturerName;
        
    [ObservableProperty] private string _manufacturerUnpContent = Resources.ManufacturerUNP;
        
    [ObservableProperty] private string _addressContent = Resources.Address;
        
    [ObservableProperty] private string _storageQuantityContent = Resources.StorageQuantity;
    
    [ObservableProperty] private string _addProductContent = Resources.AddProduct;
    
    [ObservableProperty] private string _totalSelected = Resources.TotalSelected;
    
    [ObservableProperty] private string _deleteChosenProducts = Resources.DeleteChosenProducts;
    
    [ObservableProperty] private string _name = "";

    [ObservableProperty] private string _manufacturerName = "";

    [ObservableProperty] private string _manufacturerUnp = "";

    [ObservableProperty] private string _storageQuantity = "";

    [ObservableProperty] private string _address = "";
    
    [ObservableProperty] private bool _isDeletingEnabled = false;

    private readonly ProductsTable _productsTable;

    public ObservableCollection<Product> FoundProducts { get; set; }
    
    public FindProductViewModel(ProductsTable table, bool deletion = false)
    {
        _productsTable = table;
        FoundProducts = new ObservableCollection<Product>();

        if (deletion)
        {
            Header = Resources.RemoveProductRes;
            IsDeletingEnabled = true;
        }
    }

    public void Find()
    {
        FoundProducts.Clear();
        List<Product> foundProducts;

        if (StorageQuantity != String.Empty)
        {
            foundProducts = _productsTable.Products
                .Where(x => x.Name.Contains(Name) &&
                            x.ManufacturerName.Contains(ManufacturerName) &&
                            x.ManufacturerUnp.Contains(ManufacturerUnp) &&
                            x.Address.Contains(Address) &&
                            x.StorageQuantity.ToString() == StorageQuantity).ToList();
        }
        else
        {
            foundProducts = _productsTable.Products
                .Where(x => x.Name.Contains(Name) &&
                            x.ManufacturerName.Contains(ManufacturerName) &&
                            x.ManufacturerUnp.Contains(ManufacturerUnp) &&
                            x.Address.Contains(Address)).ToList();
        } 

        foreach (var product in foundProducts) FoundProducts.Add(product);
    }

    [RelayCommand]
    public void DeleteSelection()
    {
        foreach (var product in FoundProducts) _productsTable.Products.Remove(product);
        Find();
    }
}