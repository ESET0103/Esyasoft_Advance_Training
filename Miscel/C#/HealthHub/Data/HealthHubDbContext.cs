using HealthHub.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthHub.Data
{
    public class HealthHubDbContext : DbContext
    {
        public HealthHubDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> User{ get; set; }
        public DbSet<Doctors> Doctor { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Doctors>()
        //        .HasOne(u =>u.UserId)
                 
                  
                 
        //         ;
        //}

    }
}
