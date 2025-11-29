using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Shared.Events;

namespace Order.Api.Services
{
    public class Bus : IBus
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Bus> _logger;
        public Bus(IConfiguration configuration, ILogger<Bus> logger)
        {
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = configuration.GetSection("BusSettings").GetSection("Kafka")["BootstrapServers"],
                Acks = Acks.All,
                MessageTimeoutMs = 6000
            };

            _configuration = configuration;
            _logger = logger;
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

        public async Task CreateTopicOrQueue(List<string> topicOrQueueNameList)
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig()
            {
                BootstrapServers = _configuration.GetSection("BusSettings").GetSection("Kafka")["BootstrapServers"]
            }).Build();

            try
            {
                foreach (var topicOrQueueName in topicOrQueueNameList)
                {
                    await adminClient.CreateTopicsAsync(new[]
                      {
                    new TopicSpecification()
                    {
                        Name = topicOrQueueName,NumPartitions = 6,ReplicationFactor = 1
                    }
                          });

                    _logger.LogWarning($"topic : {topicOrQueueName} oluştu!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
