using AdapterFactory.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.DTO;
using ProductService.API.DTO;

namespace AdapterFactory.controllers
{
    [ApiController]
    [Route("api/v1/Adapter")]
    public class AdapterController : Controller
    {
        private readonly IAdapterFactory _adapterFactory;
        public AdapterController(IAdapterFactory adapterFactory)
        {
            _adapterFactory = adapterFactory;
        }
        [HttpGet]
        public async Task<IActionResult> getAllContents()

        {
            try
            {
                var products = await _adapterFactory.getAllProducts();
                return Ok(products);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching product contents.", error = ex.Message });
            }
        }
        [HttpPost]
        public async  Task<IActionResult> placeOrder([FromBody] OrderDTO order)
        {
            return Ok(await _adapterFactory.placeOrder(order));
        }


    }
}
