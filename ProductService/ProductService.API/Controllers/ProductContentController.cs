using Microsoft.AspNetCore.Mvc;
using ProductService.API.DTO;
using ProductService.API.Services.serviceInterfaces;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/v1/content")]
    public class ProductContentController : ControllerBase
    {
        private readonly IProductContentService _productContentService;

        public ProductContentController(IProductContentService productContentService)
        {
            _productContentService = productContentService;
        }
        /// <summary>
        /// create a content, allow to create onecontent at a time
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ProductContentDTO dto, [FromQuery] long productId)
        {
            var result = await _productContentService.createContent(dto, productId);
            if (!result)
                return StatusCode(500, "Failed to create content.");
            return Ok("Content created successfully.");
        }
        /// <summary>
        /// get all content for the given productId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllContent([FromQuery] long productId)
        {
            var contentList = await _productContentService.getAllContent(productId);
            return Ok(contentList);
        }

        /// <summary>
        /// update a content,onl can update contents ,which created using internal system
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContent(long id, [FromBody] ProductContentDTO dto)
        {
            var result = await _productContentService.updateContent(dto, id);
            if (!result)
                return StatusCode(500, $"Failed to update content with ID {id}.");
            return Ok($"Content with ID {id} updated successfully.");
        }
        /// <summary>
        /// delete a content using it id,only can delete content ,which created using internal system
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(long id)
        {
            var result = await _productContentService.deleteContent(id);
            if (!result)
                return StatusCode(500, $"Failed to delete content with ID {id}.");
            return Ok($"Content with ID {id} deleted successfully.");
        }
    }
}
