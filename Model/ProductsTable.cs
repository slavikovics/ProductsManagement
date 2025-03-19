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
        _xmlDoc = new XmlDocument();
        _xmlDoc.Load(_xmlPath);
        Products = new ObservableCollection<Product>();
        
        XmlNodeList? productNodes = _xmlDoc.SelectNodes("/Products/Product");

        if (productNodes == null)
        {
            LoadProductsFromDatabase();
            return;
        }
        
        foreach (XmlNode productNode in productNodes)
        {
            try
            {
                Product newProduct = new Product
                {
                    Name = productNode["Name"].InnerText,
                    ManufacturerName = productNode["ManufacturerName"].InnerText,
                    ManufacturerUnp = productNode["ManufacturerUnp"].InnerText,
                    StorageQuantity = Convert.ToInt32(productNode["StorageQuantity"].InnerText),
                    Address = productNode["Address"].InnerText
                };
                Products.Add(newProduct);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot read product from xml file.");
            }
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
            addressElement.InnerText = product.StorageQuantity.ToString();
            productElement.AppendChild(addressElement);

            
            root.AppendChild(productElement);
        }

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream, Encoding.UTF8);
        xmlDoc.Save(writer);
        await writer.FlushAsync();
    }
}