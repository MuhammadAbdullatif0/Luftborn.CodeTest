using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeed
{
    public static async Task SeedAsync(StoreDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var categories = new List<Category>
        {
            new() {  Name = "Electronics" },
            new() {  Name = "Clothing" },
            new() {  Name = "Books" }
        };
        context.Categories.AddRange(categories);

        var products = new List<Product>
        {
            new() { Name = "Laptop", Description = "High performance laptop", Price = 999.99m, PictureUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=800&h=600&fit=crop", ProductCategoryId = 1 },
            new() { Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, PictureUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800&h=600&fit=crop", ProductCategoryId = 2 },
            new() { Name = "Clean Code", Description = "Book by Robert Martin", Price = 39.99m, PictureUrl = "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=800&h=600&fit=crop", ProductCategoryId = 3 }
        };
        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
