using Microsoft.AspNetCore.Connections;
//using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
//using SmartMeter.Services.RabbitMqService.Model;
//using SmartMeter.Services.RabbitMQMeterProcessingService.Utils;
using System.Text;
using System.Text.Json;
//using SmartMeter.Services.RabbitMqService.Model;
using SmartMeter.Services.RabbitMqService.Utils;
using SmartMeter.Models;

namespace SmartMeter.Services.RabbitMqService
{
    public class RabbitMqProcessService
    { 
        public class RabbitMqConsumerService : BackgroundService
    
        {
            private readonly ILogger<RabbitMqConsumerService> _logger;
            private readonly IServiceProvider _serviceProvider;
            private IConnection? _connection; 
            private IChannel? _channel;
            private const string ExchangeName = "logs";
            private const string QueueName = "meter_readings_queue";

            public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, IServiceProvider serviceProvider)
            {
                _logger = logger;
                _serviceProvider = serviceProvider;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                try
                {
                    var factory = new ConnectionFactory { HostName = "localhost" };
                    _connection = await factory.CreateConnectionAsync();
                    _channel = await _connection.CreateChannelAsync();

                    await _channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Fanout);
                    await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false);
                    await _channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: string.Empty);
                    await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new AsyncEventingBasicConsumer(_channel);

                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var messageJson = Encoding.UTF8.GetString(body);

                        try
                        {
                            var data = JsonSerializer.Deserialize<Meterreading>(messageJson);
                            _logger.LogInformation("Received -> Meter: {MeterId}, Energy: {Energy}",
                                data?.Meterid, data?.Energyconsumed);

                            // Use DI scope to resolve DatabaseService
                            using var scope = _serviceProvider.CreateScope();
                            var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();

                            Console.WriteLine("TESTING");

                            await dbService.InsertMeterReadingAsync(data!);
                            _logger.LogInformation("✅ Inserted into database.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "[!] Error processing message.");
                        }
                    };

                    await _channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer);

                    // Keep running until the app stops
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RabbitMQ consumer failed to start.");
                }
            }

            public override async Task StopAsync(CancellationToken cancellationToken)
            {
                _logger.LogInformation("Stopping RabbitMQ consumer...");
                if (_channel != null)
                    await _channel.CloseAsync();

                if (_connection != null)
                    await _connection.CloseAsync();

                await base.StopAsync(cancellationToken);
            }

         
        }
    }
}
