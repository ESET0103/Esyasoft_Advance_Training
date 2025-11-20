using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer
{
    public class IdempotentProducer
    {
        public static async Task ProduceMessages()
        {
            Console.WriteLine("=== Idempotent Producer ===");
            Console.WriteLine("This producer prevents duplicates even with retries");

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                // Enable idempotence - this automatically sets:
                // Acks = Acks.All, Retries = int.MaxValue, MaxInFlight = 5
                EnableIdempotence = true,
                // Optional: You can still set these explicitly
                MessageSendMaxRetries = 10,
                RetryBackoffMs = 300
            };

            using var producer = new ProducerBuilder<Null, string>(config)
                .SetErrorHandler((_, error) =>
                    Console.WriteLine($"Producer Error: {error.Reason}"))
                .Build();

            try
            {
                // First batch of messages
                for (int i = 1; i <= 3; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"Idempotent-Product-{i}", Price = 200 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    var deliveryReport = await producer.ProduceAsync("idempotent-topic",
                        new Message<Null, string> { Value = jsonOrder });

                    Console.WriteLine($"Message {i}: Partition {deliveryReport.Partition}, Offset {deliveryReport.Offset}");
                    await Task.Delay(1000);
                }

                Console.WriteLine("\n--- Simulating network issues and retries ---\n");

                // Simulate a scenario where retries might happen
                // In real scenario, this would happen automatically due to network issues
                for (int i = 4; i <= 5; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"Idempotent-Retry-Product-{i}", Price = 300 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    var deliveryReport = await producer.ProduceAsync("idempotent-topic",
                        new Message<Null, string> { Value = jsonOrder });

                    Console.WriteLine($"Message {i}: Partition {deliveryReport.Partition}, Offset {deliveryReport.Offset}");

                    // Simulate potential duplicate send (in real scenario, this would be automatic retry)
                    Console.WriteLine($"Simulating retry for message {i}...");

                    // Even if we try to send same message again, idempotence prevents duplicates
                    try
                    {
                        var duplicateReport = await producer.ProduceAsync("idempotent-topic",
                            new Message<Null, string> { Value = jsonOrder });
                        Console.WriteLine($"Duplicate send for {i}: Partition {duplicateReport.Partition}, Offset {duplicateReport.Offset}");
                    }
                    catch (ProduceException<Null, string> e)
                    {
                        Console.WriteLine($"Duplicate prevented: {e.Error.Reason}");
                    }

                    await Task.Delay(1000);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("Idempotent Producer completed.");
        }
    }
}
