using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunglassesDAL.Model;

namespace Sunglasses.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<IEnumerable<Cart>> GetSelectedCartItemsForCheckoutAsync(Guid userId);
        Task<bool> SaveOrderAsync(Order order);
        Task<bool> SaveOrderDetailsAsync(Guid orderId, IEnumerable<Cart> cartItems);
        Task<bool> UpdateCartItemsSelectionToFalse(Guid userId, List<Guid> productIds);
        Task<bool> CheckAndUpdateInventoryAsync(IEnumerable<Cart> cartItems);

    }
}
