using Microsoft.AspNetCore.Mvc;
using SmartMeter.Data;
using SmartMeter.Services;
using SmartMeter.Services.UserServices;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/user/[Controller]")]
    public class UserController : ControllerBase
    {
        public readonly SmartMeterDbContext SmartMeterDbContext;
        public readonly IUserServices _userServices;
        public UserController(SmartMeterDbContext smartMeterDbContext, IUserServices userServices)
        {
            SmartMeterDbContext = smartMeterDbContext;
            _userServices = userServices;
        }

        
        [HttpGet("consumption-history")]
        //public async Task<IActionResult> GetHistoricalConsumption(
        //    [FromQuery] int orgUnitId,
        //    [FromQuery] DateTime startDate,
        //    [FromQuery] DateTime endDate)
        //{
        //    if (orgUnitId <= 0)
        //        return BadRequest("Invalid OrgUnitId.");

        //    if (startDate > endDate)
        //        return BadRequest("StartDate cannot be after EndDate.");

        //    var result = await _userServices.GetHistoricalConsumptionAsync(orgUnitId, startDate, endDate);
        //    //var result = await _userServices.

        //    return Ok(new
        //    {
        //        OrgUnitId = orgUnitId,
        //        StartDate = startDate,
        //        EndDate = endDate,
        //        Records = result
        //    });
        //}
        public async Task<IActionResult> GetTotalEnergyConsumed([FromQuery] int orgUnitId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();
            // Validate the date range
            if (startDate > endDate)
                return BadRequest("Start date cannot be greater than end date.");

            // Get the historical consumption data from the service
            var historicalData = await _userServices.GetHistoricalConsumptionAsync(orgUnitId, startDate, endDate);

            // Check if no data is found for the provided OrgUnit and date range
            if (historicalData == null)
                return NotFound("No data found for the given OrgUnit and date range.");

            // Return the result
            return Ok(historicalData);
        }





    }
}
