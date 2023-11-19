using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ConsolShopV2.Services;

// liknande funktioner (steg 1-4) som i ProductMenu där det finns fler förklarande kommentarer om vad som sker
// finns mer koomentarer för steg 5-8 längre ner, flest kommentarer för RemoveAddressFromCustomer 
internal static class CustomerMenu
{
    public static async Task ShowMenu(CustomerService customerService, AdressService adressService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välj ett val:");
            Console.WriteLine("1. Skapa kund");
            Console.WriteLine("2. Uppdatera kund");
            Console.WriteLine("3. Ta bort kund");
            Console.WriteLine("4. Visa kund");
            Console.WriteLine("5. Lägg till address till kund");
            Console.WriteLine("6. Uppdatera address till kund");
            Console.WriteLine("7. Ta bort address till kund");
            Console.WriteLine("8. Visa address till kund");
            Console.WriteLine("9. Gå tillbaka");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateCustomer(customerService);
                    break;
                case "2":
                    await UpdateCustomer(customerService);
                    break;
                case "3":
                    await DeleteCustomer(customerService);
                    break;
                case "4":
                    await GetCustomer(customerService);
                    break;
                case "5":
                    await AddAddressToCustomer(adressService, customerService);
                    break;
                case "6":
                    await UpdateAddressForCustomer(adressService, customerService);
                    break;
                case "7":
                    await RemoveAddressFromCustomer(adressService, customerService);
                    break;
                case "8":
                    await ShowCustomerAddresses(customerService, adressService);
                    break;
                case "9":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }
        }
    }

    private static async Task CreateCustomer(CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange kundens förnamn:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Ange kundens efternamn:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Ange kundens e-postadress:");
        string email = Console.ReadLine();

        var newCustomer = new CustomerEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };

        var createdCustomer = await customerService.CreateAsync(newCustomer);

        if (createdCustomer != null)
        {
            Console.WriteLine("Kund skapad och sparad!");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Kunden med den angivna e-postadressen finns redan i databasen.");
            Console.ReadKey();
        }
    }

    private static async Task UpdateCustomer(CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för kunden att uppdatera:");
        string email = Console.ReadLine();

        var existingCustomer = await customerService.GetByEmailAsync(email);

        if (existingCustomer != null)
        {
            Console.WriteLine("Ange nya uppgifter:");

            Console.WriteLine("Ny e-postadress:");
            string newEmail = Console.ReadLine();
            existingCustomer.Email = newEmail;

            Console.WriteLine("Nytt förnamn:");
            string newFirstName = Console.ReadLine();
            existingCustomer.FirstName = newFirstName;

            Console.WriteLine("Nytt efternamn:");
            string newLastName = Console.ReadLine();
            existingCustomer.LastName = newLastName;


            Console.WriteLine($"Kund (E-postadress: {email}) uppdaterad!");

            await customerService.UpdateAsync(email, existingCustomer);
            Console.ReadKey();
        }

    }

    private static async Task DeleteCustomer(CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för kunden att ta bort:");
        string email = Console.ReadLine();

        await customerService.DeleteAsync(email);
        Console.ReadKey();
    }

    private static async Task GetCustomer(CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för att hämta kundinformation:");
        string email = Console.ReadLine();

        await customerService.GetByEmailAsync(email);
        Console.ReadKey();
    }

    private static async Task AddAddressToCustomer(AdressService addressService, CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange kundens e-post för att koppla adressen:");
        string customerEmail = Console.ReadLine();

        var existingCustomer = await customerService.GetByEmailAsync(customerEmail);

        if (existingCustomer != null)
        {
            var addressEntity = new AdressEntity();

            Console.WriteLine("Ange gatuadress:");
            addressEntity.StreetName = Console.ReadLine();

            Console.WriteLine("Ange postnummer (utan mellanslag):");
            if (int.TryParse(Console.ReadLine(), out int postalCode))
            {
                addressEntity.PostalCode = postalCode;

                Console.WriteLine("Ange stad:");
                addressEntity.City = Console.ReadLine();
                

                await addressService.CreateAndAssociateWithCustomerAsync(existingCustomer.CustomerId, addressEntity);

            }
            else
            {
                Console.WriteLine("Ogiltigt postnummer. Skapa adress avbruten.");
                Console.ReadKey();
            }
        }

    }

    private static async Task RemoveAddressFromCustomer(AdressService addressService, CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för kunden vars adress ska tas bort:");
        string email = Console.ReadLine();

        // hämtar kund efter input från användare 
        var existingCustomer = await customerService.GetByEmailAsync(email);

        if (existingCustomer != null)
        {
            Console.WriteLine("Välj adress att ta bort:");
            // hämtar alla adresser för den befintliga kunden genom att anropa GetAddressesByCustomerIdAsync 
            var customerAddresses = await addressService.GetAddressesByCustomerIdAsync(existingCustomer.CustomerId);

            //  loopar igenom listan av adresser och skriver ut varje adress i nummerordning
            for (int i = 0; i < customerAddresses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Gatuadress: {customerAddresses[i].StreetName}, Postnummer: {customerAddresses[i].PostalCode}, Stad: {customerAddresses[i].City}");
            }

            // läser av input från användare för val av adress att ta bort, kontrollerar också att inputen är inom giltiga intervall sett till adresser
            if (int.TryParse(Console.ReadLine(), out int selectedAddressIndex) && selectedAddressIndex > 0 && selectedAddressIndex <= customerAddresses.Count)
            {
                var selectedAddress = customerAddresses[selectedAddressIndex - 1];

                await addressService.DeleteAsync(selectedAddress.AdressId);
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Ta bort adress avbruten.");
                Console.ReadKey();
            }
        }

    }

    // liknande metod som ovan fast uppdaterar adress istället för att ta bort
    private static async Task UpdateAddressForCustomer(AdressService addressService, CustomerService customerService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för kunden vars adress ska uppdateras:");
        string email = Console.ReadLine();

        var existingCustomer = await customerService.GetByEmailAsync(email);

        if (existingCustomer != null)
        {
            Console.WriteLine("Välj adress att uppdatera:");

            var customerAddresses = await addressService.GetAddressesByCustomerIdAsync(existingCustomer.CustomerId);

            for (int i = 0; i < customerAddresses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Gatuadress: {customerAddresses[i].StreetName}, Postnummer: {customerAddresses[i].PostalCode}, Stad: {customerAddresses[i].City}");
            }

            if (int.TryParse(Console.ReadLine(), out int selectedAddressIndex) && selectedAddressIndex > 0 && selectedAddressIndex <= customerAddresses.Count)
            {
                var selectedAddress = customerAddresses[selectedAddressIndex - 1];

                Console.WriteLine("Ange nya uppgifter:");

                Console.WriteLine("Ny gatuadress:");
                selectedAddress.StreetName = Console.ReadLine();

                Console.WriteLine("Nytt postnummer (utan mellanslag):");
                if (int.TryParse(Console.ReadLine(), out int newPostalCode))
                {
                    selectedAddress.PostalCode = newPostalCode;

                    Console.WriteLine("Ny stad:");
                    selectedAddress.City = Console.ReadLine();
                    

                    await addressService.UpdateAsync(selectedAddress);
                }
                else
                {
                    Console.WriteLine("Ogiltigt postnummer. Uppdatera adress avbruten.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Uppdatera adress avbruten.");
                Console.ReadKey();
            }
        }

    }

    // visar alla adresser för vald customer via email
    private static async Task ShowCustomerAddresses(CustomerService customerService, AdressService addressService)
    {
        Console.Clear();
        Console.WriteLine("Ange e-postadress för att visa adresser för kunden:");
        string email = Console.ReadLine();

        var existingCustomer = await customerService.GetByEmailAsync(email);

        if (existingCustomer != null)
        {
            var customerAddresses = await addressService.GetAddressesByCustomerIdAsync(existingCustomer.CustomerId);

            Console.WriteLine($"Adresser för kund (E-post: {email}):");

            foreach (var address in customerAddresses)
            {
                Console.WriteLine($"Gatuadress: {address.StreetName}, Postnummer: {address.PostalCode}, Stad: {address.City}");
            }

            Console.ReadKey();
        }

    }

}
