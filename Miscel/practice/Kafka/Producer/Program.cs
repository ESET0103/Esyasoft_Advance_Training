//using Confluent.Kafka;
//using Producer.DTOs;
//using System.Text.Json;

////var config = new ProducerConfig
////{
////    // Replace "localhost:9092" with your actual broker addresses.
////    BootstrapServers = "localhost:9092"
////};

////using var producer = new ProducerBuilder<Null, string>(config).Build();
////using var producer = new ProducerBuilder<Null, string>(config).Build();

//// consumer group

//var config = new ProducerConfig
//{
//    BootstrapServers = "localhost:9092",
//    // This single setting enables idempotence.
//    // It automatically sets Acks=All and other necessary configs.
//    EnableIdempotence = true
//};

//using var producer = new ProducerBuilder<Null, string>(config).Build();


//var order = new OrderDto { OrderId = 124, ProductName = "Mobile", Price = 1200.50m };
//string jsonOrder = JsonSerializer.Serialize(order);








//try
//{
//    var deliveryReport = await producer.ProduceAsync("my-topic", new Message<Null, string>
//    {
//        Value = jsonOrder,
//    });

//    if (deliveryReport.Status != PersistenceStatus.Persisted)
//    {
//        Console.WriteLine($"Message delivery failed: {deliveryReport.Status}");
//    }
//    else
//    {
//        Console.WriteLine($"Message delivered to {deliveryReport.TopicPartitionOffset}");
//    }
//}
//catch (ProduceException<Null, string> e)
//{
//    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
//}

//// Ensure all buffered messages are sent.
//producer.Flush(TimeSpan.FromSeconds(10));


using Confluent.Kafka;

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
    EnableAutoCommit = false, // Must be false for transactional workflow
    IsolationLevel = IsolationLevel.ReadCommitted // Ensures consumer reads only committed messages
};

using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
using var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

// Initialize transaction
producer.InitTransactions(TimeSpan.FromSeconds(10));

consumer.Subscribe("my-topic");

try
{
    producer.BeginTransaction();

    var consumeResult = consumer.Consume();

    // Produce new message
    producer.Produce("my-output-topic", new Message<Null, string> { Value = consumeResult.Message.Value });

    // Send offsets to transaction
    producer.SendOffsetsToTransaction(
        new[] { new TopicPartitionOffset(consumeResult.TopicPartition, consumeResult.Offset + 1) },
        consumer.ConsumerGroupMetadata, TimeSpan.FromSeconds(10)
    );

    producer.CommitTransaction();
}
catch (KafkaException ex)
{
    Console.WriteLine($"Transaction failed: {ex.Error.Reason}");
    producer.AbortTransaction();
}
