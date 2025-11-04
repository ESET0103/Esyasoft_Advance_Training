using Microsoft.AspNetCore.Mvc;
using SmartMeter.Data;
using SmartMeter.Models.DTOs;
using SmartMeter.Models;
using SmartMeter.Services;
using SmartMeter.Services.UserServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles = "Client")]
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

        [HttpPost("register-consumer")]

        public async Task<ActionResult> RegisterConsumer(ConsumerDto request)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Unauthorized("User not authenticated.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("UserId")?.Value;

            var consumer = await _userServices.RegisterConsumerAsync(request);
            if (consumer is null) return BadRequest("Consumer already exist");
            consumer.Createdby = userIdClaim;
            consumer.Updatedby = userIdClaim;
            return Ok(consumer);

        }


        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            // Get user ID from JWT token
            Console.WriteLine(request.CurrentPassword);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
            {
                return Unauthorized("Invalid token");
            }

            // Validate request
            if (string.IsNullOrEmpty(request.CurrentPassword) ||
                string.IsNullOrEmpty(request.NewPassword) ||
                string.IsNullOrEmpty(request.ConfirmPassword))
            {
                return BadRequest("All fields are required");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("New password and confirmation do not match");
            }

            // Attempt to change password
            var result = await _userServices.ChangePasswordAsync(userId, request);

            if (!result)
            {
                return BadRequest("Failed to change password. Please check your current password.");
            }

            return Ok("Password changed successfully");
        }


    }
}
