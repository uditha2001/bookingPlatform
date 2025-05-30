using OrderService.API.DTO;
using ProductService.API.Data;
using ProductService.API.DTO;
using ProductService.API.Models.Entities;

namespace ProductService.API.Repository.RepositoryInterfaces
{
    public interface IProductRepo
    {
        /// <summary>
        /// Removes all product attributes from the specified product entity that have a non-empty provider value.
        /// </summary>
        /// <param name="existingProduct">The <see cref="ProductEntity"/> containing the attributes to be removed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAllProductAttributesByProvider(ProductEntity existingProduct);

        /// <summary>
        /// Removes all product content entries from the specified product entity that have a non-empty provider value.
        /// </summary>
        /// <param name="existingProduct">The <see cref="ProductEntity"/> containing the content entries to be removed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAllProductContentsWhereProviderNotEmpty(ProductEntity existingProduct);

        /// <summary>
        /// Retrieves an existing product entity that matches the given <see cref="ProductDTO"/> based on origin ID and provider.
        /// Includes related attributes and contents in the query.
        /// </summary>
        /// <param name="productDto">The <see cref="ProductDTO"/> containing origin ID and provider to match.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the matched <see cref="ProductEntity"/> 
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        Task<ProductEntity> GetProductIfExistsAsync(ProductDTO productDto);

        /// <summary>
        /// Adds a new product to the database asynchronously.
        /// </summary>
        /// <param name="product">The <see cref="ProductEntity"/> object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task addProduct(ProductEntity productEntity);

        /// <summary>
        /// Updates an existing product entity in the database.only allow update entity created via internal system
        /// </summary>
        /// <param name="product">The <see cref="ProductEntity"/> to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        Task UpdateProductAsync(ProductEntity product);

        /// <summary>
        /// Retrieves all product entities from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="ProductEntity"/> objects.
        /// </returns>
        Task<List<ProductEntity>> getAllProducts();

        /// <summary>
        /// Saves a new product entity to the database.
        /// </summary>
        /// <param name="productEntity">The <see cref="ProductEntity"/> to be saved.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task<long> saveProduct(ProductEntity productEntity);

        /// <summary>
        /// Deletes a product with the specified ID if it was created internally 
        /// (i.e., has an empty Provider and originId of -1).
        /// </summary>
        /// <param name="productId">The ID of the product to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when a product with the specified ID does not exist or does not match the internal system criteria.
        /// </exception>
        Task deleteProductAsync(long productId);

        /// <summary>
        /// Retrieves all products created by the internal system, identified by an empty Provider and originId of -1.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="ProductEntity"/> objects.
        /// </returns>
        Task<List<ProductEntity>> getInternalSystemProducts();

        /// <summary>
        /// Updates the remaining item count for a specific product.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="restItemsCount">The new remaining count to set for the product.</param>
        /// <returns>
        /// A boolean indicating whether the update was successful.
        /// Returns <c>true</c> if the product was found and updated; otherwise, <c>false</c>.
        /// </returns>

        Task<bool> sellProducts(long productId, int restItemsCount);

        Task<List<ProductCategoryEntity>> getAllCategories();


        Task<ProductEntity> getExternalProductByIdAsync(long productId);
        Task<List<ProductEntity>> getOwnerProducts(long userId);
        Task<ProductEntity> GetProductById(long productId);
        Task<ProductEntity> chekout(CheckoutDTO order);

        Task<bool> checkInternalSystemProduct(long productId);
        



    }
}
