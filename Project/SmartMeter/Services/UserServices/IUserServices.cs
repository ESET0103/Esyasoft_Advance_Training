using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using SmartMeter.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartMeter.Services
{
    public interface IUserServices
    {
        public interface IUserService
        {
            Task<User> RegisterConsumerAsync(User request);
        }
    }
}