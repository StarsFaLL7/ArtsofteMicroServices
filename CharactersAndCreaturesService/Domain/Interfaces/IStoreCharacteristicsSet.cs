using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCharacteristicsSet
{
    Task SaveAsync(CharacteristicsSet characteristicsSet);

    Task RemoveAsync(Guid characteristicsSetId);
    
    Task<CharacteristicsSet> GetByIdAsync(Guid id);
    
    Task<bool> IsExist(Guid id);
}