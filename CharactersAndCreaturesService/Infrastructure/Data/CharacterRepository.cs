using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class CharacterRepository : IStoreCharacter
{
    private readonly PostgresDbContext _dbContext;

    public CharacterRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsExistAsync(Guid characterId)
    {
        var character = _dbContext.Characters.FirstOrDefault(c => c.Id == characterId);
        return character != null;
    }

    public async Task SaveAsync(Character character)
    {
        if (!_dbContext.Characters.Contains(character))
        {
            _dbContext.Characters.Add(character);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Character[]> GetPlayersCharactersAsync(Guid userId)
    {
        return _dbContext.Characters.Where(c => c.UserId == userId).ToArray();
    }

    public async Task RemoveAsync(Guid characterId)
    {
        var character = _dbContext.Characters.FirstOrDefault(creature => creature.Id == characterId);
        if (character == null)
        {
            return;
        }
        _dbContext.Characters.Remove(character);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Character> GetByIdAsync(Guid id)
    {
        return _dbContext.Characters.First(c => c.Id == id);
    }

    public async Task<Character[]> GetByIdsAsync(params Guid[] ids)
    {
        return _dbContext.Characters.Where(c => ids.Contains(c.Id)).ToArray();
    }

    public async Task<InventoryItem[]> AddItemAsync(Guid characterId, InventoryItem item)
    {
        var character = _dbContext.Characters
            .Include(character => character.InventoryItems)
            .First(c => c.Id == characterId);
        character.InventoryItems.ToList().Add(item);
        await _dbContext.SaveChangesAsync();
        return character.InventoryItems.ToArray();
    }
}