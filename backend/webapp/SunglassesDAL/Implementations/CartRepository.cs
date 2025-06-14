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
    public class CartRepository : ICartRepository
    {
        private readonly WebshopContext _context;

        public CartRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartItemAsync(Guid userId, Guid productId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId );
        }

        public async Task AddCartItemAsync(Cart cartItem)
        {
            await _context.Carts.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(Cart cartItem)
        {
            _context.Carts.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId && c.IsSelected == false)
                .Include(c => c.Product) 
                .ThenInclude(p => p.Brand) 
                .Include(c => c.Product.Color) 
                .ToListAsync();
        }
        public async Task ClearCartForUserAsync(Guid userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Cart>> GetCartItemCountAsync(Guid userId)
        {
            return await _context.Carts
         .Where(c => c.UserId == userId)
         .Include(c => c.Product) 
         .ToListAsync();
        }
        public async Task UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.IsSelected = isSelected;
                _context.Carts.Update(cartItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Cart>> GetSelectedCartItemsAsync(Guid userId)
        {
           
            return await _context.Carts
                .Where(c => c.UserId == userId && c.IsSelected==true) 
                .Include(c => c.Product)  
                .ThenInclude(p => p.Brand) 
                .Include(c => c.Product.Color) 
                .ToListAsync();
        }
        public async Task<IEnumerable<Cart>> GetSelectedCartForEmailItemsAsync(Guid userId)
        {
            return await _context.Carts
         .Where(c => c.UserId == userId)
         .ToListAsync(); 
        
    }

        public async Task RemoveCartItems(IEnumerable<Cart> cartItems)
        {
         
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
       
        public async Task<Cart?> GetCartItemByProductIdAsync(Guid userId, Guid productId)
        {
            return await _context.Carts
                                 .Where(x => x.UserId == userId && x.ProductId == productId)
                                 .FirstOrDefaultAsync();
        }
        public async Task RemoveCartItemAsync(Cart cartItem)
        {
            _context.Carts.Remove(cartItem);  
            await _context.SaveChangesAsync();  
        }
        public async Task<List<Guid>> GetUsersWithInactiveCartsAsync(TimeSpan inactivityThreshold)
        {
            var thresholdTime = DateTime.UtcNow.Subtract(inactivityThreshold); 

            var inactiveCarts = await _context.Carts
                .Where(cart => cart.AddedAt <= thresholdTime) 
                .Select(cart => cart.UserId) 
                .Distinct() 
                .ToListAsync();

            return inactiveCarts;
        }
    }
}
