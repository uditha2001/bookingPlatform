using OrderService.API.DTO;
using ProductService.API.DTO;
using ProductService.API.Models.Entities;

namespace ProductService.API.Services.serviceInterfaces
{
    /// <summary>
    /// Defines the bussinesss logics for product-related operations.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Maps a <see cref="ProductEntity"/> object to a <see cref="ProductDTO"/> object.
        /// NOTE: This method does not map the Attributes and Contents.
        /// </summary>
        /// <param name="productEntity">The product entity object to be mapped.</param>
        /// <returns>A <see cref="ProductDTO"/> object.</returns>
        public ProductDTO ProductEntityToDTO(ProductEntity productEntity);

        /// <summary>
        /// Maps a <see cref="ProductDTO"/> object to a <see cref="ProductEntity"/> object.
        /// NOTE: This method does not map the Attributes and Contents.
        /// </summary>
        /// <param name="productDTo">The product DTO object to be mapped.</param>
        /// <returns>A <see cref="ProductDTO"/> object.</returns>
        public ProductEntity ProductDTOToEntity(ProductDTO productDTo);

        /// <summary>
        /// Updates products by retrieving data from the external adapters and mapping it to internal product entities.all adapters will import data parallaly,thismethode auto maticaly run when project start.
        /// NOTE:all contents and attributes also imports
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="ProductDTO"/> objects,came in external services or <c>null</c> if the update fails.
        /// </returns>
        Task<List<ProductDTO>?> UpdateProductsFromAdapterAsync();

        /// <summary>
        /// Retrieves all products, including those from internal and external services.
        /// Useful for displaying products in the frontend.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The result contains a list of <see cref="ProductDTO"/> objects.</returns>
        Task<List<ProductDTO>> getAllProducts();


        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="productDto">The product data transfer object containing the product details.</param>
        /// <returns>
        /// A boolean value: returns <c>true</c> if the product was created successfully; otherwise, <c>false</c>.
        /// </returns>
        Task<long> createProduct(ProductDTO productdto);

        /// <summary>
        /// Deletes an existing product asynchronously.
        /// Note: Only products created internally by the system can be deleted.
        /// </summary>
        /// <param name="productId">The ID of the product to be deleted.</param>
        /// <returns>
        /// A boolean value: returns <c>true</c> if the product was deleted successfully; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> deleteProductAsync(long productId);

        /// <summary>
        /// Retrieves all products created using the internal system.
        /// This is used to identify products that can be safely deleted or updated without affecting external systems.
        /// </summary>
        /// <returns>A list of <see cref="ProductDTO"/> objects.</returns>
        Task<List<ProductDTO>> getInternalSystemProducts();

        /// <summary>
        /// Extracts attributes and content from a <see cref="ProductDTO"/> and adds them to a <see cref="ProductEntity"/>.
        /// </summary>
        /// <param name="productdto">The source <see cref="ProductDTO"/> object containing attribute and content data.</param>
        /// <param name="productentity">The target <see cref="ProductEntity"/> object to which the data will be added.</param>

        void extractAttributesAndContentFromProductDTO(ProductDTO productdto, ProductEntity productentity);

        Task<List<ProductDTO>> getOwnerProducts(long userId);
        Task<List<ProductCategoryDTO>> getAllCategories();

        Task<bool> SellProducts(List<CheckoutDTO> orderDto);
        Task<ProductDTO> GetExtranalProductById(long productId);
        Task<ProductDTO> GetProductById(long productId);
        Task<bool> GetCheckout(CheckoutDTO order);


    }
}
