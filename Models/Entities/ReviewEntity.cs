using System.ComponentModel.DataAnnotations;

namespace ConsolShopV2.Models.Entities;

public class ReviewEntity
{
    [Key]

    public int ReviewId { get; set; }

    [Required]

    public int ProductId { get; set; }

    public ProductEntity Product { get; set; } = null!;


    [Required]

    public int Rating { get; set; }

    [Required]

    public string ReviewText { get; set; } = null!;
}
