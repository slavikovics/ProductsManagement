namespace ProductsManagement;

public class Product
{
    public string Name {get; set;}
    
    public double Price {get; set;}
    
    public string ManufacturerUnp {get; set;}
    
    public string StorageQuantity {get; set;}
    
    public string Address {get; set;}

    public Product(string name, double price, string manufacturerUnp, int storageQuantity, string address)
    {
        Name = name;
        Price = price;
        ManufacturerUnp = manufacturerUnp;
        if (storageQuantity <= 0) StorageQuantity = "нет на складе";
        else StorageQuantity = storageQuantity.ToString() + " шт.";
        Address = address;
    }
}