using Eshtry_Me.DTOs;

namespace Eshtry_Me.Repositories.Interfaces 
{
    public interface IHomeRepository
    {
        Task<HomeDataDto> GetHomeDataAsync();
        Task<List<ProductDto>> SearchProductsAsync(string searchString);
        Task<List<ProductDto>> FilterByCategory(int id);
    }
}
