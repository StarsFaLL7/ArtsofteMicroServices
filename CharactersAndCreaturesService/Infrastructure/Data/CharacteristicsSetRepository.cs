using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CharacteristicsSetRepository : IStoreCharacteristicsSet
{
    private readonly ConcurrentDictionary<Guid, CharacteristicsSet> _store = new();
    
    public async Task SaveAsync(CharacteristicsSet characteristicsSet)
    {
        _store[characteristicsSet.Id] = characteristicsSet;
    }

    public async Task RemoveAsync(Guid characteristicsSetId)
    {
        _store.TryRemove(characteristicsSetId, out _);
    }

    public async Task<CharacteristicsSet> GetByIdAsync(Guid id)
    {
        return _store[id];
    }

    public async Task<bool> IsExist(Guid id)
    {
        return _store.ContainsKey(id);
    }
}