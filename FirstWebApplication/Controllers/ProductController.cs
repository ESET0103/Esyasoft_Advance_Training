using FirstWebApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController
    {
        [HttpPost]
        public bool Post([FromBody] Product product)
        {
            Console.WriteLine(product.Name);
            return true;
        }
    }
}
