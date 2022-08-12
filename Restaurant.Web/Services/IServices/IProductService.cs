using Restaurant.Web.Models;

namespace Restaurant.Web.Services.IServices
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int idProduct);
        Task<T> CreateProductAsync<T>(ProductDto productDto);
        Task<T> UpdateProductAsync<T>(ProductDto productDto);
        Task<T> DeleteProductAsync<T>(int idProduct);

    }
}
