using Eshtry_Me.DTOs;
using Eshtry_Me.HelperClasses;

namespace Eshtry_Me.Services.Interfaces
{
    public interface IAuth
    {
        Task<AuthModel> LoginAsync(LoginDto loginDto);
        Task<AuthModel> RegisterAsync(RegisterDto registerDto);
        Task SignOut();
    }
}