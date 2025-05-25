using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.API.Data;
using ProductService.API.DTO;
using ProductService.API.Models.Entities;
using ProductService.API.Repository.RepositoryInterfaces;
using ProductService.API.Services.serviceInterfaces;
using Quartz;
using System.Net.Http;

namespace ProductService.API.Services
{
    public class ProductContentServiceIMPL : IProductContentService
    {
        private readonly IProductContentRepository _repository;
        private readonly ILogger<ProductContentServiceIMPL> _logger;
        private readonly HttpClient _httpClient;
        private readonly ProductDbContext _dbContext;

        public ProductContentServiceIMPL(IProductContentRepository repository, ILogger<ProductContentServiceIMPL> logger, ProductDbContext dbContext)
        {
            _repository = repository;
            _logger = logger;
            _httpClient = new HttpClient();
            _dbContext = dbContext;
        }

        public async Task<bool> createContent(ProductContentDTO productContentDTO, long productId)
        {
            try
            {
                var entity = ToEntity(productContentDTO, productId);
                return await _repository.CreateContentAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product content for ProductId: {ProductId}", productId);
                return false;
            }
        }

        public async Task<bool> deleteContent(long id)
        {
            try
            {
                return await _repository.DeleteContentByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product content with ContentId: {ContentId}", id);
                return false;
            }
        }

        public async Task<List<ProductContentDTO>> getAllContent(long productId)
        {
            try
            {
                var entities = await _repository.GetAllContentAsync(productId);
                return entities.Select(ToDTO).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all product content.");
                return new List<ProductContentDTO>();
            }
        }

        public async Task<bool> updateContent(ProductContentDTO productContentDTO, long contentId)
        {
            try
            {
                var entity = ToEntity(productContentDTO, contentId);
                return await _repository.UpdateContentAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product content with ContentId: {ContentId}", contentId);
                return false;
            }
        }

        public ProductContentDTO ToDTO(ProductContentEntity entity)
        {
            return new ProductContentDTO
            {
                provider = entity.provider,
                contentId = entity.ContentId,
                Type = entity.Type,
                Url = entity.Url,
                Description = entity.Description
            };
        }

        public ProductContentEntity ToEntity(ProductContentDTO dto, long productId)
        {
            return new ProductContentEntity
            {
                ContentId = dto.contentId,
                provider = dto.provider,
                Type = dto.Type,
                Url = dto.Url,
                Description = dto.Description,
                ProductId = productId
            };
        }
   
    }
}
