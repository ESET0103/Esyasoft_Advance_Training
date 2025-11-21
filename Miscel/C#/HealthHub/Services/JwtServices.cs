using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthHub.Data.Entities;
using HealthHub.Data;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace HealthHub.Services
{
    public class JwtServices
    {
        private readonly JwtSetting _jwtSettings;
        public JwtServices(JwtSetting jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateToken(Users user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secrete_key);
            var tokenHandler = new JwtSecurityTokenHandler ();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType)
            }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinute),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
