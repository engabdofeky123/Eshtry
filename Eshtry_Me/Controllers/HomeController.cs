using Eshtry_Me.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshtry_Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _homeRepository;

        public HomeController(IHomeRepository repo)
        {
            _homeRepository = repo;
        }

        public async Task<IActionResult> GetHomeDataAsync()
        {
            return Ok(await _homeRepository.GetHomeDataAsync());
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var result = await _homeRepository.SearchProductsAsync(searchString);
            if(result.Count == 0)
                return NotFound("No products found matching the search criteria.");
            return Ok(result);
        }

        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
           var product = await _homeRepository.FilterByCategory(categoryId);
            return Ok(product);
        }
    }
}
