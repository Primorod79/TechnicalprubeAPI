using EcommerceAPI.Models;

namespace EcommerceAPI.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (context.Users.Any() || context.Categories.Any() || context.Products.Any())
                return;

            var admin = new User
            {
                Email = "admin@test.com",
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = Core.Enums.Role.Admin,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user = new User
            {
                Email = "user@test.com",
                Username = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                Role = Core.Enums.Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(admin, user);

            var cat1 = new Category { Name = "Electrónica", Description = "Electrónicos y gadgets", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat2 = new Category { Name = "Ropa", Description = "Prendas de vestir", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat3 = new Category { Name = "Alimentos", Description = "Comestibles", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Categories.AddRange(cat1, cat2, cat3);

            var products = new List<Product>
            {
                new Product { Name = "Smartphone X", Description = "Teléfono inteligente de ejemplo", Price = 499.99m, Stock = 50, Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Laptop Pro", Description = "Portátil potente", Price = 1299.99m, Stock = 20, Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Camisa Casual", Description = "Camisa de algodón", Price = 29.99m, Stock = 100, Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Pantalón Jeans", Description = "Jeans azules", Price = 49.99m, Stock = 80, Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Cereal Mix", Description = "Caja de cereal", Price = 4.99m, Stock = 200, Category = cat3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}