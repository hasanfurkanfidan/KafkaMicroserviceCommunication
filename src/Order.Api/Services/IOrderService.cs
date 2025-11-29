using Order.Api.Dtos;

namespace Order.Api.Services
{
    public interface IOrderService
    {
        Task<bool> Create(OrderCreateRequestDto orderCreateRequestDto);
    }
}
