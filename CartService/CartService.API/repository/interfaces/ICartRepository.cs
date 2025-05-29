using CartService.API.DTO;
using CartService.API.Model.Entities;

namespace CartService.API.repository.interfaces
{
   public interface ICartRepository
    {
        /// <summary>
        /// Retrieves all cart items for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>List of cart items.</returns>
        Task<List<CartItemEntity>> GetCartByUserIdAsync(long userId);

        /// <summary>
        /// Adds a new item to the cart or updates the quantity if it already exists.
        /// </summary>
        /// <param name="item">The cart item entity.</param>
        /// <returns>The boolean value .</returns>
        Task<bool> AddOrUpdateCartItemAsync(CartItemEntity item);

        /// <summary>
        /// Updates the quantity of a specific cart item.
        /// </summary>
        /// <param name="cartItemId">The cart item ID.</param>
        /// <param name="newQuantity">The new quantity.</param>
        /// <returns>True if updated successfully; otherwise false.</returns>
        //Task<bool> UpdateItemQuantityAsync(long cartItemId, int newQuantity);

        /// <summary>
        /// Removes a specific item from the cart.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>True if removed successfully; otherwise false.</returns>
        Task<bool> RemoveItemFromCartAsync(long cartItemId);

        /// <summary>
        /// Clears all items from a user's cart.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>True if cleared successfully; otherwise false.</returns>
        Task<bool> ClearCartAsync(long userId);

        Task<bool> UpdateItemQuantityAsync(long cartItemId, int newQuantity,decimal newTotalPrice);
    }

}
