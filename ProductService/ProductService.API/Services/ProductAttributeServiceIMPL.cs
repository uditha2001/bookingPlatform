﻿using ProductService.API.DTO;
using ProductService.API.Models.Entities;
using ProductService.API.Repository.RepositoryInterfaces;
using ProductService.API.Services.serviceInterfaces;

namespace ProductService.API.Services
{
    public class ProductAttributeServiceIMPL : IProductAttributeService
    {
        private readonly IProductAttriuteRepository _repo;
        private readonly ILogger<ProductAttributeServiceIMPL> _logger;
        public ProductAttributeServiceIMPL(IProductAttriuteRepository repo, ILogger<ProductAttributeServiceIMPL> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<bool> createAttribute(List<ProductAttributesDTO> productAttributesDTOs, long productId)
        {
            try
            {
                if (productAttributesDTOs != null)
                {
                   List<ProductAttributesEntity> productAttributesEntities = new List<ProductAttributesEntity>();
                    foreach (ProductAttributesDTO productAttributesDTO in productAttributesDTOs)
                    {
                        ProductAttributesEntity productAttributesEntity = ToEntity(productAttributesDTO,productId);
                        productAttributesEntities.Add(productAttributesEntity);
                    }
                    bool result = await _repo.createAttributes(productAttributesEntities);
                    return result;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.GetType().Name}, Message: {e.Message}, StackTrace: {e.StackTrace}");
                _logger.LogError(e, "An error occurred while processing the request.");
                return false;
            }
        }

        public async Task<bool> deleteAttribute(long attributeId)
        {
            try
            {
                bool result = await _repo.deleteAttribute(attributeId);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.GetType().Name}, Message: {e.Message}, StackTrace: {e.StackTrace}");
                _logger.LogError(e, "An error occurred while processing the request.");
                return false;
            }
        }

        public async Task<List<ProductAttributesDTO>> getAllAttributes(long productId) 
        {
           
                List<ProductAttributesDTO> productAttributesDTOs = new List<ProductAttributesDTO>();
                List<ProductAttributesEntity> productAttributeEntity =await _repo.getAllAttributes(productId);
                foreach (ProductAttributesEntity attributesEntity in productAttributeEntity)
                {
                    ProductAttributesDTO productAttributesDTO = ToDTO(attributesEntity);
                    productAttributesDTOs.Add(productAttributesDTO);
                }
               return productAttributesDTOs;     
            
        }

        public async Task<bool> updateAttribute(List<ProductAttributesDTO> productAttributeDTOs, long productId)
        {
            try
            {
                List<ProductAttributesEntity> productAttributesEntities = new List<ProductAttributesEntity>();
                foreach (ProductAttributesDTO productAttributesDTO in productAttributeDTOs)
                {
                    ProductAttributesEntity productAttributesEntity = ToEntity(productAttributesDTO, productId);
                    productAttributesEntities.Add(productAttributesEntity);
                }
                bool result = await _repo.UpdateAttributesAsync(productAttributesEntities);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.GetType().Name}, Message: {e.Message}, StackTrace: {e.StackTrace}");
                _logger.LogError(e, "An error occurred while processing the request.");
                return false;
            }
        }
        public ProductAttributesEntity ToEntity(ProductAttributesDTO dto, long productId)
        {
            return new ProductAttributesEntity
            {
                AttributeId = dto.attributeId,
                provider = dto.provider,
                Key = dto.Key,
                Value = dto.Value,
                ProductId = productId
            };
        }
        public ProductAttributesDTO ToDTO(ProductAttributesEntity entity)
        {
            return new ProductAttributesDTO
            {
                attributeId = entity.AttributeId,
                provider = entity.provider,
                Key = entity.Key,
                Value = entity.Value
            };
        }
    }
}
