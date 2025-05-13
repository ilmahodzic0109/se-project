using SunglassesDAL.Model;


namespace Sunglasses.Services.Interfaces
{
    public interface IProductService
    {

        Task<Product?> GetProductDetailsAsync(Guid productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();

    }
}