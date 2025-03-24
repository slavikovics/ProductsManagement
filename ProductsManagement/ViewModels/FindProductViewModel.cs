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

    public ProductsContext Context { get; }

    public ObservableCollection<Product> ProductsPage { get; set; }
    
    public FindProductViewModel()
    {
        Context = new ProductsContext();
        ProductsPage = new ObservableCollection<Product>();
    }

    public void Find()
    {
        ProductsPage.Clear();
        var foundProducts = Context.Products.Where(x => x.Name.Contains(Name))
            .Where(x => x.ManufacturerName.Contains(ManufacturerName))
            .Where(x => x.ManufacturerUnp.Contains(ManufacturerUnp))
            .Where(x => x.Address.Contains(Address))
            .Where(x => x.StorageQuantity.ToString().Contains(StorageQuantity));

        foreach (var product in foundProducts) ProductsPage.Add(product);
    }
}