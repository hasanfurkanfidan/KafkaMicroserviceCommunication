using Order.Api.Dtos;
using Shared.Events;

namespace Order.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBus _bus;
        public OrderService(IBus bus)
        {
            _bus = bus;
        }
        public async Task<bool> Create(OrderCreateRequestDto orderCreateRequestDto)
        {
            string orderCode = Guid.NewGuid().ToString();
            var orderCreatedEvent = new OrderCreatedEvent(orderCode, "10", 1000);

            var result = await _bus.Publish(orderCode, orderCreatedEvent, BusConsts.OrderCreatedEventTopicName);

            return result;
        }
    }
}
