using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace SunglassesDAL.Implementations
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly WebshopContext _context;

        public CheckoutRepository(WebshopContext context)
        {
            _context = context;
        }
        public async Task<bool> SaveOrderAsync(Order order)
        {
            
            await _context.Orders.AddAsync(order);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> SaveOrderDetailAsync(OrderDetail orderDetail)
        {
            
            await _context.OrderDetails.AddAsync(orderDetail);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<Cart>> GetSelectedCartItemsAsync(Guid userId)
        {
            
            return await _context.Carts
               .Where(c => c.UserId == userId && c.IsSelected)  
               .Include(c => c.Product)
               .ThenInclude(p => p.Brand)  
               .ToListAsync();
        }
        public async Task<ProductInventory?> GetProductInventoryAsync(Guid productId)
        {
            return await _context.ProductInventories
                .FirstOrDefaultAsync(pi => pi.ProductId == productId);
        }

        public async Task<bool> UpdateProductInventoryAsync(Guid productId, int quantityOrdered)
        {
            var inventory = await _context.ProductInventories
                .FirstOrDefaultAsync(pi => pi.ProductId == productId);

            if (inventory == null || inventory.QuantityInStock < quantityOrdered)
            {
                return false; 
            }

            inventory.QuantityInStock -= quantityOrdered;
            inventory.ItemSold += quantityOrdered;

            _context.ProductInventories.Update(inventory);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
