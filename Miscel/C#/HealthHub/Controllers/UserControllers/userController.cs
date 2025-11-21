using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using HealthHub.Data;     
using HealthHub.Data.Entities;

namespace HealthHub.Controllers.UserControllers
{
    [ApiController]
    [Route("api/user/update/{id}")]
    public class userController: ControllerBase
    {

        // take apointments
        // update profile
        // filter doctors
        // request for doctor
        public readonly HealthHubDbContext _context;
        public userController(HealthHubDbContext context)
        {
            _context = context;
        }


        //[HttpPost("updateprofile")]
        //public async Task<IActionResult>  updateProfile([FromBody] Users user)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var User = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        //    if(User == null) return NotFound(new {message = "user"})

        //}


    }
}
