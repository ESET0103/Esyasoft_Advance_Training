using Confluent.Kafka;
using KafkaProducer.Config;
using System.Text.Json;

namespace KafkaProducer
{
    public class AtMostOnceProducer
    {
        public static async Task ProduceMessages()
        {
            Console.WriteLine("=== At-Most-Once Producer ===");
            Console.WriteLine("This producer may lose messages but is fastest");

            var config = DeliverySemanticsConfig.CreateAtMostOnceConfig();
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    var order = new Order { OrderId = i, ProductName = $"AtMostOnce-Product-{i}", Price = 50 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    producer.Produce("delivery-semantics-topic", new Message<Null, string> { Value = jsonOrder },
                        deliveryReport =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Message {i} failed: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Message {i} sent (no guarantee of delivery)");
                            }
                        });

                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("At-Most-Once Producer completed.");
        }
    }
}