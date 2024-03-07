using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCharacter
{
    Task<bool> IsExistAsync(Guid characterId);
    
    Task SaveAsync(Character character);

    Task<Character[]> GetPlayersCharactersAsync(Guid userId);

    Task RemoveAsync(Guid characterId);

    Task<Character> GetByIdAsync(Guid id);
    
    Task<InventoryItem[]> AddItemAsync(Guid characterId, InventoryItem item);
}