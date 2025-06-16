using Server.Models;

namespace Server.Services;

public interface IOrderService
{
    Task<int> CreateOrderFromComponentsAsync(int userId, IEnumerable<int> componentIds);
    Task<IEnumerable<Order>> GetOrdersForUserAsync(int userId);
}