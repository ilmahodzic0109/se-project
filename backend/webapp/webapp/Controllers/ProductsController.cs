using Microsoft.AspNetCore.Mvc;
using SunglassesDAL.Interfaces;
using Sunglasses.Services.Implementations;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Model;
using webapp.dto;
using webapp.Mappers;
using SunglassesDAL.Enum;

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

        /* [HttpGet("categories")]
         public async Task<IActionResult> GetAllProductsForShop()
         {
             var products = await _productRepository.GetAllProductsForShopAsync(); 
             return Ok(products);  
         }*/

        [HttpGet("shop")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsForShop(
        int pageNumber = 1,
        int pageSize = 9,
        bool? category = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? condition = null,
        SortBy? sortBy = null,
        string brandName = null)
        {
            var isAdmin = Request.Headers["isAdmin"].FirstOrDefault() == "true";
            var (products, totalProducts) = await _productService.GetFilteredProductsAsync(
                pageNumber,
                pageSize,
                category,
                minPrice,
                maxPrice,
                condition,
                sortBy,
                isAdmin,
                brandName);
            if (products == null || !products.Any())
            {
                return NotFound("No products available with the selected filters.");
            }
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var productDtos = ProductMapper.ToProductDTOs(products);

            return Ok(new { products = productDtos, totalPages });
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

        [HttpPost("product")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] AddProductDto productDto)
        {
            var product = productDto.ToProduct();
            var productInventory = productDto.ToProductInventory(product.ProductId);
            var addedProduct = await _productService.AddProductAsync(product, productInventory);
            return CreatedAtAction(nameof(GetProductById), new { productId = addedProduct.ProductId }, addedProduct);
        }


        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> SoftDeleteProduct(Guid productId)
        {
            var product = await _productService.SoftDeleteProductAsync(productId);

            if (product == null)
            {
                return NotFound();
            }
            product.IsDeleted = true;
            await _productService.UpdateProductAsync(product);


            return Ok(new { message = "Product soft deleted successfully." });
        }

        [HttpPut("restore/{productId}")]
        public async Task<IActionResult> RestoreProduct(Guid productId)
        {
            var product = await _productService.GetProductDetailsAsync(productId);

            if (product == null)
            {
                return NotFound();
            }
            product.IsDeleted = false;
            await _productService.UpdateProductAsync(product);
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchProducts([FromQuery] string? brandName, [FromQuery] bool? category)
        {
            var products = await _productService.SearchProductsAsync(brandName, category);

            if (products == null || !products.Any())
            {
                return NotFound("No products found matching your criteria.");
            }
            var productDtos = products
                .Where(p => !p.IsDeleted)
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Image,
                    p.Price,
                    p.Description,
                    BrandName = p.Brand != null ? p.Brand.Name : "Unknown",
                    Category = p.ProductCategoryNavigation != null
            ? (p.ProductCategoryNavigation.Category ? "Women's Sunglasses" : "Men's Sunglasses")
            : "Unknown"
                }).ToList();

            return Ok(productDtos);
        }
        [HttpPut("modify/{productId}")]
        public async Task<IActionResult> ModifyProduct(Guid productId, [FromBody] UpdateProductDto updatedProductDto)
        {
            try
            {

                var product = await _productService.GetProductDetailsAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }


                product.Name = updatedProductDto.Name;
                product.Model = updatedProductDto.Model;
                product.DatePublished = updatedProductDto.DatePublished;
                product.Description = updatedProductDto.Description;
                product.DeliveryTime = updatedProductDto.DeliveryTime;
                product.Image = updatedProductDto.Image;
                product.Price = updatedProductDto.Price;
                product.ShippingPrice = updatedProductDto.ShippingPrice;
                product.ColorId = updatedProductDto.ColorId;
                product.ConditionId = updatedProductDto.ConditionId;
                product.Gender = updatedProductDto.Gender;
                product.ProductCategory = updatedProductDto.ProductCategory;
                product.BrandId = updatedProductDto.BrandId;


                int newQuantityInStock = updatedProductDto.QuantityInStock;
                int newItemsSold = updatedProductDto.ItemsSold;


                var updatedProductVersion = await _productService.ModifyProductAsync(productId, product, newQuantityInStock, newItemsSold);

                return Ok(new
                {
                    message = "Product modified successfully",
                    product = new
                    {
                        updatedProductVersion.ProductId,
                        updatedProductVersion.Name,
                        updatedProductVersion.Model,
                        updatedProductVersion.Price,
                        updatedProductVersion.Image,
                        updatedProductVersion.Description,
                        updatedProductVersion.ColorId,
                        updatedProductVersion.ConditionId,
                        updatedProductVersion.Gender,
                        updatedProductVersion.ProductCategory,
                        updatedProductVersion.BrandId,
                        updatedProductVersion.IsCurrent
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("purchase-history/{userId}")]
        public async Task<IActionResult> GetUserPurchaseHistory(Guid userId)
        {
            var purchaseHistory = await _productService.GetUserPurchaseHistoryAsync(userId);

            if (purchaseHistory == null || purchaseHistory.Count == 0)
            {
                return NotFound("No purchase history found.");
            }

            return Ok(purchaseHistory);
        }
    }
}