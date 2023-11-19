using System.ComponentModel.DataAnnotations;

namespace ConsolShopV2.Models.Entities;

public class CustomerEntity
{
    [Key]

    public int CustomerId { get; set; }

    [Required]

    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]

    public string Email { get; set; } = null!;

    public List<AdressEntity> Addresses { get; set; } = new List<AdressEntity>();
}
