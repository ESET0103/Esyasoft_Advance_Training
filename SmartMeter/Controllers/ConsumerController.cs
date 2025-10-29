using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Services.ConsumerServices;
using SmartMeter.Services.TariffServices;
using System.Security.Claims;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ConsumerController: ControllerBase
    {
        //private readonly SmartMeterDbContext _context;
        private readonly ITariffServices _tariffServices;
        private readonly ConsumerServices _consumerServices;
        public ConsumerController(SmartMeterDbContext context,ConsumerServices consumerServices,ITariffServices tariffServices)
        {
            //_context = context;
            _consumerServices = consumerServices;
            _tariffServices = tariffServices;

        }

        [HttpGet("get-consumer-tariff")]
        //[Authorize]
        public async Task<ActionResult<Tariff>> GetConsumerTariff()
        {



            //Console.WriteLine("We enter in the getconsumertariff ....");
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Console.WriteLine($"token useid {userIdClaim}");
            //if (userIdClaim == null)
            //{
            //    return Unauthorized("Invalid Token");
            //}

            //if (!long.TryParse(userIdClaim, out long userId))
            //{
            //    return BadRequest("Invalid User Id");
            //}

            //User? user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);
            //Console.WriteLine($"User = {user}");

            //Consumer? consumer = await _context.Consumers.FirstOrDefaultAsync(c => c.Email == user.Email);

            //Console.WriteLine($"consumer =  {consumer}");

            //var tariff = await _consumerServices.GetConsumerTariffAsync();

            ////Console.WriteLine($"consumer tariff =  {tariff}");

            //if (tariff == null)
            //{
            //    return NotFound("Tariff not found");
            //}

            //return Ok();


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid Token");

            if (!long.TryParse(userIdClaim, out long userId))
                return BadRequest("Invalid User Id");

            var consumer = await _consumerServices.GetConsumerByUserIdAsync(userId);
            if (consumer == null)
                return NotFound("Consumer not found");

            var tariff = await _tariffServices.GetTariffByIdAsync(consumer.Tariffid);
            if (tariff == null)
                return NotFound("Tariff not found");

            return Ok(new
            {
                TariffName = tariff.Name,
                BaseRate = tariff.Baserate,
                TaxRate = tariff.Taxrate
            });

        }

    }



}
