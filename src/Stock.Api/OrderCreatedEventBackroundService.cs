
using Confluent.Kafka;
using Shared.Events;
using Stock.Api.Services;

namespace Stock.Api
{
    public class OrderCreatedEventBackroundService : BackgroundService
    {
        private readonly IBus _bus;
        public OrderCreatedEventBackroundService(IBus bus)
        {
            _bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new ConsumerBuilder<string, OrderCreatedEvent>(_bus.GetConsumerConfig(BusConsts.OrderCreatedEventTopicGroupName))
                .SetValueDeserializer(new CustomValueDeserializer<OrderCreatedEvent>())
                .Build();

            consumer.Subscribe(BusConsts.OrderCreatedEventTopicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult != null)
                {
                    consumer.Commit(consumeResult);
                }
            }

            await Task.Delay(10);
        }
    }
}
