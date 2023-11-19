using ConsolShopV2.Contexts;
using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Services;

// CRUD för customers
internal class CustomerService
{
    private readonly DataContext _context;

    public CustomerService(DataContext context)
    {
        _context = context;
    }

    public async Task<CustomerEntity> CreateAsync(CustomerEntity customerEntity)
    {
        if (!await _context.Customers.AnyAsync(x => x.Email == customerEntity.Email))
        {
            _context.Customers.Add(customerEntity);
            await _context.SaveChangesAsync();

            return customerEntity;
        }

        Console.WriteLine("Kunden med den angivna e-postadressen finns redan i databasen.");
        return null!;
    }

    public async Task<CustomerEntity> UpdateAsync(string email, CustomerEntity updatedCustomer)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

        if (existingCustomer != null)
        {
            existingCustomer.FirstName = updatedCustomer.FirstName;
            existingCustomer.LastName = updatedCustomer.LastName;
            existingCustomer.Email = updatedCustomer.Email;

            await _context.SaveChangesAsync();

            return existingCustomer;
        }

        Console.WriteLine($"Kund med e-postadressen {email} hittades inte.");
        Console.ReadKey();
        return null!;
    }

    public async Task<bool> DeleteAsync(string email)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

        if (existingCustomer != null)
        {
            _context.Customers.Remove(existingCustomer);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Kund (E-postadress: {email}) borttagen!");
            return true;
        }

        Console.WriteLine($"Kund med e-postadressen {email} hittades inte.");
        return false;
    }

    public async Task<CustomerEntity> GetByEmailAsync(string email)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

        if (customer != null)
        {
            Console.WriteLine($"Kundens uppgifter (E-postadress: {email}):");
            Console.WriteLine($"Förnamn: {customer.FirstName}");
            Console.WriteLine($"Efternamn: {customer.LastName}");

            return customer;
        }

       Console.WriteLine($"Kund med e-postadressen {email} hittades inte.");
        Console.ReadKey();
        return null!;
    }

}
