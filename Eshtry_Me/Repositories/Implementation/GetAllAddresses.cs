using Eshtry_Me.Data;
using Eshtry_Me.DTOs;
using Eshtry_Me.Models;
using Eshtry_Me.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshtry_Me.Repositories.Implementation
{
    public class GetAllAddresses : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public GetAllAddresses(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewAddressAsync(AddressDto dto, string userId)
        {
            var address = new Address
            {
                UserId = userId,
                RecipientName = dto.RecipientName,
                PhoneNumber = dto.PhoneNumber,
                Governorate = dto.Governorate,
                City = dto.City,
                StreetAddress = dto.StreetAddress,
                AdditionalDetails = dto.AdditionalDetails,
                IsDefault = dto.IsDefault
            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AddressDto>> GetAllAddressesAsync(string userId) 
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    RecipientName = a.RecipientName,
                    PhoneNumber = a.PhoneNumber,
                    Governorate = a.Governorate,
                    City = a.City,
                    StreetAddress = a.StreetAddress,
                    AdditionalDetails = a.AdditionalDetails,
                    IsDefault = a.IsDefault
                })
                .ToListAsync();
        }
    }
}
