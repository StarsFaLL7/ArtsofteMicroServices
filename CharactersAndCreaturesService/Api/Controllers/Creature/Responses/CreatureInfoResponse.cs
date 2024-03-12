using Api.Controllers.OtherResponses.CharacteristicsSet;
using Domain.Enums;

namespace Api.Controllers.Creature.Responses;

public class CreatureInfoResponse
{
    public required Guid UserId { get; init; }
    
    public required string ImagePath { get; init; }
    
    public required string Name { get; init; }
    
    public required int MaxHealth { get; init; }
    
    public required int Health { get; init; }
    
    public required int Armor { get; init; }
    
    public required string Description { get; init; }
    
    public required HostilityType HostilityId { get; init; }
    
    public required CharacteristicsSetInfoResponse CharacteristicsSet { get; init; }
}