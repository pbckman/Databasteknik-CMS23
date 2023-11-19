using ConsolShopV2.Contexts;
using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Services;

// CRUD för kategorier
internal class ProductCategoryService
{
    private readonly DataContext _context;

    public ProductCategoryService(DataContext context)
    {
        _context = context;
    }

    public async Task<ProductCategoryEntity> CreateAsync(ProductCategoryEntity categoryEntity)
    {
        _context.ProductCategories.Add(categoryEntity);
        await _context.SaveChangesAsync();
        return categoryEntity;
    }

    public async Task<List<ProductCategoryEntity>> GetAllAsync()
    {
        return await _context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategoryEntity> GetByNameAsync(string categoryName)
    {
        return await _context.ProductCategories.FirstOrDefaultAsync(category => category.CategoryName == categoryName);
    }

    public async Task<ProductCategoryEntity> DeleteAsync(ProductCategoryEntity categoryEntity)
    {
        _context.ProductCategories.Remove(categoryEntity);
        await _context.SaveChangesAsync();
        return categoryEntity;
    }

    public async Task<ProductCategoryEntity> UpdateCategoryAsync(ProductCategoryEntity categoryEntity)
    {
        _context.ProductCategories.Update(categoryEntity);
        await _context.SaveChangesAsync();
        return categoryEntity;
    }


}
