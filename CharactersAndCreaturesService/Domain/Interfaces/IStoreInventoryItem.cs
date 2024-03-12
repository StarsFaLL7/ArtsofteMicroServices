using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreInventoryItem
{
    Task SaveAsync(InventoryItem inventoryItem);

    Task RemoveAsync(Guid inventoryItemId);

    Task<InventoryItem> GetByIdAsync(Guid id);

    Task<InventoryItem[]> GetAllByCharacterId(Guid characterId);
    
    Task RemoveByCharacterId(Guid characterId);
}