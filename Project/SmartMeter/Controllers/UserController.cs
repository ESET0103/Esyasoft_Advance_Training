using Microsoft.AspNetCore.Mvc;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Services.UserServices;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/user/[Controller]")]
    public class UserController : ControllerBase
    {
        public readonly SmartMeterDbContext SmartMeterDbContext;
        public readonly UserServices _userServices;
        public UserController(SmartMeterDbContext smartMeterDbContext, UserServices userServices)
        {
            SmartMeterDbContext = smartMeterDbContext;
            _userServices = userServices;
        }

        //public async Task<ActionResult<User>> RegisterConsumer(Consumer request)
        //{
        //    var consumer = await _userServices.RegisterConsumerAsync(request);
        //    if (consumer == null) return null;
        //    return Ok(consumer);

        //}




    }
}
