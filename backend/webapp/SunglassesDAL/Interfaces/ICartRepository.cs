using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunglassesDAL.Model;

namespace SunglassesDAL.Interfaces
{
    public interface ICartRepository
    {
       Task<Cart> GetCartItemAsync(Guid userId, Guid productId);
        Task AddCartItemAsync(Cart cartItem);
        Task UpdateCartItemAsync(Cart cartItem);
        Task ClearCartForUserAsync(Guid userId);
        Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(Guid userId);
        Task<IEnumerable<Cart>> GetCartItemCountAsync(Guid userId);
        Task UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected);
        Task<IEnumerable<Cart>> GetSelectedCartItemsAsync(Guid userId);
        Task RemoveCartItems(IEnumerable<Cart> cartItems);
        Task<Cart?> GetCartItemByProductIdAsync(Guid userId, Guid productId);
        Task RemoveCartItemAsync(Cart cartItem);
        Task<IEnumerable<Cart>> GetSelectedCartForEmailItemsAsync(Guid userId);
        Task<List<Guid>> GetUsersWithInactiveCartsAsync(TimeSpan inactivityThreshold);
    }

}
