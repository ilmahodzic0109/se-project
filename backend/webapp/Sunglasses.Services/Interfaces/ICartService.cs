using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunglassesDAL.Model;

namespace Sunglasses.Services.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddProductToCartAsync(Guid userId, Guid productId, int quantity);
        Task<IEnumerable<Cart>> GetCartItemsAsync(Guid userId);
        Task<bool> UpdateCartItemAsync(Guid userId, Guid productId, int quantity);
        Task<IEnumerable<Cart>> GetCartItemCountAsync(Guid userId);
        Task<bool> UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected);
        Task<IEnumerable<Cart>> GetSelectedCartItemsForCheckoutAsync(Guid userId);
        Task<bool> RemoveItemFromCheckoutAsync(Guid userId, Guid productId);
        Task<bool> ClearCartForUserAsync(Guid userId);
        Task<bool> UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected, int quantity);
        Task<bool> AddProductToCartAsync(Guid userId, Guid productId, int quantity, bool isSelected);
        Task<bool> RemoveItemsFromCartAsync(Guid userId, List<Guid> productIds);
       
        Task<DateTime?> GetLastAddedProductTimeAsync(Guid userId);
        
        Task ProcessCartActivityAsync();
    }
}
