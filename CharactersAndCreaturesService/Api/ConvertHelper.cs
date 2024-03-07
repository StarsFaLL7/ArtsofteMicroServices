using Api.Controllers.Ability.Responses;
using Api.Controllers.Character.Responses;
using Api.Controllers.Creature.Responses;
using Api.Controllers.OtherResponses;
using Api.Controllers.OtherResponses.CharacteristicsSet;
using Api.Controllers.OtherResponses.Items;
using Api.Controllers.OtherResponses.SkillSet;
using Domain.Entities;
using Domain.Enums;

namespace Api;

public static class ConvertHelper
{
    public static SkillSetInfoResponse SkillSetToResponse(SkillSet skillSet)
    {
        return new SkillSetInfoResponse
        {
            Acrobatics = skillSet.Acrobatics,
            AnimalHandling = skillSet.AnimalHandling,
            Arcana = skillSet.Arcana,
            Athletics = skillSet.Athletics,
            Deception = skillSet.Deception,
            History = skillSet.History,
            Insight = skillSet.Insight,
            Intimidation = skillSet.Intimidation,
            Investigation = skillSet.Investigation,
            Medicine = skillSet.Medicine,
            Nature = skillSet.Nature,
            Perception = skillSet.Perception,
            Performance = skillSet.Performance,
            Persuasion = skillSet.Persuasion,
            Religion = skillSet.Religion,
            SleightOfHands = skillSet.SleightOfHands,
            Stealth = skillSet.Stealth,
            Survival = skillSet.Survival
        };
    }

    public static CharacteristicsSetInfoResponse CharacteristicsSetToResponse(CharacteristicsSet set)
    {
        return new CharacteristicsSetInfoResponse
        {
            Strength = set.Strength,
            Agility = set.Agility,
            Endurance = set.Endurance,
            Wisdom = set.Wisdom,
            Intelligence = set.Intelligence,
            Charisma = set.Charisma
        };
    }

    public static AbilityInfoResponse[] AbilitiesToInfoResponseArray(ICollection<Ability> abilities)
    {
        return abilities.Select(a => new AbilityInfoResponse
        {
            Id = a.Id,
            Title = a.Title,
            ImagePath = a.ImagePath,
            Description = a.Description,
            IsHealing = a.IsHealing
        }).ToArray();
    }
    
    public static InventoryItemInfoResponse[] InvItemsToInfoResponseArray(ICollection<InventoryItem> inventoryItems)
    {
        return inventoryItems.Select(item => new InventoryItemInfoResponse
        {
            Title = item.Title,
            Description = item.Description,
            Count = item.Count,
            Id = item.Id
        }).ToArray();
    }

    public static CharacterInfoResponse CharacterToInfoResponse(Character character)
    {
        return new CharacterInfoResponse
        {
            SkillSet = SkillSetToResponse(character.SkillSetModel),
            CharacteristicsSet = CharacteristicsSetToResponse(character.CharacteristicsSet),
            Name = character.Name,
            ImagePath = character.ImagePath,
            Level = character.Level,
            Experience = character.Experience,
            MaxHealth = character.MaxHealth,
            Health = character.Health,
            Armor = character.Armor,
            Race = character.Race,
            Class = character.Class,
            Temperament = character.Temperament,
            Description = character.Description,
            History = character.History,
            Abilities = AbilitiesToInfoResponseArray(character.Abilities),
            InventoryItems = InvItemsToInfoResponseArray(character.InventoryItems),
            Id = character.Id
        };
    }

    public static CreatureInfoResponse CreatureToInfoResponse(Creature creature)
    {
        return new CreatureInfoResponse
        {
            UserId = creature.UserId,
            ImagePath = creature.ImagePath,
            Name = creature.Name,
            MaxHealth = creature.MaxHealth,
            Health = creature.Health,
            Armor = creature.Armor,
            Description = creature.Description,
            HostilityId = HostilityType.Neutral,
            CharacteristicsSet = CharacteristicsSetToResponse(creature.CharacteristicsSet)
        };
    }
}