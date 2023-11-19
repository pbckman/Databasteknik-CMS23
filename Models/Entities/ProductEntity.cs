using System.ComponentModel.DataAnnotations;

namespace ConsolShopV2.Models.Entities;

public class ProductEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    public string ProductDescription { get; set; } = null!;

    
    public int? ProductCategoryId { get; set; }

    public ProductCategoryEntity ProductCategory { get; set; } = null!;

}
