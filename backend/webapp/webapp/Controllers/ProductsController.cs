using Microsoft.AspNetCore.Mvc;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Model;
using webapp.dto;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductById(Guid productId)
        {
            var product = await _productService.GetProductDetailsAsync(productId);
            var productInventory = product.ProductInventories.FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDetailsDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Model = product.Model,
                DatePublished = product.DatePublished,
                Description = product.Description,
                DeliveryTime = product.DeliveryTime,
                Image = product.Image,
                Price = product.Price,
                ShippingPrice = product.ShippingPrice,
                Color = product.Color.Name,
                Condition = product.Condition != null ? (product.Condition.Name ? "New" : "Used") : "Unknown",
                Gender = product.GenderNavigation != null ? (product.GenderNavigation.Name ? "Female" : "Male") : "Unknown",
                Category = product.ProductCategoryNavigation != null ? (product.ProductCategoryNavigation.Category ? "Female Category" : "Male Category") : "Unknown",
                Brand = product.Brand.Name,
                QuantityInStock = productInventory?.QuantityInStock ?? 0,
                ItemsSold = productInventory?.ItemSold ?? 0,
                IsDeleted = product.IsDeleted,
            };

            return Ok(productDto);
        }

    }
}