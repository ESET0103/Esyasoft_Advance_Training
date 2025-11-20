using Confluent.Kafka;
using KafkaProducer.Config;
using System.Text.Json;

namespace KafkaProducer
{
    public class AtLeastOnceProducer
    {
        public static async Task ProduceMessages()
        {
            Console.WriteLine("=== At-Least-Once Producer ===");
            Console.WriteLine("This producer won't lose messages but may have duplicates");

            var config = DeliverySemanticsConfig.CreateAtLeastOnceConfig();
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"AtLeastOnce-Product-{i}", Price = 75 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    var deliveryReport = await producer.ProduceAsync("delivery-semantics-topic",
                        new Message<Null, string> { Value = jsonOrder });

                    Console.WriteLine($"Message {i} confirmed: Partition {deliveryReport.Partition}, Offset {deliveryReport.Offset}");

                    await Task.Delay(500);
                }
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed after retries: {e.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("At-Least-Once Producer completed.");
        }
    }
}
