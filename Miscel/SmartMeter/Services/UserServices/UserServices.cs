using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace SmartMeter.Services.UserServices
{
    public class UserServices : IUserServices
    {

        public readonly SmartMeterDbContext _context;
        private readonly PasswordHasher<Consumer> _passwordHasherConsumer;
        private readonly PasswordHasher<User> _passwordHasherUser;
        public UserServices(SmartMeterDbContext context)
        {
            _context = context;
            _passwordHasherConsumer = new PasswordHasher<Consumer>();
            _passwordHasherUser = new PasswordHasher<User>();
        }
        
        public async Task<HistoricalConsumptionDto> GetHistoricalConsumptionAsync(int orgUnitId, DateTime startDate, DateTime endDate)
        {
            // Ensure dates are UTC to prevent PostgreSQL timestamp issues
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            // ✅ Fetch OrgUnit details first
            var orgUnit = await _context.Orgunits
                .FirstOrDefaultAsync(o => o.Orgunitid == orgUnitId);

            if (orgUnit == null)
                return null; // OrgUnit not found

            // ✅ Fetch total energy consumed via join from OrgUnit → Consumer → Meter → MeterReading
            var totalEnergyConsumed = await (
                from ou in _context.Orgunits
                join c in _context.Consumers on ou.Orgunitid equals c.Orgunitid
                join m in _context.Meters on c.Consumerid equals m.Consumerid
                join r in _context.Meterreadings on m.Meterserialno equals r.Meterid
                where ou.Orgunitid == orgUnitId
                      && r.Meterreadingdate >= startDate
                      && r.Meterreadingdate <= endDate
                select r.Energyconsumed
            ).SumAsync();

            // ✅ Return the DTO with results
            return new HistoricalConsumptionDto
            {
                OrgUnitName = orgUnit.Name,
                StartDate = startDate,
                EndDate = endDate,
                TotalEnergyConsumed = totalEnergyConsumed
            };
        }

        public async Task<Consumer?> RegisterConsumerAsync(ConsumerDto request)
        {
            bool exists = await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (exists)
                return null;


            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Roles = "Consumer",
                Displayname = request.Name
               
            };
            var consumer = new Consumer
            {
                Username = request.Username,
                Name = request.Username,
                Email = request.Email,
                Phone = request.Phone,
                Orgunitid = request.Orgunitid,
                Tariffid = request.Tariffid,
                Createdat = DateTime.UtcNow,
                //Createdby = 
            };

            // Hash password and convert to byte[]
            var hashedPassword = _passwordHasherConsumer.HashPassword(consumer, request.Password);
            consumer.Passwordhash = Encoding.UTF8.GetBytes(hashedPassword);
            user.Passwordhash = consumer.Passwordhash;

            await _context.Consumers.AddAsync(consumer);
            await _context.Users.AddAsync(user); ;
            await _context.SaveChangesAsync();

            return consumer;

        }


        public async Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto request)
        {
            // Validate new password confirmation
            if (request.NewPassword != request.ConfirmPassword)
            {
                return false;
            }

            // Find user
            User user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }

            // Verify current password
            var storedHashedPassword = Encoding.UTF8.GetString(user.Passwordhash);
            var currentPasswordVerification = _passwordHasherUser.VerifyHashedPassword(user, storedHashedPassword, request.CurrentPassword);

            if (currentPasswordVerification == PasswordVerificationResult.Failed)
            {
                return false;
            }

            // Hash new password
            var newHashedPassword = _passwordHasherUser.HashPassword(user, request.NewPassword);
            user.Passwordhash = Encoding.UTF8.GetBytes(newHashedPassword);

            // Invalidate all refresh tokens (optional security measure)
            //user.RefreshToken = null;
            //user.RefreshTokenExpiry = null;

            // Save changes
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
