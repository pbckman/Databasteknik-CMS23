using ConsolShopV2.Contexts;
using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Services;

// CRUD för adresser
internal class AdressService
{
    private readonly DataContext _context;

    public AdressService(DataContext context)
    {
        _context = context;
    }

    public async Task<AdressEntity> CreateAndAssociateWithCustomerAsync(int customerId, AdressEntity addressEntity)
    {
        // hämtar den befintliga kunden afrån databasen baserat på kundens id genom FindAsync metoden
        var existingCustomer = await _context.Customers.FindAsync(customerId);

        // kontrollerar om kunden finns i databasen
        if (existingCustomer != null)
        {
            // kopplar kundID till nya adressen
            addressEntity.CustomerId = customerId;
            // lägger till nya adressen till listan över kundens adresser
            existingCustomer.Addresses.Add(addressEntity);

            // lägger till nya adressen i adressentiteten och sparar till databasen
            _context.Adresses.Add(addressEntity);
            await _context.SaveChangesAsync();

            Console.WriteLine("Adress skapad och kopplad till kunden!");
            Console.ReadKey();

            return addressEntity;
        }

        return null!;
    }

    public async Task<AdressEntity> UpdateAsync(AdressEntity addressEntity)
    {
        var existingAddress = await _context.Adresses.FindAsync(addressEntity.AdressId);

        if (existingAddress != null)
        {
            existingAddress.StreetName = addressEntity.StreetName;
            existingAddress.PostalCode = addressEntity.PostalCode;
            existingAddress.City = addressEntity.City;

            await _context.SaveChangesAsync();

            Console.WriteLine("Adress uppdaterad!");
            Console.ReadKey();
            return existingAddress;
        }

        Console.WriteLine($"Adress med ID {addressEntity.AdressId} hittades inte.");
        Console.ReadKey();
        return null!;
    }

    public async Task<AdressEntity> GetByIdAsync(int addressId)
    {
        var address = await _context.Adresses.FindAsync(addressId);

        if (address != null)
        {
            Console.WriteLine($"Adressens uppgifter (ID: {addressId}):");
            Console.WriteLine($"Gatuadress: {address.StreetName}");
            Console.WriteLine($"Postnummer: {address.PostalCode}");
            Console.WriteLine($"Stad: {address.City}");

            return address;
        }

        Console.WriteLine($"Adress med ID {addressId} hittades inte.");
        return null!;
    }

    public async Task<bool> DeleteAsync(int addressId)
    {
        var addressToDelete = await _context.Adresses.FindAsync(addressId);

        if (addressToDelete != null)
        {
            _context.Adresses.Remove(addressToDelete);
            await _context.SaveChangesAsync();

            Console.WriteLine("Adress borttagen!");
            return true;
        }

        Console.WriteLine($"Adress med ID {addressId} hittades inte.");
        return false;
    }

    public async Task<List<AdressEntity>> GetAddressesByCustomerIdAsync(int customerId)
    {
        return await _context.Adresses
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();
    }
}
