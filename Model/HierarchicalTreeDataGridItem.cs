namespace ProductsManagement;

public class HierarchicalTreeDataGridItem
{
    public HierarchicalTreeDataGridItem(Product product)
    {
        Name = product.Name;
        Children = new List<HierarchicalTreeDataGridItem>();
        Children.Add(new HierarchicalTreeDataGridItem(product.ManufacturerName));
        Children.Add(new HierarchicalTreeDataGridItem(product.ManufacturerUnp));
        Children.Add(new HierarchicalTreeDataGridItem(product.StorageQuantity.ToString()));
        Children.Add(new HierarchicalTreeDataGridItem(product.Address));
    }

    public HierarchicalTreeDataGridItem(string value)
    {
        Name = value;
    }

    public string? Name { get; set; }

    public List<HierarchicalTreeDataGridItem> Children { get; set; }
}