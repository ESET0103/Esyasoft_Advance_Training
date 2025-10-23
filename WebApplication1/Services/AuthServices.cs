using WebApplication1.Data;
using System.Text;

using WebApplication1.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Azure.Core;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
namespace WebApplication1.Services
{
    public class AuthServices:IAuthServices
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthServices(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        // Registeration 
        public async Task<User ?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.email == request.email)) return null;
            var user = new User();
            user.email = request.email;
            user.passwordHash = new PasswordHasher<User>().HashPassword(user, request.password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


        // Login

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (user == null) return null;
            if(new PasswordHasher<User>().VerifyHashedPassword(user,user.passwordHash,request.password) == PasswordVerificationResult.Failed) return null;
            var token = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
            return token;
        }


        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            var refreshToken = Convert.ToBase64String(randomNumber);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Roles)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<String>("AppSetting:Token")!));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                    issuer: _configuration.GetValue<String>("AppSetting:Issuer"),
                    audience: _configuration.GetValue<string>("AppSetting:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        async Task<TokenResponseDto?>IAuthServices.RefreshTokenAsync (RefreshTokenRequestDto request)
        {
            var user = await _context.Users.FindAsync(request.Id);
            if(user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return null;
            }
            var token = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
            return token;
        }

    }
}
