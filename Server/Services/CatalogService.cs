using Server.Data.Repositories;
using Server.Models;

namespace Server.Services;

public class CatalogService : ICatalogService
{
    private readonly IComponentRepository _componentRepository;

    public CatalogService(IComponentRepository componentRepository)
    {
        _componentRepository = componentRepository;
    }

    public async Task<IEnumerable<Component>> GetAllComponentsAsync()
    {
        return await _componentRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Component>> GetComponentsByFilterAsync(string nameFilter)
    {
        return await _componentRepository.GetByFilterAsync(nameFilter);
    }
}