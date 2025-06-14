using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Implementations;
using SunglassesDAL.Interfaces;
using SunglassesDAL.Model;

namespace Sunglasses.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProducts _productRepository;
        private readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;

        public CartService(ICartRepository cartRepository, IProducts productRepository, EmailService emailService, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<bool> AddProductToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null || product.IsDeleted)
            {
                return false;
            }
            var existingCartItem = await _cartRepository.GetCartItemAsync(userId, productId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;

                await _cartRepository.UpdateCartItemAsync(existingCartItem);
            }
            else
            {
                var newCartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepository.AddCartItemAsync(newCartItem);
            }

            return true;
        }

        public async Task<IEnumerable<Cart>> GetCartItemsAsync(Guid userId)
        {
            return await _cartRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task<bool> UpdateCartItemAsync(Guid userId, Guid productId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
            if (cartItem == null)
            {
                return false;
            }

            cartItem.Quantity = quantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);

            return true;
        }
        public async Task<IEnumerable<Cart>> GetCartItemCountAsync(Guid userId)
        {
            return await _cartRepository.GetCartItemCountAsync(userId);
        }
        public async Task<bool> UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

            if (cartItem == null)
            {
                return false;
            }

            await _cartRepository.UpdateCartItemSelectionAsync(userId, productId, isSelected);

            return true;
        }
        public async Task<IEnumerable<Cart>> GetSelectedCartItemsForCheckoutAsync(Guid userId)
        {
            
            return await _cartRepository.GetSelectedCartItemsAsync(userId);
        }
        public async Task<bool> RemoveItemFromCheckoutAsync(Guid userId, Guid productId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);

            if (cartItem != null)
            {
                
                await _cartRepository.RemoveCartItemAsync(cartItem); 
                return true;
            }

            return false;
        }

        public async Task<bool> ClearCartForUserAsync(Guid userId)
        {
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);

            if (cartItems.Any())
            {
                _cartRepository.RemoveCartItems(cartItems); 

                return true;
            }
            return false;
        }

        public async Task<bool> RemoveItemsFromCartAsync(Guid userId, List<Guid> productIds)
        {
            foreach (var productId in productIds)
            {
                var result = await RemoveItemFromCheckoutAsync(userId, productId);
                if (!result)
                {
                    return false; 
                }
            }

            return true; 
        }

        public async Task<bool> UpdateCartItemSelectionAsync(Guid userId, Guid productId, bool isSelected, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemByProductIdAsync(userId, productId);

            if (cartItem != null)
            {
                
                cartItem.IsSelected = isSelected;
                cartItem.Quantity = quantity;

                await _cartRepository.UpdateCartItemAsync(cartItem);
                return true;
            }

            return false;
        }
        public async Task<bool> AddProductToCartAsync(Guid userId, Guid productId, int quantity, bool isSelected)
        {
            
            var existingCartItem = await _cartRepository.GetCartItemAsync(userId, productId);

            if (existingCartItem != null)
            {
                
                existingCartItem.Quantity = quantity;
                existingCartItem.IsSelected = isSelected;
                existingCartItem.AddedAt = DateTime.Now; 

                await _cartRepository.UpdateCartItemAsync(existingCartItem);
            }
            else
            {
                
                var newCartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    IsSelected = isSelected,
                    AddedAt = DateTime.Now
                };

                await _cartRepository.AddCartItemAsync(newCartItem);
            }

            return true;
        }
        


        public async Task<DateTime?> GetLastAddedProductTimeAsync(Guid userId)
        {
            
            var cartItems = await _cartRepository.GetSelectedCartForEmailItemsAsync(userId);

            
            var lastCartItem = cartItems.OrderByDescending(c => c.AddedAt).FirstOrDefault();

            return lastCartItem?.AddedAt;
        }

        
        public async Task ProcessCartActivityAsync()
        {
            var inactivityThreshold = TimeSpan.FromMinutes(1); 
            var inactiveUserIds = await _cartRepository.GetUsersWithInactiveCartsAsync(inactivityThreshold);

            foreach (var userId in inactiveUserIds)
            {
                var user = await _userRepository.GetUserByIdAsync(userId); 
                if (user != null && !string.IsNullOrEmpty(user.Email)) 
                {
                    
                    await _emailService.SendEmailAsync(user.Email,
                                                       "Your cart's waiting! Finish your purchase before it disappears!",
                                                       "Hi, it looks like you've added some items to your cart. Did you forget to complete your order?");
                    Console.WriteLine($"Email sent to {user.Email} for inactive cart.");
                }
            }
        }


    }
}
    
