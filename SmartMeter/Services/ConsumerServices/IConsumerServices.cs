using SmartMeter.Models;
using SmartMeter.Models.DTOs;

namespace SmartMeter.Services.ConsumerServices
{
    public interface IConsumerServices
    {
        Task<Consumer?> GetConsumerByUserIdAsync(long userId);
    }
}
