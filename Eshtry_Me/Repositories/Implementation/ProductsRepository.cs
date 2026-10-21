using Eshtry_Me.Data;
using Eshtry_Me.DTOs;
using Eshtry_Me.Repositories.Interfaces;
using EshtryWepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshtry_Me.Repositories.Implementation
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            return await _context.Products.Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    HasDiscount = p.HasActiveOffer,
                    Discount = p.DiscountAmount,
                    OfferCreatedDate = p.OfferCreatedDate,
                    OfferEndDate = p.OfferEndDate
                }).FirstOrDefaultAsync();
        }    
    }
}