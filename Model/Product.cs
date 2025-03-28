﻿using System.ComponentModel.DataAnnotations;

namespace Model;

public class Product
{
    [Key] 
    public int Id { get; set; }

    public string Name {get; set;}
    
    public string ManufacturerName {get; set;}
    
    public string ManufacturerUnp {get; set;}
    
    public int StorageQuantity {get; set;}

    public string StorageQuantityRecord => GetQuantityStringForTable();
    
    public string Address {get; set;}

    public Product(string name, string manufacturerName, string manufacturerUnp, int storageQuantity, string address)
    {
        Name = name;
        ManufacturerName = manufacturerName;
        ManufacturerUnp = manufacturerUnp;
        StorageQuantity = storageQuantity;
        Address = address;
    }

    public string GetQuantityStringForTable()
    {
        if (StorageQuantity != 0) return StorageQuantity.ToString();
        return "out of stock";
    }

    public Product()
    {
    }
}