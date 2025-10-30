namespace SmartMeter.Models.DTOs
{
    public class TariffDto
    {
        public int TariffId { get; set; }

        public string Effectivefrom { get; set; }

        public string Effectiveto { get; set; }

        public decimal? Baserate { get; set; }

        public decimal? Taxrate { get; set; }
    }
}
