using Confluent.Kafka;

namespace Stock.Api.Services
{
    public class Bus : IBus
    {
        private readonly IConfiguration _configuration;
        public Bus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ConsumerConfig GetConsumerConfig(string groupId)
        {
            return new ConsumerConfig()
            {
                BootstrapServers = _configuration.GetSection("BusSettings").GetSection("Kafka")["BootstrapServers"],
                GroupId = groupId,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
    }
}
