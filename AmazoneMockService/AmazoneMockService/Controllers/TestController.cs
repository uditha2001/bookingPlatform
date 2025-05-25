using CdeMockService.DTO;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.DTO;
using System.Collections.Generic;

namespace CdeMockService.Controllers
{
    [ApiController]
    [Route("api/v1/product")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMockData()
        {
            var products = new List<CdeDTO>();

            for (int i = 1; i <= 20; i++)
            {
                var product = new CdeDTO
                {
                    originId = i,
                    Name = $"Item {Convert.ToChar(64 + i)}", // Item A, Item B, etc.
                    Description = $"Description for Item {Convert.ToChar(64 + i)}",
                    Price = 50m + i * 10,
                    Currency = "USD",
                    CategoryId = Guid.NewGuid(),
                    owner ="owner "+i,
                    availableQuantity=10+i,
                    Attributes = new List<ProductAttributesDTO>(), // empty for now
                    Contents = new List<ProductContentDTO>
                    {
                        new ProductContentDTO
                        {
                            contentId = 1000 + i,
                            Type = "Image",
                            Url = $"http://example.com/image{Convert.ToChar(64 + i)}.jpg",
                            Description = $"Image content for Item {Convert.ToChar(64 + i)}"
                        }
                    }
                };

                products.Add(product);
            }

            return Ok(products);
        }
        [HttpPost]
        public IActionResult placeOrder()
        {
            return Ok("order placement sucessfull");
        }
    }
    }
