﻿using AdapterFactory.Adapters;
using OrderService.API.DTO;
using ProductService.API.DTO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;


namespace AdapterFactory.Service
{
    public class AdapterFactoryServiceIMPL : IAdapterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdapterFactoryServiceIMPL> _logger;
        private String[] serviceIdentifiers={ "abc", "cde" };

        public AdapterFactoryServiceIMPL(IServiceProvider serviceProvider,HttpClient httpClient, ILogger<AdapterFactoryServiceIMPL> logger)
        {
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
            _logger = logger;
        }


        public IAdapter GetAdapterById(string adapterId)
        {
            return adapterId.ToLower() switch
            {
                "abc" => _serviceProvider.GetRequiredService<AbcAdapter>(),
                "cde" => _serviceProvider.GetRequiredService<CdeAdapter>(),
                _ => throw new NotSupportedException($"Adapter with ID '{adapterId}' is not supported.")
            };
        }
        public async Task<List<ProductDTO>> getAllProducts()
        {
            var productTasks = serviceIdentifiers.Select(async adapterId =>
            {
                try
                {
                    IAdapter adapter = GetAdapterById(adapterId);
                    return await adapter.GetProductContentsFromExternalServiceAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching products from adapter '{adapterId}': {ex.Message}");
                    return new List<ProductDTO>(); 
                }
            });

            var productLists = await Task.WhenAll(productTasks);

            return productLists.SelectMany(p => p).ToList();
        }

        public async Task<bool> placeOrder(OrderDTO order)
        {
            try
            {
                List<ProductDTO> products = new List<ProductDTO>();
                foreach (OrderItemsDTO orderItems in order.items)
                {
                    _logger.LogInformation("Calling ProductService with ID: {ProductId}", orderItems.ProductId);

                    var response = await _httpClient.GetAsync($"https://localhost:7120/api/v1/product/byId?productId={orderItems.ProductId}");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Order submission failed. StatusCode: {response.StatusCode}");
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    ProductDTO product = JsonSerializer.Deserialize<ProductDTO>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    IAdapter adapter = GetAdapterById(product.provider);
                    adapter.placeOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in checkoutOrder: {ex.Message}");
                throw;
            }



        }

        public async Task<bool> checkoutOrder(CheckoutDTO order)

        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7120/api/v1/product/byId?productId={order.ProductId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Order submission failed. StatusCode: {response.StatusCode}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                ProductDTO product = JsonSerializer.Deserialize<ProductDTO>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                IAdapter adapter = GetAdapterById(product.provider);
                return adapter.checkout();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in checkoutOrder: {ex.Message}");
                throw;
            }
           
            
        }
    }
}
