using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer
{
    public class DeliverySemanticsConsumer
    {
        public static void StartConsuming()
        {
            Console.WriteLine("=== Delivery Semantics Consumer ===");
            Console.WriteLine("This consumer tracks duplicates across all delivery semantics");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "delivery-semantics-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("delivery-semantics-topic");

            Console.WriteLine("Delivery Semantics Consumer started. Press Ctrl+C to exit.");

            var processedMessages = new HashSet<int>();
            var duplicateCount = 0;

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

                    if (processedMessages.Contains(order.OrderId))
                    {
                        duplicateCount++;
                        Console.WriteLine($"⚠️ DUPLICATE DETECTED: OrderId {order.OrderId}, Product: {order.ProductName}");
                        Console.WriteLine($"📊 Statistics: {processedMessages.Count} unique, {duplicateCount} duplicates");
                    }
                    else
                    {
                        processedMessages.Add(order.OrderId);
                        Console.WriteLine($"✅ Processed: OrderId {order.OrderId}, Product: {order.ProductName}, Price: ${order.Price}");
                    }

                    consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer stopped.");
            }
            finally
            {
                consumer.Close();
                Console.WriteLine($"Final Statistics: {processedMessages.Count} unique messages, {duplicateCount} duplicates");
            }
        }
    }
}
