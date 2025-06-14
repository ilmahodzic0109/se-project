using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Implementations;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace Sunglasses.Services.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProducts _productRepository;

        public CheckoutService(ICheckoutRepository checkoutRepository, ICartRepository cartRepository, IProducts productRepository)
        {
            _checkoutRepository = checkoutRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Cart>> GetSelectedCartItemsForCheckoutAsync(Guid userId)
        {
           
            return await _cartRepository.GetSelectedCartItemsAsync(userId);
        }

        public async Task<bool> SaveOrderAsync(Order order)
        {
           
            return await _checkoutRepository.SaveOrderAsync(order);
        }

        public async Task<bool> SaveOrderDetailsAsync(Guid orderId, IEnumerable<Cart> cartItems)
        {
            
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    PriceAtPurchase = cartItem.Product.Price,
                    Subtotal = cartItem.Quantity * cartItem.Product.Price,
                    
                };

                var result = await _checkoutRepository.SaveOrderDetailAsync(orderDetail);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<bool> UpdateCartItemsSelectionToFalse(Guid userId, List<Guid> productIds)
        {
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);

            var itemsToUpdate = cartItems.Where(c => productIds.Contains(c.ProductId)).ToList();

            if (itemsToUpdate.Any())
            {
                foreach (var item in itemsToUpdate)
                {
                    item.IsSelected = false;  
                    await _cartRepository.UpdateCartItemAsync(item);  
                }
                return true;
            }
            return false;
        }

        public async Task<bool> PlaceOrderAsync(Order order, IEnumerable<Cart> cartItems)
        {
           
            foreach (var cartItem in cartItems)
            {
                var inventory = await _checkoutRepository.GetProductInventoryAsync(cartItem.ProductId);

                
                if (inventory == null || inventory.QuantityInStock < cartItem.Quantity)
                {
                    return false; 
                }

                var inventoryUpdated = await _checkoutRepository.UpdateProductInventoryAsync(cartItem.ProductId, cartItem.Quantity);
                if (!inventoryUpdated)
                {
                    return false; 
                }
            }

            
            var orderSaved = await _checkoutRepository.SaveOrderAsync(order);
            if (!orderSaved)
            {
                return false; 
            }

            var orderDetailsSaved = await SaveOrderDetailsAsync(order.OrderId, cartItems);
            return orderDetailsSaved;
        }
        public async Task<bool> CheckAndUpdateInventoryAsync(IEnumerable<Cart> cartItems)
        {
            foreach (var cartItem in cartItems)
            {
                var inventory = await _checkoutRepository.GetProductInventoryAsync(cartItem.ProductId);

                if (inventory == null || inventory.QuantityInStock < cartItem.Quantity)
                {
                    return false; 
                }

                var inventoryUpdated = await _checkoutRepository.UpdateProductInventoryAsync(cartItem.ProductId, cartItem.Quantity);
                if (!inventoryUpdated)
                {
                    return false; 
                }
            }

            return true; 
        }

    }
}
