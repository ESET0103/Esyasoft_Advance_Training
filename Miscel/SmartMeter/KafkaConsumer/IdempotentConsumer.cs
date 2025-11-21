using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer
{
    public class IdempotentConsumer
    {
        public static void StartConsuming()
        {
            Console.WriteLine("=== Idempotent Test Consumer ===");
            Console.WriteLine("This consumer tracks messages to show idempotence effect");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "idempotent-test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("idempotent-topic");

            Console.WriteLine("Idempotent Test Consumer started. Press Ctrl+C to exit.");

            var messageCounts = new Dictionary<int, int>(); // OrderId -> Count
            var totalMessages = 0;

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

                    totalMessages++;

                    // Track how many times we've seen each OrderId
                    if (messageCounts.ContainsKey(order.OrderId))
                    {
                        messageCounts[order.OrderId]++;
                    }
                    else
                    {
                        messageCounts[order.OrderId] = 1;
                    }

                    var count = messageCounts[order.OrderId];

                    if (count > 1)
                    {
                        Console.WriteLine($"🚨 DUPLICATE! OrderId {order.OrderId} seen {count} times - Product: {order.ProductName}");
                    }
                    else
                    {
                        Console.WriteLine($"✅ First time - OrderId {order.OrderId}, Product: {order.ProductName}");
                    }

                    Console.WriteLine($"📊 Stats: {messageCounts.Count} unique orders, {totalMessages} total messages received");

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

                // Final statistics
                var duplicates = messageCounts.Values.Count(count => count > 1);
                Console.WriteLine($"\n📊 FINAL STATISTICS:");
                Console.WriteLine($"Total unique orders: {messageCounts.Count}");
                Console.WriteLine($"Total messages received: {totalMessages}");
                Console.WriteLine($"Orders with duplicates: {duplicates}");

                if (duplicates > 0)
                {
                    Console.WriteLine($"Duplicate orders:");
                    foreach (var kvp in messageCounts.Where(x => x.Value > 1))
                    {
                        Console.WriteLine($"  OrderId {kvp.Key}: {kvp.Value} times");
                    }
                }
            }
        }
    }
}