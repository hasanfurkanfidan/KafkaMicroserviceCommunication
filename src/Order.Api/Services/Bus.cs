using Confluent.Kafka;
using Shared.Events;

namespace Order.Api.Services
{
    public class Bus : IBus
    {
        private readonly ProducerConfig _producerConfig;
        public Bus(IConfiguration configuration)
        {
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = configuration.GetSection("BusSettings").GetSection("Kafka")["BootstrapServers"],
                Acks = Acks.All,
                MessageTimeoutMs = 6000
            };

        }

        public async Task<bool> Publish<T1, T2>(T1 key, T2 value, string topicOrQueueName)
        {
            using var producer = new ProducerBuilder<T1, T2>(_producerConfig)
                .SetKeySerializer(new CustomKeySerializer<T1>())
                .SetValueSerializer(new CustomValueSerializer<T2>())
                .Build();

            var message = new Message<T1, T2>
            {
                Key = key,
                Value = value
            };

            var result = await producer.ProduceAsync(topicOrQueueName, message);

            return result.Status == PersistenceStatus.Persisted;
        }
    }
}
