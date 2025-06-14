using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sunglasses.Services.Implementations;
using Sunglasses.Services.Interfaces;
using SunglassesDAL.Model;
using webapp.dto;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly ICheckoutService _checkoutService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IUserService userService, ICheckoutService checkoutService, IProductService productService)
        {
            _cartService = cartService;
            _userService = userService;
            _checkoutService = checkoutService;
            _productService = productService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = request.UserId;
            var user = await _userService.GetUserByIdAsync(userId);

            if (user.IsAdmin)
            {
                return BadRequest("Admins cannot add items to the cart.");
            }

            
            var result = await _cartService.AddProductToCartAsync(userId, request.ProductId, request.Quantity);

            if (result)
            {
                
                
                return Ok(new { message = "Product added to cart successfully." });
            }

            return BadRequest(new { message = "Failed to add product to cart." });
        }


        [HttpGet("view/{userId}")]
        public async Task<IActionResult> ViewCart(Guid userId)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("No items in the cart.");
            }
            decimal total = 0;
            var cartDtos = cartItems.Select(cartItem =>
            {
                var product = cartItem.Product;
                var brand = product.Brand.Name;
                var color = product.Color.Name;
                var image = product.Image;
                var price = product.Price;
                var subtotal = price * cartItem.Quantity;
                total += subtotal;

                return new CartItemDto
                {
                    CartId = cartItem.CartId,
                    Quantity = cartItem.Quantity,
                    AddedAt = cartItem.AddedAt,
                    UserId = cartItem.UserId,
                    ProductId = cartItem.ProductId,
                    BrandName = brand,
                    ColorName = color,
                    Image = image,
                    Price = price,
                    Subtotal = subtotal
                };
            }).ToList();

            return Ok(new
            {
                CartItems = cartDtos,
                TotalAmount = total
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemDto request)
        {
            var result = await _cartService.UpdateCartItemAsync(request.UserId, request.ProductId, request.Quantity);

            if (result)
            {
                return Ok(new { message = "Cart item updated successfully." });
            }

            return BadRequest(new { message = "Failed to update cart item." });
        }

        [HttpGet("count/{userId}")]
        public async Task<IActionResult> GetCartItemCount(Guid userId)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return Ok(new { count = 0 });
            }
            var uniqueProductCount = cartItems.Select(item => item.ProductId).Distinct().Count();
            return Ok(new { count = uniqueProductCount });
        }
        [HttpPut("update-selection")]
        public async Task<IActionResult> UpdateCartItemSelection([FromBody] CartItemSelectionRequest request)
        {
            var result = await _cartService.UpdateCartItemSelectionAsync(request.UserId, request.ProductId, request.IsSelected);

            if (result)
            {
                return Ok(new { message = "Cart item selection updated successfully." });
            }

            return BadRequest(new { message = "Failed to update cart item selection." });
        }

        [HttpGet("get-selected-cart-items/{userId}")]
        public async Task<IActionResult> GetSelectedCartItems(Guid userId)
        {
            
            var cartItems = await _cartService.GetSelectedCartItemsForCheckoutAsync(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("No selected items in cart.");
            }

            var response = cartItems.Select(item => new
            {
                ProductId = item.ProductId,
                BrandName = item.Product.Brand.Name,  
                Quantity = item.Quantity,             
                Total = item.Quantity * item.Product.Price,  
                Subtotal = item.Quantity * item.Product.Price  
            }).ToList();

            var totalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);  

            return Ok(new
            {
                CartItems = response,
                TotalAmount = totalAmount  
            });
        }
        
        [HttpPut("buy-now")]
        public async Task<IActionResult> BuyNow([FromBody] AddToCartRequest request)
        {
            var result = await _cartService.AddProductToCartAsync(request.UserId, request.ProductId, request.Quantity, true);

            if (result)
            {
                return Ok(new { message = "Product successfully added to cart and selected." });
            }

            return BadRequest(new { message = "Failed to add product to cart." });
        }


        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var userId = request.UserId;

            var cartItems = await _cartService.GetSelectedCartItemsForCheckoutAsync(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("No items selected for checkout.");
            }

           
            var inventoryCheckResult = await _checkoutService.CheckAndUpdateInventoryAsync(cartItems);
            if (!inventoryCheckResult)
            {
                return BadRequest("Not enough stock for one or more products.");
            }

            decimal totalPrice = cartItems.Sum(item => item.Quantity * item.Product.Price);

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.Now,
                TotalPrice = totalPrice,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber
            };

            var orderResult = await _checkoutService.SaveOrderAsync(order);
            if (!orderResult)
            {
                return BadRequest("Failed to place the order.");
            }

            var orderDetailsResult = await _checkoutService.SaveOrderDetailsAsync(order.OrderId, cartItems);
            if (!orderDetailsResult)
            {
                return BadRequest("Failed to save order details.");
            }

            var cartItemsIds = cartItems.Select(item => item.ProductId).ToList();
            var removeItemsFromCartResult = await _cartService.RemoveItemsFromCartAsync(userId, cartItemsIds);
            if (!removeItemsFromCartResult)
            {
                return BadRequest(new { message = $"Not enough stock for one or more products." });
            }

            return Ok(new { message = "Order placed successfully and cart cleared.", orderId = order.OrderId });
        }


        [HttpPost("clear-cart/{userId}")]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
          
            var cartClearResult = await _cartService.ClearCartForUserAsync(userId);

            if (!cartClearResult)
            {
                return BadRequest("Failed to clear the cart.");
            }

            return Ok(new { message = "Cart cleared successfully." });
        }


        [HttpPut("remove-selection/{userId}/{productId}")]
        public async Task<IActionResult> RemoveSelection(Guid userId, Guid productId)
        {
           
            var result = await _cartService.RemoveItemFromCheckoutAsync(userId, productId);

            if (result)
            {
                return Ok(new { message = "Product removed from cart and checkout successfully." });
            }

            return BadRequest(new { message = "Failed to remove product from cart." });
        }
    }

    public class AddToCartRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }
    }
    public class BuyNowRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class PlaceOrderRequest
    {
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }
    }
}
