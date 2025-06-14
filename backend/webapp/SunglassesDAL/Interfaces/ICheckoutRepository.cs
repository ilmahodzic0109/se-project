using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunglassesDAL.Model;

namespace SunglassesDAL.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<bool> SaveOrderAsync(Order order);
        Task<bool> SaveOrderDetailAsync(OrderDetail orderDetail);
        Task<IEnumerable<Cart>> GetSelectedCartItemsAsync(Guid userId);
        Task<ProductInventory?> GetProductInventoryAsync(Guid productId);
        Task<bool> UpdateProductInventoryAsync(Guid productId, int quantityOrdered);
    }
}
