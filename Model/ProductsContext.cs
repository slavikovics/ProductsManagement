using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ProductsManagement;

public class ProductsContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;");
    }
}