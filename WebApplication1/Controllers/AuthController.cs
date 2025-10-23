using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Model;
using System.Configuration;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using Azure.Messaging;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        //private readonly IConfiguration _configuration;
        private readonly IAuthServices _services;

        public AuthController(IAuthServices services)
        {
            this._services = services;

        }

        //private static User? user = new();


        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {

            var user = await _services.RegisterAsync(request);
            if (user is null) return BadRequest("user already exist...");
            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var token = await _services.LoginAsync(request);
            if (token is null)
            {
                return BadRequest("email/password is wrong");
            }
            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var token = await _services.RefreshTokenAsync(request);
            if(token is null)
            {
                return BadRequest("Invalid/expired Token");
            }

            return Ok(token);
        }

        [HttpGet("Auth-end points")]
        [Authorize]

        public ActionResult AuthCheck()
        {
            return Ok();
        }

        [HttpGet("Admin-end points")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCheck()
        {
            Console.WriteLine("Admin accessed...");
            return Ok();
        }

        //[HttpPost("register")]
        //public ActionResult<User?> Register(UserDto request)
        //{
        //    user.email = request.email;
        //    user.passwordHash = new PasswordHasher<User>().HashPassword(user, request.password);
        //    return Ok(User);
        //}



        //[HttpPost("login")]
        //public ActionResult<User?> Login(UserDto request)
        //{
        //    if (user.email != request.email) {
        //        return BadRequest("User not found");
        //    }
        //    if (new PasswordHasher<User>().VerifyHashedPassword(user, user.passwordHash, request.password) == PasswordVerificationResult.Failed) {
        //        return BadRequest("Invalid Password");
        //    }
        //    string token = generateToken(user);
        //    return Ok(token); 
        //}

        //private string generateToken(User user)
        //{ 
        //    // claims -> who the user is and data need to pass along with token
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email , user.email)
        //    };
        //    // generate key
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSetting:Token")!));
        //    // now sign vis encryption algorithm
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        //    // now create token
        //    var tokenDescriptor = new JwtSecurityToken(
        //            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
        //            audience: _configuration.GetValue<string>("AppSettings:Audience"),
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddDays(1),
        //            signingCredentials: creds
        //        );
        //    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        //}
    }
}
