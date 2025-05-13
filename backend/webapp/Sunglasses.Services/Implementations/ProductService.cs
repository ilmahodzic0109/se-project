using Sunglasses.Services.Interfaces;
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
    }
}