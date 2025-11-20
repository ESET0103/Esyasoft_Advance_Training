//using Confluent.Kafka;
//using KafkaProducer;
//using System.Text.Json;

//var config = new ProducerConfig
//{
//    // Replace "localhost:9092" with your actual broker addresses.
//    BootstrapServers = "localhost:9092"
//};

//using var producer = new ProducerBuilder<Null, string>(config).Build();
//var order = new Order { OrderId = 124, ProductName = "Desktop", Price = 1300.50m };
//string jsonOrder = JsonSerializer.Serialize(order);

//try
//{
//    var deliveryReport = await producer.ProduceAsync("my-third-topic", new Message<Null, string>
//    {
//        Value = jsonOrder
//    });

//    Console.WriteLine($"Message sent to partition: {deliveryReport.Partition}, offset: {deliveryReport.Offset}");
//}
//catch (ProduceException<Null, string> e)
//{
//    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
//}

//// Ensure all buffered messages are sent.
//producer.Flush(TimeSpan.FromSeconds(10));



using Confluent.Kafka;
using KafkaProducer;
//using Newtonsoft.Json.Schema;
using System.Text.Json;

//Console.WriteLine("=== Kafka Producer ===");
//Console.WriteLine("Choose producer type:");
//Console.WriteLine("1 - Basic Producer (Your existing code)");
//Console.WriteLine("2 - At-Most-Once Producer");
//Console.WriteLine("3 - At-Least-Once Producer");
//Console.WriteLine("4 - Exactly-Once Producer");
//Console.WriteLine("5 - Idempotent Producer");
//Console.WriteLine("6 - Non-Idempotent Producer");
//Console.WriteLine("7 - Transactional Producer");
//Console.WriteLine("8 - Transactional Producer with Failure Simulation");
//Console.WriteLine("9 - JsonSchema");

//User user = new User
//{ PhoneNumber = "4569871235",
//    Name = "Mantu",
//    Age = 23,
//    Address = "Mangalore",
//    Email = "mantu@gmail.com",
//    CreatedAt = DateTime.Now,


//};



//var choice = Console.ReadLine();

//switch (choice)
//{
//    case "1":
//        await RunBasicProducer();
//        break;
//    case "2":
//        await AtMostOnceProducer.ProduceMessages();
//        break;
//    case "3":
//        await AtLeastOnceProducer.ProduceMessages();
//        break;
//    case "4":
//        await ExactlyOnceProducer.ProduceMessages();
//        break;
//    case "5":
//        await IdempotentProducer.ProduceMessages();
//        break;
//    case "6":
//        await NonIdempotentProducer.ProduceMessages();
//        break;
//    case "7":
//        await TransactionalProducer.ProduceWithTransaction();
//        break;
//    case "8":
//        await TransactionalProducerWithFailure.ProduceWithSimulatedFailure();
//        break;
//    case "9":
//        await JsonSchema.ProduceUserMessageAsync("app-agent", user);
//        break;
//    default:
//        Console.WriteLine("Invalid choice, running Basic Producer");
//        await RunBasicProducer();
//        break;
//}

//// Your existing basic producer code
//static async Task RunBasicProducer()
//{
//    var config = new ProducerConfig
//    {
//        BootstrapServers = "localhost:9092"
//    };

//    using var producer = new ProducerBuilder<Null, string>(config).Build();
//    var order = new Order { OrderId = 124, ProductName = "Desktop", Price = 1300.50m };
//    string jsonOrder = JsonSerializer.Serialize(order);

//    try
//    {
//        var deliveryReport = await producer.ProduceAsync("my-third-topic", new Message<Null, string>
//        {
//            Value = jsonOrder
//        });

//        Console.WriteLine($"Message sent to partition: {deliveryReport.Partition}, offset: {deliveryReport.Offset}");
//    }
//    catch (ProduceException<Null, string> e)
//    {
//        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
//    }

//    producer.Flush(TimeSpan.FromSeconds(10));
//    Console.WriteLine("Basic Producer completed.");
//}


using Confluent.Kafka;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;
using System;
using System.Threading.Tasks;
using Streamiz.Kafka.Net.Crosscutting;
using Streamiz.Kafka.Net.State;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new StreamConfig<StringSerDes, StringSerDes>();
        config.ApplicationId = "sensor-data-processor-app";
        config.BootstrapServers = "localhost:9092";
        config.AutoOffsetReset = AutoOffsetReset.Earliest;

        var builder = new StreamBuilder();

        builder
            .Stream<string, string>("sensor-readings-topic")
            // Convert string → double
            .MapValues<double>(value => double.Parse(value))
            // Filter values >= 10
            .Filter((key, value) => value >= 10.0)
            // Group by key
            .GroupByKey(new StringSerDes(), new DoubleSerDes())
            // 1-minute tumbling window
            .WindowedBy(TumblingWindowOptions.Of(TimeSpan.FromMinutes(1)))
            // Count with Materialized store
            .Count(
                Materialized<string, long, IWindowStore<Bytes, byte[]>>
                    .Create("sensor-counts")
            )
            // Convert windowed result back to stream
            .ToStream(
                (windowedKey, count) =>
                    $"{windowedKey.Key}_{windowedKey.Window.Start}_{windowedKey.Window.End}",
                (windowedKey, count) => count.ToString()
            )
            // Send to final topic
            .To("sensor-counts-topic", new StringSerDes(), new StringSerDes());

        var topology = builder.Build();
        var stream = new KafkaStream(topology, config);

        await stream.StartAsync();
    }
}
