using ConsolShopV2.Contexts;
using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Services;

internal class ProductService
{
    private readonly DataContext _context;

    // konstruktor som tar emot en DataContext och lagrar den för användning i tjänsten, CRUD för products
    public ProductService(DataContext context)
    {
        _context = context;
    }

    // ,etod för att skapa en ny produkt i databasen. 
    public async Task<ProductEntity> CreateAsync(ProductEntity productEntity)
    {
        // kontrollerar om en pordukt med samma namn redan finns i databasen
        if (!await _context.Products.AnyAsync(x => x.ProductName == productEntity.ProductName))
        {
            // lägger till den nya produkten i databasen och sparar ändringar
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            Console.WriteLine("Produkt skapad och sparad!");
            Console.ReadKey();

            return productEntity;
        }

        Console.WriteLine("Produkten med det angivna namnet finns redan i databasen.");
        Console.ReadKey();
        return null!;
    }

    public async Task<ProductEntity> UpdateAsync(string productName, ProductEntity updatedProduct)
    {
        // hämtar den befintliga produkten från databasen baserat på produktens namn
        var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == productName);

        if (existingProduct != null)
        {
            // uppdaterar informationen om den befintliga produkten och sparar ändringarna.
            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.ProductDescription = updatedProduct.ProductDescription;
            existingProduct.ProductCategoryId = updatedProduct.ProductCategoryId;

            await _context.SaveChangesAsync();

            Console.WriteLine($"Produkt (Namn: {productName}) uppdaterad!");
            Console.ReadKey();
            return existingProduct;
        }

        Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        Console.ReadKey();
        return null!;
    }

    // raderar befintlig produkt genom samma söksätt som ovan
    public async Task<bool> DeleteAsync(string productName)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == productName);

        if (existingProduct != null)
        {
            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Produkt (Namn: {productName}) borttagen!");
            Console.ReadKey();
            return true;
        }

        Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        Console.ReadKey();
        return false;
    }

    // hämtar befintlig produkt genom samma söksätt som ovan
    public async Task<ProductEntity> GetByNameAsync(string productName)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == productName);

        if (product != null)
        {
            Console.WriteLine($"Produktens uppgifter (Namn: {productName}):");
            Console.WriteLine($"Produktnamn: {product.ProductName}");
            Console.WriteLine($"Produktbeskrivning: {product.ProductDescription}");
            Console.WriteLine($"Produktkategori ID: {product.ProductCategoryId}");
            

            return product;
        }

        Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        Console.ReadKey();
        return null!;
    }

}
