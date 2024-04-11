using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CharacterService : ICharacterService
{
    private readonly IStoreAbility _storeAbility;
    private readonly IStoreCharacter _storeCharacter;
    private readonly IStoreCharacteristicsSet _storeCharacteristicsSet;
    private readonly IStoreSkillSet _storeSkillSet;
    private readonly IStoreInventoryItem _storeInventoryItem;
    
    public CharacterService(IStoreAbility storeAbility, IStoreCharacter storeCharacter, 
        IStoreCharacteristicsSet storeCharacteristicsSet, IStoreSkillSet storeSkillSet, IStoreInventoryItem storeInventoryItem)
    {
        _storeAbility = storeAbility;
        _storeCharacter = storeCharacter;
        _storeCharacteristicsSet = storeCharacteristicsSet;
        _storeSkillSet = storeSkillSet;
        _storeInventoryItem = storeInventoryItem;
    }
    
    public async Task<Guid> CreateOrUpdateAsync(Character character)
    {
        if (await _storeCharacter.IsExistAsync(character.Id))
        {
            var aggregatedInfo = await GetAggregatedInfoAsync(character.Id);
            aggregatedInfo.Name = character.Name;
            aggregatedInfo.ImagePath = character.ImagePath;
            aggregatedInfo.Level = character.Level;
            aggregatedInfo.Experience = character.Experience;
            aggregatedInfo.MaxHealth = character.MaxHealth;
            aggregatedInfo.Health = character.Health;
            aggregatedInfo.Armor = character.Armor;
            aggregatedInfo.Race = character.Race;
            aggregatedInfo.Class = character.Class;
            aggregatedInfo.Temperament = character.Temperament;
            aggregatedInfo.Description = character.Description;
            aggregatedInfo.History = character.History;
            await aggregatedInfo.SaveAsync(_storeCharacter);
        }
        else
        {
            character.Abilities = new List<Ability>();
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
            character.CharacteristicsSet = characteristicSet;
            character.CharacteristicsSetId = characteristicSet.Id;
            var skillSet = new SkillSet
            {
                Id = Guid.NewGuid(),
                Acrobatics = 0,
                AnimalHandling = 0,
                Arcana = 0,
                Athletics = 0,
                Deception = 0,
                History = 0,
                Insight = 0,
                Intimidation = 0,
                Investigation = 0,
                Medicine = 0,
                Nature = 0,
                Perception = 0,
                Performance = 0,
                Persuasion = 0,
                Religion = 0,
                SleightOfHands = 0,
                Stealth = 0,
                Survival = 0
            };
            character.SkillSetId = skillSet.Id;
            character.SkillSetModel = skillSet;
            await _storeCharacteristicsSet.SaveAsync(characteristicSet);
            await _storeSkillSet.SaveAsync(skillSet);
            await _storeCharacter.SaveAsync(character);
        }

        return character.Id;
    }

    public async Task<Character> GetAggregatedInfoAsync(Guid id)
    {
        var character = await _storeCharacter.GetByIdAsync(id);
        character.SkillSetModel = await _storeSkillSet.GetByIdAsync(character.SkillSetId);
        character.Abilities = await _storeAbility.GetByCharacterIdAsync(character.Id);
        character.CharacteristicsSet = await _storeCharacteristicsSet.GetByIdAsync(character.CharacteristicsSetId);
        character.InventoryItems = await _storeInventoryItem.GetAllByCharacterId(character.Id);
        return character;
    }

    public async Task<Character[]> GetSimpleInfoAsync(params Guid[] ids)
    {
        return await _storeCharacter.GetByIdsAsync(ids);
    }

    public async Task<Character> GetInfoAsync(Guid id)
    {
        return await _storeCharacter.GetByIdAsync(id);
    }

    public async Task DeleteAsync(Guid characterId)
    {
        var character = await GetInfoAsync(characterId);
        await _storeAbility.RemoveByCharacterIdAsync(characterId);
        await _storeInventoryItem.RemoveByCharacterId(characterId);
        
        await _storeCharacteristicsSet.RemoveAsync(character.CharacteristicsSetId);
        await _storeSkillSet.RemoveAsync(character.SkillSetId);
        
        await _storeCharacter.RemoveAsync(characterId);
    }

    public async Task<InventoryItem[]> AddItem(Guid characterId, InventoryItem item)
    {
        await _storeCharacter.AddItemAsync(characterId, item);
        await _storeInventoryItem.SaveAsync(item);
        return await _storeInventoryItem.GetAllByCharacterId(characterId);
    }

    public async Task<InventoryItem[]> UpdateItem(Guid characterId, InventoryItem item)
    {
        var currentItem = await _storeInventoryItem.GetByIdAsync(item.Id);
        currentItem.Count = item.Count;
        currentItem.Title = item.Title;
        currentItem.Description = item.Description;
        await _storeInventoryItem.SaveAsync(currentItem);
        return await _storeInventoryItem.GetAllByCharacterId(characterId);
    }

    public async Task RemoveItem(Guid itemId)
    {
        await _storeInventoryItem.RemoveAsync(itemId);
    }

    public async Task<SkillSet> UpdateSkillSet(Guid characterId, SkillSet skillSet)
    {
        var character = await _storeCharacter.GetByIdAsync(characterId);
        await _storeSkillSet.RemoveAsync(character.SkillSetId);
        skillSet.Id = Guid.NewGuid();
        character.SkillSetId = skillSet.Id;
        await _storeSkillSet.SaveAsync(skillSet);
        return skillSet;
    }

    public async Task<CharacteristicsSet> UpdateCharacteristicsSet(Guid characterId, CharacteristicsSet characteristics)
    {
        var character = await _storeCharacter.GetByIdAsync(characterId);
        await _storeCharacteristicsSet.RemoveAsync(character.CharacteristicsSetId);
        characteristics.Id = Guid.NewGuid();
        character.CharacteristicsSetId = characteristics.Id;
        await _storeCharacteristicsSet.SaveAsync(characteristics);
        return characteristics;
    }
}