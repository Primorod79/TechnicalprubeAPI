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
            var cat4 = new Category { Name = "Hogar", Description = "Artículos para el hogar", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat5 = new Category { Name = "Deportes", Description = "Artículos deportivos", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat6 = new Category { Name = "Libros", Description = "Libros y revistas", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat7 = new Category { Name = "Juguetes", Description = "Juguetes para niños", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat8 = new Category { Name = "Belleza", Description = "Productos de belleza", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Categories.AddRange(cat1, cat2, cat3, cat4, cat5, cat6, cat7, cat8);

            var products = new List<Product>
            {
                new Product { Name = "Smartphone X", Description = "Teléfono inteligente de ejemplo", Price = 499.99m, Stock = 50, Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Laptop Pro", Description = "Portátil potente", Price = 1299.99m, Stock = 20, Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Camisa Casual", Description = "Camisa de algodón", Price = 29.99m, Stock = 100, Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Pantalón Jeans", Description = "Jeans azules", Price = 49.99m, Stock = 80, Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Cereal Mix", Description = "Caja de cereal", Price = 4.99m, Stock = 200, Category = cat3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Aspiradora Smart", Description = "Aspiradora inteligente", Price = 249.99m, Stock = 15, Category = cat4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Balón de Fútbol", Description = "Balón profesional", Price = 34.99m, Stock = 60, Category = cat5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "El Principito", Description = "Libro clásico", Price = 12.99m, Stock = 150, Category = cat6, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "LEGO City", Description = "Set de construcción", Price = 79.99m, Stock = 40, Category = cat7, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Perfume Elegance", Description = "Fragancia premium", Price = 89.99m, Stock = 35, Category = cat8, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}