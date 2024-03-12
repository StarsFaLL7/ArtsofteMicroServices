using Domain.Entities;

namespace Application.Interfaces;

public interface IAbilityService
{
    Task<Guid> CreateOrUpdateAsync(Ability ability);

    Task<Ability> GetInfoAsync(Guid id);

    Task DeleteAsync(Guid abilityId);

    Task<Ability[]> GetAbilitiesByCharacter(Guid characterId);
}