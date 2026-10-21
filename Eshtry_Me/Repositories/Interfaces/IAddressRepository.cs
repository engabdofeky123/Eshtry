using Eshtry_Me.DTOs;

namespace Eshtry_Me.Repositories.Interfaces
{
    public interface IAddressRepository 
    {
        Task<List<AddressDto>> GetAllAddressesAsync(string userId);
        Task<bool> AddNewAddressAsync(AddressDto dto, string userId);
    }
}
