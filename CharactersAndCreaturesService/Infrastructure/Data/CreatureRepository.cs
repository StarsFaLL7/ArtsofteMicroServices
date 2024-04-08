using System.Collections.Concurrent;
using Domain.AggregateModels;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CreatureRepository : IStoreCreature
{
    private readonly PostgresDbContext _dbContext;

    public CreatureRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsExistAsync(Guid creatureId)
    {
        var creature = _dbContext.Creatures.FirstOrDefault(c => c.Id == creatureId);
        return creature != null;
    }

    public async Task SaveAsync(Creature creature)
    {
        if (!_dbContext.Creatures.Contains(creature))
        {
            _dbContext.Creatures.Add(creature);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Creature[]> GetPlayersCreaturesAsync(Guid userId)
    {
        var res = _dbContext.Creatures.Where(c => c.UserId == userId).ToArray();
        return res;
    }

    public async Task RemoveAsync(Guid creatureId)
    {
        var creature = _dbContext.Creatures.FirstOrDefault(creature => creature.Id == creatureId);
        if (creature == null)
        {
            return;
        }
        _dbContext.Creatures.Remove(creature);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Creature> GetByIdAsync(Guid id)
    {
        return _dbContext.Creatures.First(c => c.Id == id);
    }

    public async Task<Creature[]> GetCreaturesBySearchAsync(string searchText, int maxCount)
    {
        var lowerSearch = searchText.ToLower();
        return _dbContext.Creatures.Where(c => c.Name.ToLower().Contains(lowerSearch)).ToArray();
    }
}