using Confluent.Kafka;

namespace Stock.Api.Services
{
    public interface IBus
    {
        ConsumerConfig GetConsumerConfig(string groupId);
    }
}
