using SunglassesDAL.Model;
using webapp.dto;

namespace webapp.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProduct(this AddProductDto productDto)
        {
            return new Product
            {
                ProductId = Guid.NewGuid(),
                Name = productDto.Name,
                Model = productDto.Model,
                Description = productDto.Description,
                DeliveryTime = productDto.DeliveryTime,
                Image = productDto.Image,
                Price = productDto.Price,
                ShippingPrice = productDto.ShippingPrice,
                ColorId = productDto.ColorId,
                ConditionId = productDto.ConditionId,
                Gender = productDto.Gender,
                ProductCategory = productDto.ProductCategory,
                BrandId = productDto.BrandId,
                DatePublished = DateOnly.FromDateTime(DateTime.UtcNow),
                IsCurrent = true,
                IsDeleted = false
            };
        }
        public static ProductInventory ToProductInventory(this AddProductDto productDto, Guid productId)
        {
            return new ProductInventory
            {
                ProductId = productId,
                QuantityInStock = productDto.QuantityInStock,
                ItemSold = productDto.ItemsSold
            };
        }

        public static object ToProductDto(Product product)
        {
            return new
            {
                product.ProductId,
                product.Name,
                product.Description,
                product.Price,
                Brand = product.Brand != null ? product.Brand.Name : "Unknown",
                Category = product.ProductCategory == 1 ? "Women's Sunglasses" : "Men's Sunglasses",
                product.Image
            };
        }

        public static ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Image = product.Image,
                Price = product.Price,
                Condition = product.Condition != null ? (product.Condition.Name ? "New" : "Used") : "Unknown",
                Category = product.ProductCategoryNavigation != null
            ? (product.ProductCategoryNavigation.Category ? "Women's Sunglasses" : "Men's Sunglasses")
            : "Unknown",
                brandName = product.Brand != null
            ? product.Brand.Name
            : "Unknown",

            };
        }
        public static List<ProductDTO> ToProductDTOs(IEnumerable<Product> products)
        {
            return products.Select(ToProductDTO).ToList();
        }
    }
}