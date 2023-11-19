using ConsolShopV2.Contexts;
using ConsolShopV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolShopV2.Services;

// CRUD för reviews
internal class ReviewService
{
    private readonly DataContext _context;

    public ReviewService(DataContext context)
    {
        _context = context;
    }

    public async Task<ReviewEntity> CreateAsync(ReviewEntity reviewEntity)
    {
        _context.Reviews.Add(reviewEntity);
        await _context.SaveChangesAsync();
        Console.WriteLine("Recension skapad och sparad!");
        return reviewEntity;
    }

    public async Task<ReviewEntity> UpdateAsync(int reviewId, int newRating, string newReviewText)
    {
        var existingReview = await _context.Reviews.FindAsync(reviewId);

        if (existingReview != null)
        {
            existingReview.Rating = newRating;
            existingReview.ReviewText = newReviewText;

            await _context.SaveChangesAsync();

            Console.WriteLine($"Recension (ID: {reviewId}) uppdaterad!");
            return existingReview;
        }

        Console.WriteLine($"Recension med ID {reviewId} hittades inte.");
        return null;
    }

    public async Task<bool> DeleteAsync(int reviewId)
    {
        var existingReview = await _context.Reviews.FindAsync(reviewId);

        if (existingReview != null)
        {
            _context.Reviews.Remove(existingReview);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Recension (ID: {reviewId}) borttagen!");
            return true;
        }

        Console.WriteLine($"Recension med ID {reviewId} hittades inte.");
        return false;
    }

    public async Task GetReviewsForProductAsync(string productName)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductName == productName);

        if (product != null)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == product.Id)
                .ToListAsync();

            if (reviews.Any())
            {
                Console.WriteLine($"Recensioner för produkten {productName}:");
                foreach (var review in reviews)
                {
                    Console.WriteLine($"Betyg: {review.Rating}, Kommentar: {review.ReviewText}");
                }
            }
            else
            {
                Console.WriteLine($"Inga recensioner hittades för produkten {productName}.");
            }
        }
        else
        {
            Console.WriteLine($"Produkt med namnet {productName} hittades inte.");
        }
    }
}
