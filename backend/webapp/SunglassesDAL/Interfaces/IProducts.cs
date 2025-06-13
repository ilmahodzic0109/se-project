
using SunglassesDAL.Enum;
using SunglassesDAL.Implementations;
using SunglassesDAL.Model;

namespace SunglassesDAL.Interfaces
{
    public interface IProducts
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetAllProductsForShopAsync();
        Task<(List<Product>, int)> GetFilteredPagedProductsAsync(
     int pageNumber,
     int pageSize,
     bool? category,
     decimal? minPrice,
     decimal? maxPrice,
     bool? condition,
     SortBy? sortBy,
     bool isAdmin,
     string brandName = null);
        Task<Product> AddProductAsync(Product product, ProductInventory productInventory);
        Task<Product?> SoftDeleteProductAsync(Guid productId);
        Task<Product> UpdateProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string? brandId, bool? category);
        Task<Product?> GetProductAsync(Guid productId);
        Task<Product> CreateNewProductVersionAsync(Product updatedProduct, int newQuantityInStock, int newItemsSold);
        int GetNextVersionId(Guid productId);
        Task<List<PurchaseHistory>> GetPurchaseHistoryAsync(Guid userId);


    }
}