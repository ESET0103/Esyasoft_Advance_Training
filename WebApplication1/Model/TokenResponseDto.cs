using Microsoft.Identity.Client;

namespace WebApplication1.Model
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken {  get; set; } = string.Empty;
    }
}