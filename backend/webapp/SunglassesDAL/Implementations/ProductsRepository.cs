
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SunglassesDAL.Enum;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace SunglassesDAL.Implementations
{
    public class ProductRepository : IProducts
    {
        private readonly WebshopContext _context;

        public ProductRepository(WebshopContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProductAsync(Product product, ProductInventory productInventory)
        {

            await _context.Products.AddAsync(product);


            await _context.ProductInventories.AddAsync(productInventory);


            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products

                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsForShopAsync()
        {
            return await _context.Products
                .ToListAsync();
        }


        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Color)
                .Include(p => p.Condition)
                .Include(p => p.GenderNavigation)
                .Include(p => p.ProductCategoryNavigation)
                .Include(p => p.Brand)
                .Include(p => p.ProductInventories)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<(List<Product>, int)> GetFilteredPagedProductsAsync(
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
            var productsQuery = _context.Products
                .Include(p => p.Condition)
                .Include(p => p.ProductCategoryNavigation)
                .Include(p => p.Brand)
                .Where(p =>
                    (isAdmin && p.IsCurrent) ||
                    (!isAdmin && !p.IsDeleted && p.IsCurrent)
                );

            if (category.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductCategoryNavigation.Category == category.Value);
            }
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }
            if (condition.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Condition.Name == condition.Value);
            }
            if (!string.IsNullOrEmpty(brandName))
            {
                productsQuery = productsQuery.Where(p => EF.Functions.Like(p.Brand.Name, $"%{brandName}%"));
            }
            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case SortBy.AlphabetAsc:
                        productsQuery = productsQuery.OrderBy(p => p.Brand.Name);
                        break;
                    case SortBy.AlphabetDesc:
                        productsQuery = productsQuery.OrderByDescending(p => p.Brand.Name);
                        break;
                    case SortBy.PriceLowToHigh:
                        productsQuery = productsQuery.OrderBy(p => p.Price);
                        break;
                    case SortBy.PriceHighToLow:
                        productsQuery = productsQuery.OrderByDescending(p => p.Price);
                        break;
                    case SortBy.DeliveryTimeAsc:
                        productsQuery = productsQuery.OrderBy(p => p.DeliveryTime);
                        break;
                    case SortBy.DeliveryTimeDesc:
                        productsQuery = productsQuery.OrderByDescending(p => p.DeliveryTime);
                        break;
                    default:
                        break;
                }
            }

            var totalProducts = await productsQuery.CountAsync();
            var products = await productsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalProducts);
        }


        public async Task<Product?> SoftDeleteProductAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product != null)
            {
                product.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductCategoryNavigation)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string? brandName, bool? category)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductCategoryNavigation)
                .Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(brandName))
            {
                query = query.Where(p => EF.Functions.Like(p.Brand.Name, $"%{brandName}%"));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.ProductCategory == (category.Value ? 1 : 2));
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetProductAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Color)
                .FirstOrDefaultAsync(p => p.ProductId == productId && !p.IsDeleted);
        }
        public async Task<Product?> GetProductForVersionAsync(Guid productId)
        {
            return await _context.Products
                .Where(p => p.ProductId == productId)
        .Include(p => p.Color)
        .Include(p => p.Condition)
        .Include(p => p.GenderNavigation)
        .Include(p => p.ProductCategoryNavigation)
        .Include(p => p.Brand)
        .FirstOrDefaultAsync();
        }
        public async Task<Product> CreateNewProductVersionAsync(Product updatedProduct, int newQuantityInStock, int newItemsSold)
        {

            var oldProduct = await _context.Products
        .Where(p => p.ProductId == updatedProduct.ProductId && p.IsCurrent == true)
        .FirstOrDefaultAsync();

            if (oldProduct == null)
            {
                throw new Exception("The product has been modified by another admin. Please refresh the page to get the latest version.");
            }
            if (oldProduct != null)
            {
                var query = "UPDATE Products SET IsCurrent = 0, isDeleted = 1 WHERE productId = @productId AND IsCurrent = 1";
                await _context.Database.ExecuteSqlRawAsync(query, new SqlParameter("@productId", updatedProduct.ProductId));
            }

            updatedProduct.IsCurrent = true;
            updatedProduct.VersionId = GetNextVersionId(updatedProduct.ProductId);
            updatedProduct.ProductId = Guid.NewGuid();
            updatedProduct.IsDeleted = false;
            _context.Products.Add(updatedProduct);
            var newProductInventory = new ProductInventory
            {
                ProductId = updatedProduct.ProductId,
                QuantityInStock = newQuantityInStock,
                ItemSold = newItemsSold
            };
            _context.ProductInventories.Add(newProductInventory);
            await _context.SaveChangesAsync();

            return updatedProduct;
        }

        public int GetNextVersionId(Guid productId)
        {
            var highestVersion = _context.Products
                .Where(p => p.ProductId == productId)
                .Max(p => p.VersionId);

            return highestVersion + 1;
        }
        public async Task<List<PurchaseHistory>> GetPurchaseHistoryAsync(Guid userId)
        {

            var purchaseHistory = await _context.Orders
                .Where(order => order.UserId == userId)
                .Include(order => order.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(product => product.Brand)
                .Select(order => new PurchaseHistory
                {
                    OrderDate = order.Date.ToString("yyyy-MM-dd"),
                    OrderTime = order.Date.ToString("HH:mm:ss"),
                    OrderDetails = order.OrderDetails.Select(od => new PurchaseHistoryDetail
                    {
                        Subtotal = od.Subtotal,
                        BrandName = od.Product.Brand.Name
                    }).ToList()
                })
                .ToListAsync();

            return purchaseHistory;
        }
    }


    public class PurchaseHistory
    {
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
        public List<PurchaseHistoryDetail> OrderDetails { get; set; }
    }

    public class PurchaseHistoryDetail
    {
        public string BrandName { get; set; }
        public decimal Subtotal { get; set; }
    }
}
