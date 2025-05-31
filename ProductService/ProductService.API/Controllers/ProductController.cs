using Microsoft.AspNetCore.Mvc;
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
