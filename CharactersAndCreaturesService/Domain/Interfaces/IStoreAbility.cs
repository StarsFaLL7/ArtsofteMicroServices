using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreAbility
{
    Task SaveAsync(Ability ability);

    Task RemoveAsync(Ability ability);

    Task<Ability> GetByIdAsync(Guid id);

    Task<Ability[]> GetByCharacterIdAsync(Guid characterId);

    Task RemoveByCharacterIdAsync(Guid characterId);
}