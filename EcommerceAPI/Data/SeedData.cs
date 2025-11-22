using EcommerceAPI.Models;

namespace EcommerceAPI.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            // Update existing products with image URLs if they don't have them
            var productsToUpdate = context.Products.Where(p => string.IsNullOrEmpty(p.ImageUrl)).ToList();
            if (productsToUpdate.Any())
            {
                var imageUrls = new Dictionary<string, string>
                {
                    ["Smartphone X"] = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=400",
                    ["Laptop Pro"] = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400",
                    ["Camisa Casual"] = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400",
                    ["Pantalon Jeans"] = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=400",
                    ["Cereal Mix"] = "https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=400",
                    ["Aspiradora Smart"] = "https://images.unsplash.com/photo-1558317374-067fb5f30001?w=400",
                    ["Balon de Futbol"] = "https://images.unsplash.com/photo-1614632537423-1e6c2e7e0aab?w=400",
                    ["El Principito"] = "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400",
                    ["LEGO City"] = "https://images.unsplash.com/photo-1587654780291-39c9404d746b?w=400",
                    ["Perfume Elegance"] = "https://images.unsplash.com/photo-1541643600914-78b084683601?w=400"
                };

                foreach (var product in productsToUpdate)
                {
                    if (imageUrls.TryGetValue(product.Name, out var imageUrl))
                    {
                        product.ImageUrl = imageUrl;
                        product.UpdatedAt = DateTime.UtcNow;
                    }
                }
                await context.SaveChangesAsync();
            }

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

            var cat1 = new Category { Name = "Electronica", Description = "Electronicos y gadgets", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat2 = new Category { Name = "Ropa", Description = "Prendas de vestir", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat3 = new Category { Name = "Alimentos", Description = "Comestibles", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat4 = new Category { Name = "Hogar", Description = "Articulos para el hogar", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat5 = new Category { Name = "Deportes", Description = "Articulos deportivos", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat6 = new Category { Name = "Libros", Description = "Libros y revistas", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat7 = new Category { Name = "Juguetes", Description = "Juguetes para ninos", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var cat8 = new Category { Name = "Belleza", Description = "Productos de belleza", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Categories.AddRange(cat1, cat2, cat3, cat4, cat5, cat6, cat7, cat8);

            var products = new List<Product>
            {
                new Product { Name = "Smartphone X", Description = "Telefono inteligente de ejemplo", Price = 499.99m, Stock = 50, ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=400", Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Laptop Pro", Description = "Portatil potente", Price = 1299.99m, Stock = 20, ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400", Category = cat1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Camisa Casual", Description = "Camisa de algodon", Price = 29.99m, Stock = 100, ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=400", Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Pantalon Jeans", Description = "Jeans azules", Price = 49.99m, Stock = 80, ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=400", Category = cat2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Cereal Mix", Description = "Caja de cereal", Price = 4.99m, Stock = 200, ImageUrl = "https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=400", Category = cat3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Aspiradora Smart", Description = "Aspiradora inteligente", Price = 249.99m, Stock = 15, ImageUrl = "https://images.unsplash.com/photo-1558317374-067fb5f30001?w=400", Category = cat4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Balon de Futbol", Description = "Balon profesional", Price = 34.99m, Stock = 60, ImageUrl = "https://images.unsplash.com/photo-1614632537423-1e6c2e7e0aab?w=400", Category = cat5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "El Principito", Description = "Libro clasico", Price = 12.99m, Stock = 150, ImageUrl = "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=400", Category = cat6, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "LEGO City", Description = "Set de construccion", Price = 79.99m, Stock = 40, ImageUrl = "https://images.unsplash.com/photo-1587654780291-39c9404d746b?w=400", Category = cat7, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Perfume Elegance", Description = "Fragancia premium", Price = 89.99m, Stock = 35, ImageUrl = "https://images.unsplash.com/photo-1541643600914-78b084683601?w=400", Category = cat8, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}