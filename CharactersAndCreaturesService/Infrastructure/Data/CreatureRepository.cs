using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CreatureRepository : IStoreCreature
{
    private readonly ConcurrentDictionary<Guid, Creature> _store = new();

    public async Task<bool> IsExistAsync(Guid creatureId)
    {
        return _store.ContainsKey(creatureId);
    }

    public async Task SaveAsync(Creature creature)
    {
        _store[creature.Id] = creature;
    }

    public async Task<Creature[]> GetPlayersCreaturesAsync(Guid userId)
    {
        var res = _store.Values.Where(c => c.UserId == userId).ToArray();
        return res;
    }

    public async Task RemoveAsync(Guid creatureId)
    {
        if (!_store.TryRemove(creatureId, out _))
        {
            throw new Exception("Существо не найдено");
        }
    }

    public async Task<Creature> GetByIdAsync(Guid id)
    {
        return _store[id];
    }
}