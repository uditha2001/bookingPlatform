﻿using ProductService.API.DTO;
using ProductService.API.Models.Entities;

namespace ProductService.API.Services.serviceInterfaces
{
    public interface IProductContentService
    {
        /// <summary>
        /// Creates a new product content entry associated with a given product ID.
        /// </summary>
        /// <param name="productContentDTO">The data transfer object containing content details to be created.</param>
        /// <param name="productId">The ID of the product to which the content belongs.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Returns <c>true</c> if the content was created successfully; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> createContent(ProductContentDTO productContentDTO, long ProductId);

        /// <summary>
        /// Updates an existing product content entry using the specified content ID.only can update contents created using internal system
        /// </summary>
        /// <param name="productContentDTO">The data transfer object containing the updated content details.</param>
        /// <param name="contentId">The ID of the content to be updated.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Returns <c>true</c> if the content was updated successfully; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> updateContent(ProductContentDTO productContentDTO, long contentId);

        /// <summary>
        /// Deletes a product content entry asynchronously by its ID.only can delete contents created using internal system 
        /// </summary>
        /// <param name="id">The ID of the product content to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Returns <c>true</c> if the content was deleted successfully; otherwise, <c>false</c>.
        /// </returns>
        public Task<bool> deleteContent(long id);

        /// <summary>
        /// Retrieves all content entries associated with the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product whose content is to be retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation. Returns a list of <see cref="ProductContentDTO"/> objects.
        /// Returns an empty list if an error occurs during retrieval.
        /// </returns>
        public Task<List<ProductContentDTO>> getAllContent(long productId);

        /// <summary>
        /// Maps a <see cref="ProductContentEntity"/> object to a <see cref="ProductContentDTO"/> object.
        /// </summary>
        /// <param name="entity">The product content entity to be mapped.</param>
        /// <returns>A <see cref="ProductContentDTO"/> object containing the mapped data.</returns>
        ProductContentDTO ToDTO(ProductContentEntity entity);

        /// <summary>
        /// Maps a <see cref="ProductContentDTO"/> object to a <see cref="ProductContentEntity"/> object.
        /// </summary>
        /// <param name="dto">The product content DTO containing the data to map.</param>
        /// <param name="productId">The ID of the product to associate with the content entity.</param>
        /// <returns>A <see cref="ProductContentEntity"/> object containing the mapped data.</returns>
        ProductContentEntity ToEntity(ProductContentDTO dto, long productId);

        /// <summary>
        /// Saves a list of product images to the server and associates them with the specified product.
        /// </summary>
        /// <param name="productId">The ID of the product to associate the images with.</param>
        /// <param name="images">A list of image files to be saved.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> indicating whether the images were successfully saved (true) or not (false).
        /// </returns>
        /// <remarks>
        /// This method saves each image to a folder path structured as "uploads/products/{productId}" under the web root directory.
        /// It generates a unique filename for each image to avoid conflicts, copies the file to the destination,
        /// and creates a corresponding ProductContentDTO to associate with the product.
        /// If the provided images list is null or empty, the method returns false.
        /// </remarks>
        Task<bool> SaveProductImagesAsync(long productId, List<IFormFile> images);


    }
}
