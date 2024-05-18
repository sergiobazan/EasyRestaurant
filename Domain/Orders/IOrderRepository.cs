namespace Domain.Orders;

public interface IOrderRepository
{
    void Add(Order order);
    Task<Order?> GetAsync(Guid id);
}
