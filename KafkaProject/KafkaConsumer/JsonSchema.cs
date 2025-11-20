using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaConsumer;

public class JsonSchema
{

    public static async Task ConsumeMessages()
    {
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081"
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "schema-registry-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
        using var consumer = new ConsumerBuilder<string, User>(consumerConfig)
            .SetValueDeserializer(
                new JsonDeserializer<User>(schemaRegistry).AsSyncOverAsync()
            )
            .Build();

        consumer.Subscribe("app-agent");

        Console.WriteLine("Consuming messages from 'app-agent'...");

        try
        {
            while (true)
            {
                var result = consumer.Consume();

                var user = result.Message.Value;

                Console.WriteLine(
                    $"Received: Key={result.Message.Key}, " +
                    $"Name={user.Name}, Age={user.Age}, Email={user.Email}, " +
                    $"Address={user.Address}, CreatedAt={user.CreatedAt}"
                );
                var schema = await schemaRegistry.GetLatestSchemaAsync("app-agent-value");

                Console.WriteLine($"Schema ID: {schema.Id}");
                Console.WriteLine($"Version: {schema.Version}");
                Console.WriteLine($"Schema: {schema.SchemaString}");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}