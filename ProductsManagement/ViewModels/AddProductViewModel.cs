using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;

namespace ProductsManagement.ViewModels;

public partial class AddProductViewModel : ViewModelBase
{
    [ObservableProperty] private string _name = "название";

    [ObservableProperty] private string _manufacturerName = "название производителя";

    [ObservableProperty] private string _manufacturerUnp = "унп производителя";

    [ObservableProperty] private string _storageQuantity = "количество";

    [ObservableProperty] private string _address = "адрес";

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