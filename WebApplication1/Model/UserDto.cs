namespace WebApplication1.Model
{
    // this class is not stored in database , this is just for passing data through api
    public class UserDto
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
