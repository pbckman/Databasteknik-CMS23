using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Contexts;

public class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Skoluppgifter\Databasteknik\Inlämningsuppgift_ver_3\Databasteknik_03\ConsolShopV2\Contexts\consolshopV2_database.mdf;Integrated Security=True;Connect Timeout=30");
    }

    public DbSet<AdressEntity> Adresses { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<ReviewEntity> Reviews { get; set; }
}
