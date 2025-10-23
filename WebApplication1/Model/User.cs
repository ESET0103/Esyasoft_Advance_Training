using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        //public string Name { get; set; }
        //public int age { get; set; }
        public string Roles { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }

    }
}
