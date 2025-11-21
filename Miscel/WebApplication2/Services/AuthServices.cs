using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.PasswordHasher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Data;
using WebApplication2.Model;
using System.Security.Cryptography;

namespace WebApplication2.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthServices(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ActionResult<User?>> RegisterAsync(UserDto request)
        {
            if(await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }
            var user = new User();
            user.Email = request.Email;
            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<ActionResult<string>> LoginAsync(UserDto request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
            if (user == null) return null;
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed) return null;
            var token = generateToken(user);
            return token;
        }


        private string generateToken(User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                //new Claim(ClaimTypes.Role,user.
            };

            var key = new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
                );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings: Issue"),
                audience: _configuration.GetValue<string>("AppSettings: Audience"),
                claims: claim,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        
    }
}
