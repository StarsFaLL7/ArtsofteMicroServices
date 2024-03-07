using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class SkillSetRepository : IStoreSkillSet
{
    private readonly ConcurrentDictionary<Guid, SkillSet> _store = new ();
    
    public async Task SaveAsync(SkillSet skillSet)
    {
        _store[skillSet.Id] = skillSet;
    }

    public async Task RemoveAsync(Guid skillSetId)
    {
        _store.TryRemove(skillSetId, out _);
    }

    public async Task<SkillSet> GetByIdAsync(Guid id)
    {
        return _store[id];
    }
}