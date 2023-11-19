using System.ComponentModel.DataAnnotations;

namespace ConsolShopV2.Models.Entities;

public class AdressEntity
{
    [Key]

    public int AdressId { get; set; }

    [Required]

    public int CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    [Required]

    public string StreetName { get; set; } = null!;

    [Required]
    public int PostalCode { get; set; }

    [Required]

    public string City { get; set; } = null!;
}
