using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Services.TariffServices;
using System.Security.Claims;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/consumer/[Controller]")]
    public class ConsumerController: ControllerBase
    {
        private readonly SmartMeterDbContext _context;
        private readonly ITariffServices _tariffService;
        public ConsumerController(SmartMeterDbContext context)
        {
            _context = context;
        }

        [HttpPost("get-consumer-tariff")]
        //[Authorize]
        public async Task<ActionResult<string>> GetConsumerTariffAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null) {
                return Unauthorized("Invalid Token");
            }
            User? user = await _context.Users.FirstOrDefaultAsync(u=>u.Userid = userId);
            if (user == null)
            {
                return null;
            }

            Consumer? consumer = await _context.Consumers.FindAsync(u => u.Email == userId);
            var tariff = await _tariffService(user.Tariff);
            if (tariff == null) {
                return null;
            }
            return Ok(tariff);

        }

    }



}
