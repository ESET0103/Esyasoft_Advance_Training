using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer
{
    public class NonIdempotentProducer
    {
        public static async Task ProduceMessages()
        {
            Console.WriteLine("=== Non-Idempotent Producer ===");
            Console.WriteLine("This producer may create duplicates with retries");

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                // No idempotence
                EnableIdempotence = false,
                // But with retries enabled (like At-Least-Once)
                Acks = Acks.Leader,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 300
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"NonIdempotent-Product-{i}", Price = 150 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    var deliveryReport = await producer.ProduceAsync("idempotent-topic",
                        new Message<Null, string> { Value = jsonOrder });

                    Console.WriteLine($"Message {i}: Partition {deliveryReport.Partition}, Offset {deliveryReport.Offset}");

                    // Simulate scenario where duplicate might occur
                    if (i == 3)
                    {
                        Console.WriteLine("Simulating network issue and automatic retry for message 3...");
                        // In real scenario, Kafka would automatically retry and might create duplicate
                    }

                    await Task.Delay(1000);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("Non-Idempotent Producer completed.");
        }
    }
}