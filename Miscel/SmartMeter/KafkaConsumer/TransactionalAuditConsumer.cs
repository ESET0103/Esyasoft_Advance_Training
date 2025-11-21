using Confluent.Kafka;
using System.Text.Json;

namespace KafkaConsumer
{
    public class TransactionalAuditConsumer
    {
        public static void StartAuditConsuming()
        {
            Console.WriteLine("=== Transactional Audit Consumer ===");
            Console.WriteLine("This consumer monitors both orders and audit topics");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "audit-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using var ordersConsumer = new ConsumerBuilder<Ignore, string>(config).Build();
            using var auditConsumer = new ConsumerBuilder<Ignore, string>(config).Build();

            ordersConsumer.Subscribe("orders-topic");
            auditConsumer.Subscribe("audit-topic");

            Console.WriteLine("Audit Consumer started (monitoring both topics). Press Ctrl+C to exit.");

            var ordersReceived = new List<string>();
            var auditReceived = new List<string>();

            try
            {
                while (true)
                {
                    // Consume from orders topic
                    var ordersResult = ordersConsumer.Consume(TimeSpan.FromMilliseconds(100));
                    if (ordersResult != null)
                    {
                        var order = JsonSerializer.Deserialize<Order>(ordersResult.Message.Value);
                        ordersReceived.Add($"Order {order.OrderId}: {order.ProductName}");
                        Console.WriteLine($"📦 ORDERS: {order.OrderId} - {order.ProductName}");
                        ordersConsumer.Commit(ordersResult);
                    }

                    // Consume from audit topic
                    var auditResult = auditConsumer.Consume(TimeSpan.FromMilliseconds(100));
                    if (auditResult != null)
                    {
                        auditReceived.Add(auditResult.Message.Value);
                        Console.WriteLine($"📋 AUDIT: {auditResult.Message.Value}");
                        auditConsumer.Commit(auditResult);
                    }

                    if (ordersReceived.Count > 0 || auditReceived.Count > 0)
                    {
                        Console.WriteLine($"📊 Stats: {ordersReceived.Count} orders, {auditReceived.Count} audit records");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Audit Consumer stopped.");
            }
            finally
            {
                ordersConsumer.Close();
                auditConsumer.Close();
            }
        }
    }
}