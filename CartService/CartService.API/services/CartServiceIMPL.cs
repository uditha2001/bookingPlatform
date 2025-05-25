using CartService.API.DTO;
using CartService.API.Model.Entities;
using CartService.API.repository.interfaces;
using CartService.API.services.interfaces;
using OrderService.API.DTO;
using System.Text;
using System.Text.Json;

namespace CartService.API.services
{
    public class CartServiceIMPL : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly HttpClient _httpClient;


        public CartServiceIMPL(ICartRepository cartRepository, HttpClient httpClient)
        {
            _cartRepository = cartRepository;
            _httpClient = httpClient;
        }

        public async Task<CartItemDTO> AddItemToCartAsync(CartItemDTO item)
        {
            try
            {
                var entity = ToEntity(item, item.userId);
                var updatedEntity = await _cartRepository.AddOrUpdateCartItemAsync(entity);
                return ToDTO(updatedEntity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to add or update item in cart.", ex);
            }
        }

        public async Task<bool> ClearCartAsync(long userId)
        {
            try
            {
                return await _cartRepository.ClearCartAsync(userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to clear cart.", ex);
            }
        }

        public async Task<List<CartItemDTO>> GetCartByUserIdAsync(long userId)
        {
            try
            {
                List<CartItemEntity> items = await _cartRepository.GetCartByUserIdAsync(userId);
                List<CartItemDTO> allCarts = new List<CartItemDTO>();
                if (items == null || !items.Any())
                {
                    throw new KeyNotFoundException("No items found in cart.");
                }

                foreach (CartItemEntity cart in items)
                {
                    CartItemDTO cartItemDTO=ToDTO(cart);
                    allCarts.Add(cartItemDTO);

                }
                return allCarts;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve cart items.", ex);
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(long cartItemId)
        {
            try
            {
                return await _cartRepository.RemoveItemFromCartAsync(cartItemId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to remove item from cart.", ex);
            }
        }

        public async Task<bool> UpdateItemQuantityAsync(long cartItemId, int newQuantity)
        {
            try
            {
                var items = await _cartRepository.GetCartByUserIdAsync(0); // Dummy call, update logic needed
                var item = items.FirstOrDefault(i => i.cartItemId == cartItemId);
                if (item == null) throw new KeyNotFoundException("Cart item not found.");

                item.Quantity = newQuantity;
                await _cartRepository.AddOrUpdateCartItemAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to update item quantity.", ex);
            }
        }

        public async Task<bool> SubmitOrderAsync(long userId)
        {
            try
            {
                List<CartItemEntity> items = await _cartRepository.GetCartByUserIdAsync(userId);
                List<OrderItemsDTO> orders= new List<OrderItemsDTO>();
                decimal totalPrice= 0;
                foreach(CartItemEntity entity in items)
                {
                    OrderItemsDTO orderItemDTO = new OrderItemsDTO();
                    orderItemDTO.ProductId = entity.ProductId;
                    orderItemDTO.quantity = entity.Quantity;
                    orderItemDTO.itemTotalPrice=entity.itemTotalPrice;
                    totalPrice+= entity.itemTotalPrice;
                    orders.Add(orderItemDTO);
                    
                }
                OrderDTO orderDTO = new OrderDTO();
                orderDTO.items = orders;
                orderDTO.totalOrderprice = totalPrice;
                orderDTO.userId= userId;
                var json = JsonSerializer.Serialize(orderDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5062/api/v1/order", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Order submission failed. StatusCode: {response.StatusCode}");
                }
                else
                {

                }

                    await _cartRepository.ClearCartAsync(userId);
                return true;

                if (!items.Any()) throw new InvalidOperationException("No items in cart to submit.");
                
                await _cartRepository.ClearCartAsync(userId); 
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to submit order.", ex);
            }
        }

        public CartItemEntity ToEntity(CartItemDTO dto, long userId)
        {
            return new CartItemEntity
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                itemTotalPrice = dto.itemTotalPrice,
                
            };
        }

        public CartItemDTO ToDTO(CartItemEntity entity)
        {
            return new CartItemDTO
            {
                userId = entity.UserId,
                cartItemId = entity.cartItemId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                itemTotalPrice = entity.itemTotalPrice,
            };
        }

      
    }
}
