using Api.Controllers.Ability.Responses;
using Api.Controllers.OtherResponses;
using Api.Controllers.OtherResponses.CharacteristicsSet;
using Api.Controllers.OtherResponses.Items;
using Api.Controllers.OtherResponses.SkillSet;

namespace Api.Controllers.Character.Responses;

public class CharacterInfoResponse
{
    public required Guid Id { get; init; }
    
    public required SkillSetInfoResponse SkillSet { get; init; }

    public required CharacteristicsSetInfoResponse CharacteristicsSet { get; init; }
    
    public required string Name { get; init; }
    
    public required string ImagePath { get; init; }
    
    public required int Level { get; init; }
    
    public required int Experience { get; init; }
    
    public required int MaxHealth { get; init; }
    
    public required int Health { get; init; }
    
    public required int Armor { get; init; }
    
    public required string Race { get; init; }
    
    public required string Class { get; init; }
    
    public required string Temperament { get; init; }
    
    public required string Description { get; init; }
    
    public required string History { get; init; }
    
    public required AbilityInfoResponse[] Abilities { get; init; }
    
    public required InventoryItemInfoResponse[] InventoryItems { get; init; }
}