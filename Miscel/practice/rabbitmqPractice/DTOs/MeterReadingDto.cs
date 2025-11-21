using System.Diagnostics.Metrics;

namespace rabbitmqPractice.DTOs
{
    public class MeterReadingDto
    {
        //public int Meterreadingid { get; set; }
        public string Meterid { get; set; } = null!;
        public DateTime Meterreadingdate { get; set; } = DateTime.UtcNow;
        public decimal? Energyconsumed { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
        public virtual Meter Meter { get; set; } = null!;
        public required string RoutingKey { get; set; }
    }
}
