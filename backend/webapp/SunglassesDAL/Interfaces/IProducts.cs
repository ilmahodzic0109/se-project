
using SunglassesDAL.Model;

namespace SunglassesDAL.Interfaces
{
    public interface IProducts
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid productId);


    }
}