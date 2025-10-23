using Microsoft.AspNetCore.Authentication.BearerToken;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public interface IAuthServices
    {

        // this will used for the autherization
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);


       
        Task<User?> RegisterAsync(UserDto request);
    }
}
