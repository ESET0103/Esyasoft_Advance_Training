using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthHub.Data.Entities
{
    public class Doctors
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public Users User { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        public bool Approved { get; set; }
        [Required]
        public ICollection<Appointments> Appointments { get; set; }
    }
}
