﻿using Microsoft.AspNetCore.Mvc;
using OrderService.API.DTO;
using ProductService.API.DTO;
using ProductService.API.Services;
using ProductService.API.Services.serviceInterfaces;

namespace ProductService.API.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _iproductService;
        public ProductController(IProductService iPoductService)
        {
            _iproductService = iPoductService;
        }
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        [HttpGet("allProducts")]
        public async Task<ActionResult<ProductDTO[]>> GetAllProducts()
        {
            var result = await _iproductService.getAllProducts();

            if (result == null)
                return NotFound("No products found.");

            return Ok(result);
        }

        /// <summary>
        /// create new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO newProduct)
        {
            try
            {
                long result = await _iproductService.createProduct(newProduct);
                return Ok(result);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

          

        }
        /// <summary>
        /// Delete a product byId,only allow to delete products which were added using internal system.
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult> deleteProduct([FromQuery]long productId)
        {
            bool result = await _iproductService.deleteProductAsync(productId);
            if (result)
            {
                return Ok("sucessfull");

            }
            else
            {
                return BadRequest("item not found");
            }
        }
        /// <summary>
        /// getInternalSystemProducts,only retriew products that add through the internal system
        /// </summary>
        [HttpGet("internalSystemProducts")]
        public async Task<IActionResult> getInternalSystemProducts()
        {
            List<ProductDTO> products=await _iproductService.getInternalSystemProducts();
            return Ok(products);
        }

        /// <summary>
        /// Processes the sale of multiple products by accepting a list of checkout details.
        /// </summary>
        /// <param name="request">
        /// A list of <see cref="CheckoutDTO"/> objects representing the products and quantities to be sold.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the sale operation:
        /// <list type="bullet">
        /// <item><c>200 OK</c> if the products were sold successfully.</item>
        /// <item><c>500 Internal Server Error</c> if an error occurred during processing.</item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// The method calls the service layer's <c>SellProducts</c> method to perform the sale logic.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpPatch("sellProducts")]
        public async Task<IActionResult> SellProducts([FromBody] List<CheckoutDTO> request)
        {
            try
            {
               bool result=await  _iproductService.SellProducts(request);
                if (result)
                {
                    return Ok("Products sold successfully.");

                }
                else
                {
                    return StatusCode(500, "An error occurred while processing the request.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SellProducts: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves an external product by its ID.
        /// </summary>
        /// <param name="productId">
        /// The ID of the external product to retrieve, passed as a query parameter.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the <see cref="ProductDTO"/> if found,
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>GetExtranalProductById</c> to fetch product data.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpGet("byId")]
        public async Task<IActionResult> getExternalProductById([FromQuery] long productId)
        {
            try
            {
                return Ok( await _iproductService.GetExtranalProductById(productId));
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves a product by its ID, including both internal and external products.
        /// </summary>
        /// <param name="productId">
        /// The ID of the product to retrieve, passed as a query parameter.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the <see cref="ProductDTO"/> if found,
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>GetProductById</c> to fetch product data from internal or external sources.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpGet("productById")]
        public async Task<IActionResult> getProductsById([FromQuery] long productId)
        {
            try
            {
                return Ok(await _iproductService.GetProductById(productId));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves all product categories.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of <see cref="ProductCategoryDTO"/> objects,
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>getAllCategories</c> to fetch all categories.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpGet("category")]
        public async Task<IActionResult> getAllCategories()
        {
            try
            {
                return Ok(await _iproductService.getAllCategories());
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Retrieves all products owned by a specific user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user whose products are to be retrieved, passed as a query parameter.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of <see cref="ProductDTO"/> objects owned by the user,
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>getOwnerProducts</c> to fetch products associated with the given user ID.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpGet("ownerProducts")]
        public async Task<IActionResult> getOwnerProducts([FromQuery]long userId)
        {
            try
            {
                return Ok(await _iproductService.getOwnerProducts(userId));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Processes a checkout order for a product.
        /// </summary>
        /// <param name="order">
        /// A <see cref="CheckoutDTO"/> object containing the order details to be processed.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating whether the checkout was successful (true) or not (false),
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>GetCheckout</c> to validate and process the checkout.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpPost("checkout")]
        public async Task<IActionResult> checkoutOrders([FromBody] CheckoutDTO order)
        {
            try
            {
                
                return Ok(await _iproductService.GetCheckout(order));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Updates an existing product's details.
        /// </summary>
        /// <param name="product">
        /// A <see cref="ProductDTO"/> object containing the updated product information.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating whether the update was successful (true) or not (false),
        /// or a 500 status code if an error occurs during processing.
        /// </returns>
        /// <remarks>
        /// Calls the service method <c>updateProduct</c> to perform the update.
        /// Exceptions are caught and logged, returning a generic error message on failure.
        /// </remarks>
        [HttpPatch("updateProduct")]
        public async Task<IActionResult> updaeProduct([FromBody] ProductDTO product)
        {
            try
            {
                return Ok(await _iproductService.updateProduct(product));
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in SellProducts: {e.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
