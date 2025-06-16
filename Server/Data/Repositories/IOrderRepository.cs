using Server.Models;

namespace Server.Data.Repositories;

public interface IOrderRepository
{
    Task<int> CreateOrderAsync(Order newOrder, IEnumerable<Component> components);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
}