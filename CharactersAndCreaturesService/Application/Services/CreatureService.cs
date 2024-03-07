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
        if (await _storeCreature.IsExistAsync(creature.Id))
        {
            var currentCreature = await _storeCreature.GetByIdAsync(creature.Id);
            creature.UserId = currentCreature.UserId;
            creature.CharacteristicsSetId = currentCreature.CharacteristicsSetId;
            await _storeCreature.RemoveAsync(currentCreature.Id);
            await _storeCreature.SaveAsync(creature);
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
            await _storeCreature.SaveAsync(creature);
        }
        
        return creature.Id;
    }

    public async Task<Creature> GetInfoAsync(Guid id)
    {
        return await _storeCreature.GetByIdAsync(id);
    }

    public async Task<Creature> GetAggregatedInfoAsync(Guid id)
    {
        var creature = await _storeCreature.GetByIdAsync(id);
        creature.CharacteristicsSet = await _storeCharacteristicsSet.GetByIdAsync(creature.CharacteristicsSetId);
        return creature;
    }

    public async Task RemoveAsync(Guid creatureId)
    {
        var creature = await _storeCreature.GetByIdAsync(creatureId);
        if (await _storeCharacteristicsSet.IsExist(creature.CharacteristicsSetId))
        {
            await _storeCharacteristicsSet.RemoveAsync(creature.CharacteristicsSetId);
        }
        await _storeCreature.RemoveAsync(creature.Id);
    }

    public async Task<CharacteristicsSet> UpdateCharacteristicsSet(Guid creatureId, CharacteristicsSet characteristicsSet)
    {
        var creature = await _storeCreature.GetByIdAsync(creatureId);
        await _storeCharacteristicsSet.RemoveAsync(creature.CharacteristicsSetId);
        characteristicsSet.Id = Guid.NewGuid();
        creature.CharacteristicsSetId = characteristicsSet.Id;
        await _storeCharacteristicsSet.SaveAsync(characteristicsSet);
        return characteristicsSet;
    }
}