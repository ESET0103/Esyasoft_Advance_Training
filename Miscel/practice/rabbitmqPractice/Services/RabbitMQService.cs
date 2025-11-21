using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using rabbitmqPractice.DTOs;
using System.Threading.Channels;

namespace rabbitmqPractice.Services
{
    public class RabbitMQService: IRabbitMQService
    {

        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        //private readonly IModel _channel;
        private readonly ILogger<RabbitMQService> _logger;
        public RabbitMQService(ILogger<RabbitMQService> logger)
        {
            _logger = logger;

            _factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "my_host", UserName = "guest", Password = "guest" };
            _connection = _factory.CreateConnectionAsync();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "meter_exchange", type: ExchangeType.Direct, durable: true);
        }

        public async Task DirectExchange(MeterReadingDto meter)
        {
            const string exchangename = "meter_reading";
            string routingkey = $"meter.{meter.RoutingKey}";

        }
    }
}
