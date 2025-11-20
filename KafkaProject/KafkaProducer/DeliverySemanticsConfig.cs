using Confluent.Kafka;

namespace KafkaProducer.Config
{
    public static class DeliverySemanticsConfig
    {
        public static ProducerConfig CreateAtMostOnceConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.None,
                MessageSendMaxRetries = 0,
                RequestTimeoutMs = 1000
            };
        }

        public static ProducerConfig CreateAtLeastOnceConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.Leader,
                MessageSendMaxRetries = 3,
                RetryBackoffMs = 100
            };
        }

        public static ProducerConfig CreateExactlyOnceConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                EnableIdempotence = true,
                Acks = Acks.All,
                MessageSendMaxRetries = int.MaxValue,
                MaxInFlight = 5
            };
        }
    }
}