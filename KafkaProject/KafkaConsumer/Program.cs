////namespace KafkaConsumer
////{
////    internal class Program
////    {
////        static void Main(string[] args)
////        {
////            Console.WriteLine("Hello, World!");
////        }
////    }
////}


//using Confluent.Kafka;
//using KafkaConsumer;
//using System.Text.Json;

//var config = new ConsumerConfig
//{
//    BootstrapServers = "localhost:9092",
//    GroupId = "my-consumer-group1",
//    AutoOffsetReset = AutoOffsetReset.Earliest
//};

//using var consumer = new ConsumerBuilder<Null, string>(config).Build();
//consumer.Subscribe("my-third-topic");

//Console.WriteLine("Consumer started. Press Ctrl+C to exit.");

//try
//{

//    while (true)
//    {
//        var consumeResult = consumer.Consume();
//        string jsonOrder = consumeResult.Message.Value;

//        var order = JsonSerializer.Deserialize<Order>(jsonOrder);

//        Console.WriteLine($"Received order for product: {order.ProductName}, ID: {order.OrderId}");
//        Thread.Sleep(100);
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




//using KafkaConsumer;

//Console.WriteLine("Starting Delivery Semantics Consumer...");
//DeliverySemanticsConsumer.StartConsuming();

using Confluent.Kafka;
using KafkaConsumer;
using System.Text.Json;

Console.WriteLine("=== Kafka Consumer ===");
Console.WriteLine("Choose consumer type:");
Console.WriteLine("1 - Basic Consumer (Your existing code)");
Console.WriteLine("2 - Delivery Semantics Consumer (Tracks duplicates)");
Console.WriteLine("3 - Idempotent Test Consumer (Tracks idempotence)");
Console.WriteLine("4 - Transactional Consumer (Orders only)");
Console.WriteLine("5 - Transactional Audit Consumer (Both topics)");
Console.WriteLine("6 - JsonSchema");


var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        RunBasicConsumer();
        break;
    case "2":
        DeliverySemanticsConsumer.StartConsuming();
        break;
    case "3":
        IdempotentConsumer.StartConsuming();
        break;
    case "4":
        TransactionalConsumer.StartConsuming();
        break;
    case "5":
        TransactionalAuditConsumer.StartAuditConsuming();
        break;
    case "6":
        JsonSchema.ConsumeMessages();
        break;
    default:
        Console.WriteLine("Invalid choice, running Basic Consumer");
        RunBasicConsumer();
        break;
}

// Your existing basic consumer code
static void RunBasicConsumer()
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "my-consumer-group1",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    using var consumer = new ConsumerBuilder<Null, string>(config).Build();
    consumer.Subscribe("my-third-topic");

    Console.WriteLine("Basic Consumer started. Press Ctrl+C to exit.");

    try
    {
        while (true)
        {
            var consumeResult = consumer.Consume();
            string jsonOrder = consumeResult.Message.Value;

            var order = JsonSerializer.Deserialize<Order>(jsonOrder);

            Console.WriteLine($"Received order for product: {order.ProductName}, ID: {order.OrderId}");
            Thread.Sleep(100);
            consumer.Commit(consumeResult);
        }
    }
    catch (OperationCanceledException)
    {
        // Triggered when Ctrl+C is pressed
    }
    finally
    {
        consumer.Close();
    }
}