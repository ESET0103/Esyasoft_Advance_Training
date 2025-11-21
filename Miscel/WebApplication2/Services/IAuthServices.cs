using Microsoft.AspNetCore.Mvc;
using WebApplication2.Model;

namespace WebApplication2.Services
{
    public interface IAuthServices
    {
        Task<ActionResult<User?>> RegisterAsync(UserDto request);
        Task<ActionResult<string>> LoginAsync(UserDto request);
    }
}
