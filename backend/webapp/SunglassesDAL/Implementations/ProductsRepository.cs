using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
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

    }
}
