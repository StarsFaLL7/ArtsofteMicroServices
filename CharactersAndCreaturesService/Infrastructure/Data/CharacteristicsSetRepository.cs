using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CharacteristicsSetRepository : IStoreCharacteristicsSet
{
    private readonly PostgresDbContext _dbContext;

    public CharacteristicsSetRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CharacteristicsSet characteristicsSet)
    {
        if (!_dbContext.CharacteristicsSets.Contains(characteristicsSet))
        {
            _dbContext.CharacteristicsSets.Add(characteristicsSet);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid characteristicsSetId)
    {
        var set = _dbContext.CharacteristicsSets
            .FirstOrDefault(creature => creature.Id == characteristicsSetId);
        if (set == null)
        {
            return;
        }
        _dbContext.CharacteristicsSets.Remove(set);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CharacteristicsSet> GetByIdAsync(Guid id)
    {
        return _dbContext.CharacteristicsSets.First(set => set.Id == id);
    }

    public async Task<bool> IsExist(Guid id)
    {
        var set = _dbContext.CharacteristicsSets.FirstOrDefault(set => set.Id == id);
        return set != null;
    }
}