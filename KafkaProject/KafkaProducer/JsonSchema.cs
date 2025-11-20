using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaProducer;

public class JsonSchema
{
    // string agentTopic = "app-agent";

    public static async Task ProduceUserMessageAsync(string topicName, User user)
    {
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081"
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };

        using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);

        using var producer = new ProducerBuilder<string, User>(producerConfig)
            .SetValueSerializer(new JsonSerializer<User>(
                schemaRegistry,
                new JsonSerializerConfig
                {
                    AutoRegisterSchemas = true,
                    SubjectNameStrategy = SubjectNameStrategy.Topic  // Correct property name
                }))
            .Build();

        var message = new Message<string, User>
        {
            Key = user.Name,
            Value = user
        };

        try
        {
            var result = await producer.ProduceAsync(topicName, message);
            Console.WriteLine($"✅ Produced message to {result.TopicPartitionOffset} with key '{result.Message.Key}'.");

            var schema = await schemaRegistry.GetLatestSchemaAsync("app-agent-value");

            Console.WriteLine($"Schema ID: {schema.Id}");
            Console.WriteLine($"Version: {schema.Version}");
            Console.WriteLine($"Schema: {schema.SchemaString}");
        }
        catch (ProduceException<string, User> ex)
        {
            Console.WriteLine($"❌ Delivery failed: {ex.Error.Reason}");
            throw;
        }
    }
}