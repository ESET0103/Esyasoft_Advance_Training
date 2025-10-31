namespace SmartMeter.Models.DTOs
{
    public class ConsumerDto
    {
        //public long Userid { get; set; }
        public long Consumerid { get; set; }

        public string? Username { get; set; }

        public string Email { get; set; } = null!;
        public string? Password { get; set; }

        public string? Name { get; set; }

        public long? Addressid { get; set; }

        public string? Phone { get; set; }


        public int Orgunitid { get; set; }

        public int Tariffid { get; set; }
    }
}
