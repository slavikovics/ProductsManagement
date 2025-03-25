using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using Avalonia.Platform.Storage;
using Exception = System.Exception;

namespace Model;

public class ProductsTable
{
    public ObservableCollection<Product> Products { get; private set; }
    
    private readonly ProductsContext _context;

    private readonly string _xmlPath; 
    
    private XmlDocument _xmlDoc;

    public ProductsTable(string xmlPath = "")
    {
        _context = new ProductsContext();
        _xmlDoc = new XmlDocument();
        _xmlPath = xmlPath;

        if (xmlPath == "") LoadProductsFromDatabase();
        else LoadProductsFromXml();
    }

    private void LoadProductsFromDatabase()
    {
        Products = new ObservableCollection<Product>(_context.Products);
        Products.CollectionChanged += UpdateDatabaseContext;
    }

    private void LoadProductsFromXml()
    {
        Products = new ObservableCollection<Product>();
        try
        {
            using (XmlReader reader = XmlReader.Create(_xmlPath))
            {
                Product? newProduct = null;
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Product":
                                newProduct = new Product();
                                break;
                            case "Name":
                                if (reader.Read() && newProduct != null)
                                    newProduct.Name = reader.Value;
                                break;
                            case "ManufacturerName":
                                if (reader.Read() && newProduct != null)
                                    newProduct.ManufacturerName = reader.Value;
                                break;
                            case "ManufacturerUnp":
                                if (reader.Read() && newProduct != null)
                                    newProduct.ManufacturerUnp = reader.Value;
                                break;
                            case "StorageQuantity":
                                if (reader.Read() && newProduct != null)
                                    newProduct.StorageQuantity = Convert.ToInt32(reader.Value);
                                break;
                            case "Address":
                                if (reader.Read() && newProduct != null)
                                    newProduct.Address = reader.Value;
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Product")
                    {
                        if (newProduct != null)
                        {
                            Products.Add(newProduct);
                        }
                    }
                }
            }
        }
        
        catch (Exception ex)
        {
            throw new Exception("Cannot read product from XML file.", ex);
        }
    }

    private void UpdateDatabaseContext(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            foreach (Product newItem in e.NewItems) _context.Products.Add(newItem);
        else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            foreach (Product oldItem in e.OldItems) _context.Products.Remove(oldItem);
        
        _context.SaveChanges();
    }

    public async Task SaveXmlFile(IStorageFile file)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("Products");
        xmlDoc.AppendChild(root);

        foreach (var product in Products)
        {
            XmlElement productElement = xmlDoc.CreateElement("Product");

            XmlElement nameElement = xmlDoc.CreateElement("Name");
            nameElement.InnerText = product.Name;
            productElement.AppendChild(nameElement);

            XmlElement manufacturerElement = xmlDoc.CreateElement("ManufacturerName");
            manufacturerElement.InnerText = product.ManufacturerName;
            productElement.AppendChild(manufacturerElement);

            XmlElement unpElement = xmlDoc.CreateElement("ManufacturerUnp");
            unpElement.InnerText = product.ManufacturerUnp;
            productElement.AppendChild(unpElement);
            
            XmlElement quantityElement = xmlDoc.CreateElement("StorageQuantity");
            quantityElement.InnerText = product.StorageQuantity.ToString();
            productElement.AppendChild(quantityElement);
            
            XmlElement addressElement = xmlDoc.CreateElement("Address");
            addressElement.InnerText = product.Address;
            productElement.AppendChild(addressElement);

            
            root.AppendChild(productElement);
        }

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream, Encoding.UTF8);
        xmlDoc.Save(writer);
        await writer.FlushAsync();
    }
}