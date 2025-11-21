using Microsoft.EntityFrameworkCore;
using API_demo.Models;

namespace API_demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product>Products { get; set; }
    }
}
