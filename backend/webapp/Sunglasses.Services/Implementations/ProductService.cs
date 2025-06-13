using Sunglasses.Services.Interfaces;
using SunglassesDAL.Enum;
using SunglassesDAL.Implementations;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;


namespace Sunglasses.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProducts _productRepository;


        public ProductService(IProducts productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> AddProductAsync(Product product, ProductInventory productInventory)
        {
            return await _productRepository.AddProductAsync(product, productInventory);
        }

        public async Task<(List<Product>, int)> GetFilteredProductsAsync(
        int pageNumber,
        int pageSize,
        bool? category,
        decimal? minPrice,
        decimal? maxPrice,
        bool? condition,
        SortBy? sortBy,
        bool isAdmin,
        string brandName = null)
        {
            var (products, totalProducts) = await _productRepository.GetFilteredPagedProductsAsync(
        pageNumber,
        pageSize,
        category,
        minPrice,
        maxPrice,
        condition,
        sortBy,
        isAdmin,
        brandName);

            return (products, totalProducts);
        }

        public async Task<Product?> GetProductDetailsAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {

            var products = await _productRepository.GetAllProductsAsync();
            return products;
        }

        public async Task<Product?> SoftDeleteProductAsync(Guid productId)
        {
            return await _productRepository.SoftDeleteProductAsync(productId);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string? brandId, bool? category)
        {
            if (string.IsNullOrEmpty(brandId) && !category.HasValue)
            {
                return await _productRepository.GetProductsAsync();
            }

            return await _productRepository.SearchProductsAsync(brandId, category);
        }
        public async Task<Product> ModifyProductAsync(Guid productId, Product updatedProduct, int newQuantityInStock, int newItemsSold)
        {
            updatedProduct.ProductId = productId;
            var updatedProductVersion = await _productRepository.CreateNewProductVersionAsync(updatedProduct, newQuantityInStock, newItemsSold);
            return updatedProductVersion;
        }
        public async Task<List<PurchaseHistory>> GetUserPurchaseHistoryAsync(Guid userId)
        {

            return await _productRepository.GetPurchaseHistoryAsync(userId);
        }
    }
}