using HealthHub.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using HealthHub.Data;       // Replace with your actual namespace
using HealthHub.Data.Entities;     // Replace with your actual namespace

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly HealthHubDbContext _context;

        public RegistrationController(HealthHubDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            if (await _context.User.AnyAsync(u => u.Email == user.Email))
                return BadRequest(new { Message = "Email already registered." });

            // Hash the password
            user.Password = HashPassword(user.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Registration successful",
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
