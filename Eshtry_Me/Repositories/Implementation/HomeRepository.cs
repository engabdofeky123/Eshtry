using Eshtry_Me.Data;
using Eshtry_Me.DTOs;
using Eshtry_Me.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshtry_Me.Repositories.Implementation
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> FilterByCategory(int id)
        {
           return await _context.Products
                .Where(p => p.CategoryId == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToListAsync();
        }

        public async Task<HomeDataDto> GetHomeDataAsync()
        {
            return new HomeDataDto
            {
                OfferProducts = await _context.Products
                    .Where(p => p.HasActiveOffer == true)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl
                    }).ToListAsync(),

                Categories = await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                    .ToListAsync()
            };
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string searchString)
        {
            var result = await _context.Products.Where(p=> p.Name.Contains(searchString) || p.Description.Contains(searchString))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToListAsync();

            return result;
        }
    }
}
