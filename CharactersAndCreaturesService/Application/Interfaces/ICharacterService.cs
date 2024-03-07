using Domain.Entities;

namespace Application.Interfaces;

public interface ICharacterService
{
    Task<Guid> CreateOrUpdateAsync(Character character);

    Task<Character> GetAggregatedInfoAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<InventoryItem[]> AddItem(Guid characterId, InventoryItem item);
    
    Task<InventoryItem[]> UpdateItem(Guid characterId, InventoryItem item);

    Task RemoveItem(Guid itemId);

    Task<SkillSet> UpdateSkillSet(Guid characterId, SkillSet skillSet);
    
    Task<CharacteristicsSet> UpdateCharacteristicsSet(Guid characterId, CharacteristicsSet characteristics);
}