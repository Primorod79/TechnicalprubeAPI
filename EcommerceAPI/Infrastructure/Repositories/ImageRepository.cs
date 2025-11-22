using EcommerceAPI.Core.Interfaces;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            return await _context.Images
                .Include(i => i.Product)
                .ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Image>> GetByProductIdAsync(int productId)
        {
            return await _context.Images
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Image> AddAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<Image> UpdateAsync(Image image)
        {
            _context.Images.Update(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return false;

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
