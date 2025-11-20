using Confluent.Kafka;
using System.Text.Json;

namespace KafkaProducer
{
    public class TransactionalProducer
    {
        public static async Task ProduceWithTransaction()
        {
            Console.WriteLine("=== Transactional Producer ===");
            Console.WriteLine("This demonstrates atomic operations across multiple messages");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                // Required for transactional producer
                TransactionalId = "my-transactional-producer-1",
                // Enable idempotence (automatically set when TransactionalId is set)
                EnableIdempotence = true,
                Acks = Acks.All,
                MessageSendMaxRetries = int.MaxValue
            };

            using var producer = new ProducerBuilder<Null, string>(producerConfig)
                .SetErrorHandler((_, error) =>
                    Console.WriteLine($"Producer Error: {error.Reason}"))
                .Build();

            try
            {
                // Step 1: Initialize transactions
                Console.WriteLine("Initializing transactions...");
                producer.InitTransactions(TimeSpan.FromSeconds(10));
                Console.WriteLine("Transactions initialized successfully");

                for (int batch = 1; batch <= 3; batch++)
                {
                    Console.WriteLine($"\n--- Starting Transaction Batch {batch} ---");

                    try
                    {
                        // Step 2: Begin transaction
                        producer.BeginTransaction();

                        // Step 3: Produce multiple messages within transaction
                        for (int i = 1; i <= 3; i++)
                        {
                            var order = new Order
                            {
                                OrderId = (batch * 10) + i,
                                ProductName = $"Tx-Product-B{batch}-{i}",
                                Price = (batch * 100) + (i * 50)
                            };

                            string jsonOrder = JsonSerializer.Serialize(order);

                            // Produce to main topic
                            producer.Produce("orders-topic",
                                new Message<Null, string> { Value = jsonOrder },
                                (deliveryReport) =>
                                {
                                    if (deliveryReport.Error.Code == ErrorCode.NoError)
                                    {
                                        Console.WriteLine($"  Produced to orders-topic: Order {order.OrderId}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"  Failed to produce: {deliveryReport.Error.Reason}");
                                    }
                                });

                            // Also produce to audit topic (same transaction)
                            producer.Produce("audit-topic",
                                new Message<Null, string> { Value = $"AUDIT: {jsonOrder}" },
                                (deliveryReport) =>
                                {
                                    if (deliveryReport.Error.Code == ErrorCode.NoError)
                                    {
                                        Console.WriteLine($"  Produced to audit-topic: Order {order.OrderId}");
                                    }
                                });
                        }

                        // Step 4: Commit transaction (all or nothing)
                        producer.CommitTransaction();
                        
                        Console.WriteLine($"✅ Transaction Batch {batch} COMMITTED successfully");
                    }
                    catch (KafkaException ex)
                    {
                        // Step 5: Abort transaction on failure
                        producer.AbortTransaction();
                        Console.WriteLine($"❌ Transaction Batch {batch} ABORTED: {ex.Message}");
                    }

                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }

            producer.Flush(TimeSpan.FromSeconds(5));
            Console.WriteLine("Transactional Producer completed.");
        }
    }
}