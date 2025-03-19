namespace ProductsManagement;

public class HierarchicalTreeDataGridItem
{
    public HierarchicalTreeDataGridItem(Product product)
    {
        Name = product.Name;
        Children = new List<HierarchicalTreeDataGridItem>();
        Children.Add(new HierarchicalTreeDataGridItem("Название производителя", product.ManufacturerName));
        Children.Add(new HierarchicalTreeDataGridItem("УНП производителя", product.ManufacturerUnp));
        Children.Add(new HierarchicalTreeDataGridItem("Количество на складе", product.StorageQuantity.ToString()));
        Children.Add(new HierarchicalTreeDataGridItem("Адрес", product.Address));
    }

    private HierarchicalTreeDataGridItem(string value, string childValue)
    {
        Name = value;
        Children = [new HierarchicalTreeDataGridItem(childValue)];
    }

    private HierarchicalTreeDataGridItem(string value)
    {
        Name = value;
        Children = [];
    }

    public string? Name { get; set; }

    public List<HierarchicalTreeDataGridItem> Children { get; set; }
}