using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Model;
using WebApplication2.Services;
namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController:ControllerBase
    {
        private readonly AppDbContext _context;
        private  readonly AuthServices _authServices;
        public AuthController(AppDbContext context , AuthServices authServices)
        {
            _context = context;
            _authServices = authServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {
            var user = await _authServices.RegisterAsync(request);
            if (user is null) return null;
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> login(UserDto request)
        {
            //var user = await _context.Users.FirstOrDefaultAsync
            var token = await _authServices.LoginAsync(request);
            if (token is null) return BadRequest("Invalid username/password ...");
            return Ok(token);
        }
    }
}
