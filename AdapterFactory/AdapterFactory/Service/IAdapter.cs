using ProductService.API.DTO;

namespace AdapterFactory.Service
{
    public interface IAdapter
    {
        Task<List<ProductDTO>> GetProductContentsFromExternalServiceAsync();
        string placeOrder();

    }

}
