using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CreatureService : ICreatureService
{
    private readonly IStoreCreature _storeCreature;
    private readonly IStoreCharacteristicsSet _storeCharacteristicsSet;
    
    public CreatureService(IStoreCreature storeCreature, IStoreCharacteristicsSet storeCharacteristicsSet)
    {
        _storeCreature = storeCreature;
        _storeCharacteristicsSet = storeCharacteristicsSet;
    }
    
    public async Task<Guid> CreateOrUpdateAsync(Creature creature)
    {
        if (await _storeCreature.IsExist(creature.Id))
        {
            await creature.SaveAsync(_storeCreature);
        }
        else
        {
            var characteristicSet = new CharacteristicsSet
            {
                Strength = 10,
                Agility = 10,
                Endurance = 10,
                Wisdom = 10,
                Intelligence = 10,
                Charisma = 10,
                Id = Guid.NewGuid()
            };
            creature.CharacteristicsSetId = characteristicSet.Id;
            creature.CharacteristicsSet = characteristicSet;
            await _storeCharacteristicsSet.SaveAsync(characteristicSet);
        }
        
        return creature.Id;
    }

    public async Task<Creature> GetInfoAsync(Guid id)
    {
        var creature = await _storeCreature.GetByIdAsync(id);
        creature.CharacteristicsSet = await _storeCharacteristicsSet.GetByIdAsync(creature.CharacteristicsSetId);
        return creature;
    }

    public async Task DeleteAsync(Creature creature)
    {
        if (await _storeCharacteristicsSet.IsExist(creature.CharacteristicsSetId))
        {
            await _storeCharacteristicsSet.RemoveAsync(creature.CharacteristicsSetId);
        }
        await _storeCreature.RemoveAsync(creature);
    }
}