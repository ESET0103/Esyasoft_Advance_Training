using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Model
{ 
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public DateTime createdAt  { get; set; } = DateTime.UtcNow;
        public DateTime? updatedAt { get; set; }
        

    }
}
