using System.ComponentModel.DataAnnotations;

namespace ConsolShopV2.Models.Entities;

public class ProductCategoryEntity
{
    [Key]

    public int CategoryId { get; set; }

    [Required]

    public string CategoryName { get; set; } = null!;

    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
