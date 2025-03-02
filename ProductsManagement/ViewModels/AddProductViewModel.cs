using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace ProductsManagement.ViewModels;

public class AddProductViewModel : ViewModelBase
{
    public string Name { get; set; }
    
    public string ManufacturerName {get; set;}
    
    public string ManufacturerUnp {get; set;}
    
    public int StorageQuantity {get; set;}
    
    public string Address {get; set;}

    private readonly ObservableCollection<Product> _products;

    public RelayCommand AddProductCommand { get; }

    public AddProductViewModel(ObservableCollection<Product> products)
    {
        Address = "";
        Name = "";
        ManufacturerUnp = "";
        ManufacturerName = "";
        AddProductCommand = new RelayCommand(AddProduct);
        _products = products;
    }

    private void AddProduct()
    {
        try
        {
            Product product = new Product(Name, ManufacturerName, ManufacturerUnp, StorageQuantity, Address);
            _products.Add(product);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }
}