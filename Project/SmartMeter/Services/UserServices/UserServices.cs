using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace SmartMeter.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly SmartMeterDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserServices (SmartMeterDbContext context)
        {
            _context = context;
                //private readonly PasswordHasher<User> _passwordHasher;
            _passwordHasher = new PasswordHasher<User> ();
        }

        public async Task<ActionResult<User>> RegisterConsumerAsync(Consumer request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return null;

            var consumer = new Consumer
            {
                Name = request.Name,
                Phone = request.Phone, // St display name
                Email = request.Email,
                Orgunitid = request.Orgunitid,
                Tariffid = request.Tariffid,
                Status = request.Status
 
            };

            // Hash password and convert to byte[]
            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
            user.Passwordhash = Encoding.UTF8.GetBytes(hashedPassword);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
    }
}
