using SmartMeter.Models;

namespace SmartMeter.Services.RabbitMqService.Utils
{
    public interface IDatabaseService
    {
        Task InsertMeterReadingAsync(Meterreading request);
    }
}
