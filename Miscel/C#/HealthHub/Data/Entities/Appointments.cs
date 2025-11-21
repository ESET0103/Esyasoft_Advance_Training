using System.ComponentModel.DataAnnotations;

namespace HealthHub.Data.Entities
{
    public class Appointments
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public Users User { get; set; }
        [Required]
        public int DoctorProfileId { get; set; }
        [Required]
        //public DoctorProfile DoctorProfile { get; set; }
        //[Required]
        public DateTime StartAt { get; set; }
        [Required]
        public DateTime EndAt { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
    }
}
