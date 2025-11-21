using HealthHub.Data;
using HealthHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using HealthHub.Services;

namespace HealthHub.Controllers.Auth
{
    public class LoginController : ControllerBase
    {
        private readonly HealthHubDbContext _context;
        private readonly JwtServices _jwtServices;
        public LoginController(HealthHubDbContext context, JwtServices jwtServices)
        {
            _context = context;
            _jwtServices = jwtServices;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Users request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hashedPassword = HashPassword(request.Password);
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == hashedPassword);

            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = _jwtServices.GenerateToken(user);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new { user.Id, user.Name, user.Email, user.UserType, user.Gender, user.Phone }
            });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        
    }

}
