using Restaurant.Web.Models;

namespace Restaurant.Web.Services.IServices
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int idProduct,string token);
        Task<T> CreateProductAsync<T>(ProductDto productDto, string token);
        Task<T> UpdateProductAsync<T>(ProductDto productDto, string token);
        Task<T> DeleteProductAsync<T>(int idProduct, string token);

    }
}
