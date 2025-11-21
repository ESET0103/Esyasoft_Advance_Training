namespace HealthHub.Services
{
    public class JwtSetting
    {
        public string Secrete_key {  get; set; }
        public int ExpiryMinute { get; set; } = 60;
    }
}
