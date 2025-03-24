using Bogus;

namespace Model;

public class ExampleData
{
    public ExampleData()
    {
        GenerateProducts();
    }
    
    private void GenerateProducts()
    {
        Faker<Product> faker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.ManufacturerName, f => f.Company.CompanyName())
            .RuleFor(p => p.StorageQuantity, f => f.Random.Even(0, 10000))
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.ManufacturerUnp, f => f.Random.Even(111111111, 999999999).ToString());

        List<Product> products = faker.Generate(150);

        ProductsContext context = new ProductsContext();
        RemoveAllProducts(context);

        context.AddRange(products);
        context.SaveChanges();
    }

    private void RemoveAllProducts(ProductsContext context)
    {
        var prevProducts = context.Products;
        context.Products.RemoveRange(prevProducts);
        context.SaveChanges();
    }
}