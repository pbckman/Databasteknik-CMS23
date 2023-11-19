using ConsolShopV2.Models.Entities;

namespace ConsolShopV2.Services;

internal static class ProductMenu
{
    // metod för att visa produktmenyn, använder sig av tre olika services för att få ihop alla funktioner till menyalternativen
    public static async Task ShowMenu(ProductService productService, ProductCategoryService productCategoryService, ReviewService reviewService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välj ett val:");
            Console.WriteLine("1. Skapa produkt");
            Console.WriteLine("2. Uppdatera produkt");
            Console.WriteLine("3. Ta bort produkt");
            Console.WriteLine("4. Visa produkt");
            Console.WriteLine("5. Lägg till kategori till produkt");
            Console.WriteLine("6. Uppdatera kategori till produkt");
            Console.WriteLine("7. Ta bort kategori till produkt");
            Console.WriteLine("8. Visa alla befintliga kategorier");
            Console.WriteLine("9. Lägg till recension till produkt");
            Console.WriteLine("10. Uppdatera recension till produkt");
            Console.WriteLine("11. Ta bort recension till produkt");
            Console.WriteLine("12. Visa alla recensioner till produkt");
            Console.WriteLine("13. Gå tillbaka");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateProduct(productService);
                    break;
                case "2":
                    await UpdateProduct(productService);
                    break;
                case "3":
                    await DeleteProduct(productService);
                    break;
                case "4":
                    await GetProduct(productService);
                    break;
                case "5":
                    await AddCategoryToProduct(productService, productCategoryService);
                    break;
                case "6":
                    await UpdateCategoryForProduct(productService, productCategoryService);
                    break;
                case "7":
                    await RemoveCategoryFromProduct(productService);
                    break;
                case "8":
                    await ShowAllProductCategories(productCategoryService);
                    break;
                case "9":
                    await AddReviewToProduct(productService, reviewService);
                    break;
                case "10":
                    await UpdateReviewForProduct(reviewService);
                    break;
                case "11":
                    await RemoveReviewForProduct(reviewService);
                    break;
                case "12":
                    await GetReviewsForProduct(reviewService);
                    break;
                case "13":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }
        }
    }

    private static async Task CreateProduct(ProductService productService)
    {
        Console.Clear();    
        Console.WriteLine("Ange produktnamn:");
        string productName = Console.ReadLine();

        Console.WriteLine("Ange produktbeskrivning:");
        string productDescription = Console.ReadLine();

        //  tar emot input från användaren och jämför det mot befintliga kategoriIds, finns IDt så skapas produkten annars avbryts skapandet.
        // samma sätt används flera gånger här nedan
        Console.WriteLine("Ange produktkategori ID:");
        if (int.TryParse(Console.ReadLine(), out int productCategoryId))
        {
            var newProduct = new ProductEntity
            {
                ProductName = productName,
                ProductDescription = productDescription,
                ProductCategoryId = productCategoryId
            };

            var createdProduct = await productService.CreateAsync(newProduct);

        }
        else
        {
            Console.WriteLine("Ogiltigt produktkategori ID. Skapa produkt avbruten.");
            Console.ReadKey();
        }
    }

    private static async Task UpdateProduct(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten att uppdatera:");
        string productName = Console.ReadLine();

        var existingProduct = await productService.GetByNameAsync(productName);

        if (existingProduct != null)
        {
            Console.WriteLine("Ange nya uppgifter:");

            Console.WriteLine("Ny produktbeskrivning:");
            string newProductDescription = Console.ReadLine();

            Console.WriteLine("Ny produktkategori ID:");
            if (int.TryParse(Console.ReadLine(), out int newProductCategoryId))
            {
                existingProduct.ProductDescription = newProductDescription;
                existingProduct.ProductCategoryId = newProductCategoryId;

                await productService.UpdateAsync(productName, existingProduct);
            }
            else
            {
                Console.WriteLine("Ogiltigt nytt produktkategori ID. Uppdatering avbruten.");
            }
        }
    }

    private static async Task DeleteProduct(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten att ta bort:");
        string productName = Console.ReadLine();

        await productService.DeleteAsync(productName);
    }

    private static async Task GetProduct(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för att hämta produktinformation:");
        string productName = Console.ReadLine();

        await productService.GetByNameAsync(productName);
        Console.ReadKey();
    }

    // metod för att lägga till kategori till produkt, visar först alla kategorier för att sedan ta emot input från användaren om vilken kategori man vill lägga till
    private static async Task AddCategoryToProduct(ProductService productService, ProductCategoryService categoryService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten att lägga till kategori:");
        string productName = Console.ReadLine();

        var existingProduct = await productService.GetByNameAsync(productName);

        if (existingProduct != null)
        {
            Console.WriteLine("Tillgängliga produktkategorier:");

            var categories = await categoryService.GetAllAsync();

            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.CategoryId}, Namn: {category.CategoryName}");
            }

            Console.WriteLine("Ange namn för produktkategorin att lägga till:");
            string categoryNameToAdd = Console.ReadLine();

            // hämta den befintliga kategorin från databasen baserat på namnet
            var selectedCategory = await categoryService.GetByNameAsync(categoryNameToAdd);

            if (selectedCategory != null)
            {
                // associera produkt och kategori
                existingProduct.ProductCategoryId = selectedCategory.CategoryId;

                // uppdatera produkten i databasen
                await productService.UpdateAsync(productName, existingProduct);
            }
            else
            {
                Console.WriteLine($"Kategori med namnet {categoryNameToAdd} hittades inte.");
            }
        }
        else
        {
            Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        }

        Console.ReadKey();
    }


    // samma metod som ovan fast updaterar kategori istället för att lägga till, liknande metoder här nedan med ta bort och visa för kategorier och reviews
    private static async Task UpdateCategoryForProduct(ProductService productService, ProductCategoryService categoryService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten att uppdatera kategori för:");
        string productName = Console.ReadLine();

        var existingProduct = await productService.GetByNameAsync(productName);

        if (existingProduct != null)
        {
            Console.WriteLine("Tillgängliga produktkategorier:");

            var categories = await categoryService.GetAllAsync();

            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.CategoryId}, Namn: {category.CategoryName}");
            }

            Console.WriteLine("Ange namn för produktkategorin att uppdatera till:");
            string newCategoryName = Console.ReadLine();

            var selectedCategory = await categoryService.GetByNameAsync(newCategoryName);

            if (selectedCategory != null)
            {              
                existingProduct.ProductCategoryId = selectedCategory.CategoryId;
                await productService.UpdateAsync(productName, existingProduct);

                Console.WriteLine($"Produktens kategori har uppdaterats till {selectedCategory.CategoryName}.");
            }
            else
            {
                Console.WriteLine($"Kategori med namnet {newCategoryName} hittades inte.");
            }
        }
        else
        {
            Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        }

        Console.ReadKey();
    }



    private static async Task RemoveCategoryFromProduct(ProductService productService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten att ta bort kategori från:");
        string productName = Console.ReadLine();

        var existingProduct = await productService.GetByNameAsync(productName);

        if (existingProduct != null)
        {
            existingProduct.ProductCategoryId = null;

            await productService.UpdateAsync(productName, existingProduct);

            Console.WriteLine($"Produkten {productName} har tagits bort från kategorin.");
        }
        else
        {
            Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        }

        Console.ReadKey();
    }

    private static async Task ShowAllProductCategories(ProductCategoryService categoryService)
    {
        Console.Clear();
        Console.WriteLine("Alla tillgängliga produktkategorier:");

        var categories = await categoryService.GetAllAsync();

        foreach (var category in categories)
        {
            Console.WriteLine($"ID: {category.CategoryId}, Namn: {category.CategoryName}");
        }

        Console.ReadKey();
    }


    private static async Task AddReviewToProduct(ProductService productService, ReviewService reviewService)
     {
         Console.Clear();
         Console.WriteLine("Ange produktnamn för produkten att lägga till recension:");
         string productName = Console.ReadLine();

         var existingProduct = await productService.GetByNameAsync(productName);

         if (existingProduct != null)
         {
             Console.WriteLine("Ange betyg (1-5) för recensionen:");
             if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5)
             {
                 Console.WriteLine("Skriv recensionen:");
                 string reviewText = Console.ReadLine();

                 var newReview = new ReviewEntity
                 {
                     ProductId = existingProduct.Id,
                     Rating = rating,
                     ReviewText = reviewText
                 };

                 await reviewService.CreateAsync(newReview);
             }
             else
             {
                 Console.WriteLine("Ogiltigt betyg.");
             }
         }
         else
         {
             Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
         }

         Console.ReadKey();
     }

    private static async Task UpdateReviewForProduct(ReviewService reviewService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten vars recension ska uppdateras:");
        string productName = Console.ReadLine();

        Console.WriteLine("Ange recensions-ID för recensionen att uppdatera:");
        if (int.TryParse(Console.ReadLine(), out int reviewId))
        {
            Console.WriteLine("Ange nya betyg (1-5) för recensionen:");
            if (int.TryParse(Console.ReadLine(), out int newRating) && newRating >= 1 && newRating <= 5)
            {
                Console.WriteLine("Skriv den uppdaterade recensionen:");
                string newReviewText = Console.ReadLine();

                await reviewService.UpdateAsync(reviewId, newRating, newReviewText);
            }
            else
            {
                Console.WriteLine("Ogiltigt betyg. Uppdatering avbruten.");
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt recensions-ID. Uppdatering avbruten.");
        }

        Console.ReadKey();
    }

    private static async Task RemoveReviewForProduct(ReviewService reviewService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för produkten vars recension ska tas bort:");
        string productName = Console.ReadLine();

        Console.WriteLine("Ange recensions-ID för recensionen att ta bort:");
        if (int.TryParse(Console.ReadLine(), out int reviewId))
        {
            await reviewService.DeleteAsync(reviewId);
        }
        else
        {
            Console.WriteLine("Ogiltigt recensions-ID. Borttagning avbruten.");
        }

        Console.ReadKey();
    }

    private static async Task GetReviewsForProduct(ReviewService reviewService)
    {
        Console.Clear();
        Console.WriteLine("Ange produktnamn för att hämta recensioner:");
        string productName = Console.ReadLine();

        await reviewService.GetReviewsForProductAsync(productName);

        Console.ReadKey();
    }
}
