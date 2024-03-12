using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class AbilityService : IAbilityService
{
    private readonly IStoreAbility _storeAbility;

    public AbilityService(IStoreAbility storeAbility)
    {
        _storeAbility = storeAbility;
    }

    public async Task<Guid> CreateOrUpdateAsync(Ability ability)
    {
        await _storeAbility.SaveAsync(ability);
        return ability.Id;
    }

    public async Task<Ability> GetInfoAsync(Guid id)
    {
        return await _storeAbility.GetByIdAsync(id);
    }

    public async Task DeleteAsync(Guid abilityId)
    {
        var ability = await _storeAbility.GetByIdAsync(abilityId);
        await ability.DeleteAsync(_storeAbility);
    }

    public async Task<Ability[]> GetAbilitiesByCharacter(Guid characterId)
    {
        return await _storeAbility.GetByCharacterIdAsync(characterId);
    }
}