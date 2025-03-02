using System.ComponentModel.DataAnnotations;

namespace ProductsManagement;

public class Product
{
    [Key] 
    public int Id { get; set; }

    public string Name {get; set;}
    
    public double Price {get; set;}
    
    public string ManufacturerUnp {get; set;}
    
    public int StorageQuantity {get; set;}
    
    public string Address {get; set;}

    public Product(string name, double price, string manufacturerUnp, int storageQuantity, string address)
    {
        Name = name;
        Price = price;
        ManufacturerUnp = manufacturerUnp;
        StorageQuantity = storageQuantity;
        Address = address;
    }
}