using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class InventoryItemRepository : IStoreInventoryItem
{
    private readonly PostgresDbContext _dbContext;

    public InventoryItemRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(InventoryItem inventoryItem)
    {
        if (!_dbContext.InventoryItems.Contains(inventoryItem))
        {
            _dbContext.InventoryItems.Add(inventoryItem);
        }

        await _dbContext.SaveChangesAsync();
    }
    

    public async Task RemoveAsync(Guid inventoryItemId)
    {
        var item = _dbContext.InventoryItems.FirstOrDefault(item => item.Id == inventoryItemId);
        if (item == null)
        {
            return;
        }
        _dbContext.InventoryItems.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<InventoryItem> GetByIdAsync(Guid id)
    {
        return _dbContext.InventoryItems.First(item => item.Id == id);
    }

    public async Task<InventoryItem[]> GetAllByCharacterId(Guid characterId)
    {
        return _dbContext.InventoryItems.Where(item => item.CharacterId == characterId).ToArray();
    }

    public async Task RemoveByCharacterId(Guid characterId)
    {
        var listToDelete = await GetAllByCharacterId(characterId);
        foreach (var item in listToDelete)
        {
            _dbContext.Remove(item);
        }

        await _dbContext.SaveChangesAsync();
    }
}