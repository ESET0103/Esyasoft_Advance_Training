using Confluent.Kafka;
using KafkaProducer.Config;
using System.Text.Json;

namespace KafkaProducer
{
    public class ExactlyOnceProducer
    {
        public static async Task ProduceMessages()
        {
            Console.WriteLine("=== Exactly-Once Producer ===");
            Console.WriteLine("This producer guarantees no loss and no duplicates");

            var config = DeliverySemanticsConfig.CreateExactlyOnceConfig();
            using var producer = new ProducerBuilder<Null, string>(config)
                .SetErrorHandler((_, error) =>
                    Console.WriteLine($"Producer Error: {error.Reason}"))
                .Build();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"ExactlyOnce-Product-{i}", Price = 100 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    var deliveryReport = await producer.ProduceAsync("delivery-semantics-topic",
                        new Message<Null, string> { Value = jsonOrder });

                    Console.WriteLine($"Message {i} guaranteed: Partition {deliveryReport.Partition}, Offset {deliveryReport.Offset}, Status: {deliveryReport.Status}");

                    await Task.Delay(500);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("Exactly-Once Producer completed.");
        }
    }
}