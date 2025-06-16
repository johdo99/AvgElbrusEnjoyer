using Server.Models;

namespace Server.Services;

public interface ICatalogService
{
    Task<IEnumerable<Component>> GetAllComponentsAsync();
    Task<IEnumerable<Component>> GetComponentsByFilterAsync(string nameFilter);
}