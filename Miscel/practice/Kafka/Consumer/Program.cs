//using Confluent.Kafka;
//using Producer.DTOs;
//using System.Text.Json;

//var config = new ConsumerConfig
//{
//    BootstrapServers = "localhost:9092",
//    GroupId = "my-consumer-group",
//    AutoOffsetReset = AutoOffsetReset.Earliest
//};

//using var consumer = new ConsumerBuilder<Null, string>(config).Build();
//consumer.Subscribe("order-topic");

//Console.WriteLine("Consumer started. Press Ctrl+C to exit.");

//try
//{
//    while (true)
//    {
//        var consumeResult = consumer.Consume();
//        string jsonOrder = consumeResult.Message.Value;

//        var order = JsonSerializer.Deserialize<OrderDto>(jsonOrder);

//        Console.WriteLine($"Received order for product: {order.ProductName}, ID: {order.OrderId}");
//        consumer.Commit(consumeResult);
//    }
//}
//catch (OperationCanceledException)
//{
//    // Triggered when Ctrl+C is pressed
//}
//finally
//{
//    consumer.Close();
//}




using System;
using System.Threading;
using Confluent.Kafka;

class TransactionalConsumer
{
    static void Main()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            TransactionalId = "my-transactional-producer"
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "my-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false, // handled manually through the transaction
            IsolationLevel = IsolationLevel.ReadCommitted // only read committed messages
        };

        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

        // Initialize transactional producer
        producer.InitTransactions(TimeSpan.FromSeconds(10));

        consumer.Subscribe("my-topic");

        Console.WriteLine("Consumer started. Listening for messages...");

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(CancellationToken.None);
                    if (consumeResult == null)
                    {
                        Console.WriteLine("ncvoowivW E");
                        continue;
                    }

                    Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");

                    // Start a new transaction
                    producer.BeginTransaction();

                    // Produce the processed message to another topic
                    producer.Produce("my-output-topic", new Message<Null, string>
                    {
                        Value = $"Processed: {consumeResult.Message.Value}"
                    });

                    // Send consumer offset to the same transaction
                    producer.SendOffsetsToTransaction(
                        new[] { new TopicPartitionOffset(consumeResult.TopicPartition, consumeResult.Offset + 1) },
                        consumer.ConsumerGroupMetadata,
                        TimeSpan.FromSeconds(10)
                    );

                    // Commit the transaction atomically
                    producer.CommitTransaction();
                    Console.WriteLine("Transaction committed successfully.\n");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Consume error: {ex.Error.Reason}");
                }
                catch (KafkaException ex)
                {
                    Console.WriteLine($"Kafka error: {ex.Error.Reason}");
                    producer.AbortTransaction();
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Shutting down consumer...");
            consumer.Close();
        }
    }
}
