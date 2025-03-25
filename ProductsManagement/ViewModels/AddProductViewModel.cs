using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using ProductsManagement.Assets;

namespace ProductsManagement.ViewModels;

public partial class AddProductViewModel : ViewModelBase
{
    [ObservableProperty] private string _nameContent = Resources.Name;
        
    [ObservableProperty] private string _manufacturerNameContent = Resources.ManufacturerName;
        
    [ObservableProperty] private string _manufacturerUnpContent = Resources.ManufacturerUNP;
        
    [ObservableProperty] private string _addressContent = Resources.Address;
        
    [ObservableProperty] private string _storageQuantityContent = Resources.StorageQuantity;
    
    [ObservableProperty] private string _addProductContent = Resources.AddProduct;
    
    [ObservableProperty] private string _name = "";

    [ObservableProperty] private string _manufacturerName = "";

    [ObservableProperty] private string _manufacturerUnp = "";

    [ObservableProperty] private string _storageQuantity = "";

    [ObservableProperty] private string _address = "";

    private readonly ObservableCollection<Product> _products;

    [ObservableProperty] private bool _isValid = false;

    public AddProductViewModel(ObservableCollection<Product> products)
    {
        _products = products;
    }
    
    public void Validate()
    {
        if (Name == "" || ManufacturerName == "" || ManufacturerUnp == "" || StorageQuantity == "" || Address == "")
        {
            IsValid = false;
            return;
        }

        try
        {
            int value = Convert.ToInt32(StorageQuantity);
            value = Convert.ToInt32(ManufacturerUnp);
        }
        catch (Exception e)
        {
            IsValid = false;
            return;
        }
        
        IsValid = true;
    }

    [RelayCommand]
    private void AddProduct()
    {
        try
        {
            Product product = new Product(
                Name, 
                ManufacturerName, 
                ManufacturerUnp, 
                Convert.ToInt32(StorageQuantity), 
                Address);
            
            _products.Add(product);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }
}