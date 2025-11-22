using EcommerceAPI.Models;

namespace EcommerceAPI.Core.Interfaces
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task<IEnumerable<Image>> GetByProductIdAsync(int productId);
        Task<Image> AddAsync(Image image);
        Task<Image> UpdateAsync(Image image);
        Task<bool> DeleteAsync(int id);
    }
}
