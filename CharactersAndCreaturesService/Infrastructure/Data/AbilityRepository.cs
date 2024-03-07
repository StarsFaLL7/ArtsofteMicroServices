using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class AbilityRepository : IStoreAbility
{
    private readonly ConcurrentDictionary<Guid, Ability> _store = new();
    
    public async Task SaveAsync(Ability ability)
    {
        _store[ability.Id] = ability;
    }
    
    public async Task RemoveAsync(Ability ability)
    {
        _store.TryRemove(ability.Id, out _);
    }

    public async Task<Ability> GetByIdAsync(Guid id)
    {
        return _store[id];
    }

    public async Task<Ability[]> GetByCharacterIdAsync(Guid characterId)
    {
        return _store.Values.Where(a => a.CharacterId == characterId).ToArray();
    }

    public async Task RemoveByCharacterIdAsync(Guid characterId)
    {
        var listToDelete = await GetByCharacterIdAsync(characterId);
        foreach (var ability in listToDelete)
        {
            _store.TryRemove(ability.Id, out _);
        }
    }
}