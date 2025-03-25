using System.Collections.Generic;
using ProductsManagement.Assets;

namespace Model;

public class HierarchicalTreeDataGridItem
{
    public HierarchicalTreeDataGridItem(Product product)
    {
        Name = product.Name;
        Children = new List<HierarchicalTreeDataGridItem>();
        Children.Add(new HierarchicalTreeDataGridItem(Resources.ManufacturerName, product.ManufacturerName));
        Children.Add(new HierarchicalTreeDataGridItem(Resources.ManufacturerUNP, product.ManufacturerUnp));
        Children.Add(new HierarchicalTreeDataGridItem(Resources.StorageQuantity, product.StorageQuantityRecord));
        Children.Add(new HierarchicalTreeDataGridItem(Resources.Address, product.Address));
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