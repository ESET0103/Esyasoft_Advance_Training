using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController:ControllerBase
    {
        public readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        } 
    }
}
