using rabbitmqPractice.DTOs;

namespace rabbitmqPractice.Services
{
    public interface IRabbitMQService
    {
        Task DirectExchange(MeterReadingDto order);
    }
}
