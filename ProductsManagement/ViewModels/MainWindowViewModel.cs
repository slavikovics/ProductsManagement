using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProductsManagement.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Header { get; } = "Products Management";

        public ObservableCollection<Product> Products { get; set; }
        
        public MainWindowViewModel()
        {
            //var products = new List<Product>();
            //Product p1 = new Product("продукт 1", 123.46, "3874728397423894723", 15, "г. Минск, пр. Независимости, 54");
            //Product p2 = new Product("продукт 2", 85.45, "3874728397423894723", 0, "г. Минск, пр. Независимости, 14");
            //Product p3 = new Product("продукт 3", 67.46, "3874728397423894723", 4, "г. Минск, пр. Независимости, 25");

            ProductsContext context = new ProductsContext();
            
            //products.Add(p1);
            //context.Products.Add(p1);
            //products.Add(p2);
            //.Products.Add(p2);
            //products.Add(p3);
            //context.Products.Add(p3);
            //context.SaveChanges();
            Products = new ObservableCollection<Product>(context.Products);
        }
    }
}
