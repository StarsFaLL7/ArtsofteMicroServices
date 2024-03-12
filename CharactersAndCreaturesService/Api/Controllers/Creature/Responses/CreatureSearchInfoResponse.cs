using Domain.Enums;

namespace Api.Controllers.Creature.Responses;

public class CreatureSearchInfoResponse
{
    public required Guid Id { get; init; }
    
    public required string UserName { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required string ImagePath { get; init; }
    
    public required string Name { get; init; }
    
    public required int MaxHealth { get; init; }
    
    public required int Health { get; init; }
    
    public required int Armor { get; init; }
    
    public required string Description { get; init; }
    
    public required HostilityType Hostility { get; init; }
}