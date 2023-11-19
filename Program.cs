using ConsolShopV2.Contexts;
using ConsolShopV2.Services;

 class Program
{
    static async Task Main(string[] args)
    {
        // skapar en instans av DataContext för att hantera databasanslutning
        using (var context = new DataContext())
        {
            // instanser av olika tjänster som behövs för olika funktionaliteter
            var productService = new ProductService(context);
            var customerService = new CustomerService(context);
            var adressService = new AdressService(context);
            var productCategoryService = new ProductCategoryService(context);
            var reviewService = new ReviewService(context);

            Console.WriteLine("Välkommen till adminsidan för ConsolShopV2!");

            while (true)
            {
                Console.WriteLine("Välj ett val:");
                Console.WriteLine("1. Produktmeny");
                Console.WriteLine("2. Kundmeny");
                Console.WriteLine("3. Avsluta");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ProductMenu.ShowMenu(productService, productCategoryService, reviewService);
                        break;
                    case "2":
                        await CustomerMenu.ShowMenu(customerService, adressService);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }
    }
}