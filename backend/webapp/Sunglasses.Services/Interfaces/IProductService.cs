
using SunglassesDAL.Enum;
using SunglassesDAL.Implementations;
using SunglassesDAL.Model;


namespace Sunglasses.Services.Interfaces
{
    public interface IProductService
    {

        Task<Product?> GetProductDetailsAsync(Guid productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product, ProductInventory productInventory);
        Task<Product?> SoftDeleteProductAsync(Guid productId);
        Task<Product> UpdateProductAsync(Product product);
        Task<IEnumerable<Product>> SearchProductsAsync(string? brandId, bool? category);
        Task<(List<Product>, int)> GetFilteredProductsAsync(
        int pageNumber,
        int pageSize,
        bool? category,
        decimal? minPrice,
        decimal? maxPrice,
        bool? condition,
        SortBy? sortBy,
        bool isAdmin,
        string brandName = null);

        Task<Product> ModifyProductAsync(Guid productId, Product updatedProduct, int newQuantityInStock, int newItemsSold);

        Task<List<PurchaseHistory>> GetUserPurchaseHistoryAsync(Guid userId);

    }
}