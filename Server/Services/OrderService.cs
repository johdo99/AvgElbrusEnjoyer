using Server.Data.Repositories;
using Server.Models;

namespace Server.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IComponentRepository _componentRepository;

    public OrderService(IOrderRepository orderRepository, IComponentRepository componentRepository)
    {
        _orderRepository = orderRepository;
        _componentRepository = componentRepository;
    }

    public async Task<int> CreateOrderFromComponentsAsync(int userId, IEnumerable<int> componentIds)
    {
        var components = new List<Component>();
        foreach (var id in componentIds)
        {
            var component = await _componentRepository.GetByIdAsync(id);
            if (component == null)
                throw new InvalidOperationException($"Компонент с ID {id} не найден.");
            components.Add(component);
        }

        var newOrder = new Order
        {
            UserId = userId,
            TotalPrice = components.Sum(c => c.Price),
            Status = "Pending",
            OrderDate = DateTime.UtcNow
        };

        return await _orderRepository.CreateOrderAsync(newOrder, components);
    }
}