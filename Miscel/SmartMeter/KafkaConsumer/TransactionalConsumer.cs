using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer
{
    public class TransactionalConsumer
    {
        public static void StartConsuming()
        {
            Console.WriteLine("=== Transactional Consumer ===");
            Console.WriteLine("This consumer only sees committed transactions");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "transactional-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                // Important: Only read committed messages
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("orders-topic");

            Console.WriteLine("Transactional Consumer started (only sees committed messages). Press Ctrl+C to exit.");

            var receivedOrders = new List<Order>();

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

                    receivedOrders.Add(order);

                    Console.WriteLine($"📦 Received COMMITTED order: ID {order.OrderId}, Product: {order.ProductName}, Price: ${order.Price}");
                    Console.WriteLine($"   Total committed orders received: {receivedOrders.Count}");

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
                Console.WriteLine($"\n📊 FINAL: Received {receivedOrders.Count} committed orders");
                foreach (var order in receivedOrders)
                {
                    Console.WriteLine($"   - Order {order.OrderId}: {order.ProductName}");
                }
            }
        }
    }
}