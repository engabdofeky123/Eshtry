using Eshtry_Me.DTOs;

namespace Eshtry_Me.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<ProductDto?> GetProductById(int id);
    }
}