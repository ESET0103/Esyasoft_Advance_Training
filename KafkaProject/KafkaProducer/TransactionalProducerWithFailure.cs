using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer
{
    public class TransactionalProducerWithFailure
    {
        public static async Task ProduceWithSimulatedFailure()
        {
            Console.WriteLine("=== Transactional Producer with Simulated Failure ===");
            Console.WriteLine("This shows how transactions handle failures (rollback)");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                TransactionalId = "my-transactional-producer-2",
                EnableIdempotence = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            try
            {
                producer.InitTransactions(TimeSpan.FromSeconds(10));
                Console.WriteLine("Transactions initialized");

                // Successful transaction
                Console.WriteLine("\n--- Transaction 1: Successful ---");
                producer.BeginTransaction();

                for (int i = 1; i <= 2; i++)
                {
                    var order = new Order { OrderId = 100 + i, ProductName = $"Success-Product-{i}", Price = 100 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    producer.Produce("orders-topic", new Message<Null, string> { Value = jsonOrder });
                    Console.WriteLine($"  Produced: Order {order.OrderId}");
                }

                producer.CommitTransaction();
                Console.WriteLine("✅ Transaction 1 COMMITTED");

                await Task.Delay(1000);

                // Failed transaction (will be rolled back)
                Console.WriteLine("\n--- Transaction 2: Will Fail ---");
                producer.BeginTransaction();

                for (int i = 1; i <= 3; i++)
                {
                    var order = new Order { OrderId = 200 + i, ProductName = $"Fail-Product-{i}", Price = 200 * i };
                    string jsonOrder = JsonSerializer.Serialize(order);

                    producer.Produce("orders-topic", new Message<Null, string> { Value = jsonOrder });
                    Console.WriteLine($"  Produced: Order {order.OrderId}");

                    // Simulate failure after 2nd message
                    if (i == 2)
                    {
                        Console.WriteLine("💥 Simulating business logic failure...");
                        throw new InvalidOperationException("Business rule violation: Inventory out of stock");
                    }
                }

                producer.CommitTransaction(); // This won't be reached
            }
            catch (Exception ex) when (ex is InvalidOperationException or KafkaException)
            {
                Console.WriteLine($"❌ Transaction ABORTED due to: {ex.Message}");
                // Transaction automatically aborted when exception is thrown
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("Transactional Producer with Failure completed.");
        }
    }
}