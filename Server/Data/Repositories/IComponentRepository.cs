using Server.Models;

namespace Server.Data.Repositories;

public interface IComponentRepository
{
    Task<Component?> GetByIdAsync(int id);
    Task<IEnumerable<Component>> GetAllAsync();

    /// <summary>
    /// Получает компоненты по строковому фильтру (поиск по имени)
    /// </summary>
    Task<IEnumerable<Component>> GetByFilterAsync(string nameFilter);

    Task<int> AddAsync(Component component);
    Task<bool> UpdateAsync(Component component);
    Task<bool> DeleteAsync(int id);
}