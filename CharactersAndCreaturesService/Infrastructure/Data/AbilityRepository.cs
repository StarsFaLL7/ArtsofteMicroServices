using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class AbilityRepository : IStoreAbility
{
    private readonly PostgresDbContext _dbContext;

    public AbilityRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(Ability ability)
    {
        if (!_dbContext.Abilities.Contains(ability))
        {
            _dbContext.Abilities.Add(ability);
        }

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task RemoveAsync(Ability ability)
    {
        var foundAbility = _dbContext.Abilities
            .FirstOrDefault(creature => creature.Id == ability.Id);
        if (foundAbility == null)
        {
            return;
        }
        _dbContext.Abilities.Remove(foundAbility);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Ability> GetByIdAsync(Guid id)
    {
        return _dbContext.Abilities.First(a => a.Id == id);
    }

    public async Task<Ability[]> GetByCharacterIdAsync(Guid characterId)
    {
        return _dbContext.Abilities.Where(a => a.CharacterId == characterId).ToArray();
    }

    public async Task RemoveByCharacterIdAsync(Guid characterId)
    {
        var listToDelete = await GetByCharacterIdAsync(characterId);
        foreach (var ability in listToDelete)
        {
            _dbContext.Abilities.Remove(ability);
        }

        await _dbContext.SaveChangesAsync();
    }
}