using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Model;

namespace ProductsManagement.ViewModels;

public partial class FindProductViewModel: ViewModelBase
{
    [ObservableProperty] private string _name = "";

    [ObservableProperty] private string _manufacturerName = "";

    [ObservableProperty] private string _manufacturerUnp = "";

    [ObservableProperty] private string _storageQuantity = "";

    [ObservableProperty] private string _address = "";

    private ProductsContext _context { get; }

    public ObservableCollection<Product> ProductsPage { get; set; }
    
    public FindProductViewModel()
    {
        _context = new ProductsContext();
        ProductsPage = new ObservableCollection<Product>();
    }

    public void Find()
    {
        ProductsPage.Clear();

        var foundProducts = _context.Products
            .Where(x => x.Name.Contains(Name) &&
                        x.ManufacturerName.Contains(ManufacturerName) &&
                        x.ManufacturerUnp.Contains(ManufacturerUnp) &&
                        x.Address.Contains(Address) &&
                        x.StorageQuantity.ToString().Contains(StorageQuantity)).ToList();

        foreach (var product in foundProducts) ProductsPage.Add(product);
    }
}