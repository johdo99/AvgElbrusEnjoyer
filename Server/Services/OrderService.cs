using System.Diagnostics;
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
        Debug.WriteLine($"[OrderService.Constructor] Сервис создан. _orderRepository Null? {_orderRepository == null}, _componentRepository Null? {_componentRepository == null}");
    }

    public async Task<int> CreateOrderFromComponentsAsync(int userId, IEnumerable<int> componentIds)
    {
        Debug.WriteLine($"[OrderService.CreateOrder] Вход в метод. _componentRepository Null? {_componentRepository == null}");

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

    public async Task<IEnumerable<Order>> GetOrdersForUserAsync(int userId)
    {
        return await _orderRepository.GetByUserIdAsync(userId);
    }
}