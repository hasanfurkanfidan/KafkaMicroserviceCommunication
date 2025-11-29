namespace Order.Api.Dtos
{
    public record OrderCreateRequestDto(string UserId, decimal TotalPrice);
}
