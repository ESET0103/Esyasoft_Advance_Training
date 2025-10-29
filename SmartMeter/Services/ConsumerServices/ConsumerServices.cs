using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using System.Security.Claims;

namespace SmartMeter.Services.ConsumerServices
{
    public class ConsumerServices : IConsumerServices
    {
        private readonly SmartMeterDbContext _context;
        public ConsumerServices(SmartMeterDbContext context)
        {
            _context = context;
        }

        //public async Task<ActionResult<Tariff>> GetConsumerTariffAsync()
        //{
        //    var userIdClaim = _context.Users.FindAsync(ClaimTypes.NameIdentifier)?.Value;

        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized("Invalid Token");
        //    }

        //    if (!long.TryParse(userIdClaim, out long userId))
        //    {
        //        return BadRequest("Invalid User Id");
        //    }

        //    User? user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);


        //    Consumer? consumer = await _context.Consumers.FirstOrDefaultAsync(c => c.Email == user.Email);

        //    var tariff = await _tariffService.GetTariffByIdAsync(consumer.Tariffid);

        //    Console.WriteLine($"consumer tariff =  {tariff}");

        //    if (tariff == null)
        //    {
        //        return NotFound("Tariff not found");
        //    }

        //    return Ok(tariff.Name);

        //}

        public async Task<Consumer?> GetConsumerByUserIdAsync(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);

            if (user == null)
                return null;

            return await _context.Consumers.FirstOrDefaultAsync(c => c.Email == user.Email);
        }
    }
}
