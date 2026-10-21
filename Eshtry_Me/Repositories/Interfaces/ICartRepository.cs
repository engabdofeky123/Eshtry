using Eshtry_Me.DTOs;

namespace Eshtry_Me.Repositories.Interfaces
{
    public interface ICartRepository
    {
        public Task<CompleteOrderResult> CompleteOrder(CompleteOrderDto dto, string userId);
        public Task<CartDto> GetCartByUserId(string userId);
        Task<AddToCartResult> AddToCartAsync(AddToCartModel model, string userId);

    }
}
