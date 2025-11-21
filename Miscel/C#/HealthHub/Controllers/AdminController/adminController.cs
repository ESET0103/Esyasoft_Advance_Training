using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using HealthHub.Data;
using HealthHub.Data.Entities;

namespace HealthHub.Controllers.AdminController
{
    [ApiController]
    //[HttpGet("api/admin")]
    public class adminController: ControllerBase
    {
        public readonly HealthHubDbContext _context;
        public adminController(HealthHubDbContext context)
        {
            _context = context;
        }

        [HttpGet("userlist")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            //try
            //{
                var userList = await _context.User.ToListAsync();

                if (userList == null || userList.Count == 0)
                    return NotFound(new { message = "No users found." });

                return Ok(userList);
        //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    return 
            //}
        }
        
    }
}
