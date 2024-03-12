using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class InventoryItemRepository : IStoreInventoryItem
{
    private readonly ConcurrentDictionary<Guid, InventoryItem> _store = new();
    
    public async Task SaveAsync(InventoryItem inventoryItem)
    {
        _store[inventoryItem.Id] = inventoryItem;
    }
    

    public async Task RemoveAsync(Guid inventoryItemId)
    {
        _store.TryRemove(inventoryItemId, out _);
    }

    public async Task<InventoryItem> GetByIdAsync(Guid id)
    {
        return _store[id];
    }

    public async Task<InventoryItem[]> GetAllByCharacterId(Guid characterId)
    {
        return _store.Values.Where(i => i.CharacterId == characterId).ToArray();
    }

    public async Task RemoveByCharacterId(Guid characterId)
    {
        var listToDelete = await GetAllByCharacterId(characterId);
        foreach (var item in listToDelete)
        {
            _store.TryRemove(item.Id, out _);
        }
    }
}