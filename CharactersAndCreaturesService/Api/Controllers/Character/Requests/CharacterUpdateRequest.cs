namespace Api.Controllers.Character.Requests;

public class CharacterUpdateRequest
{
    public required Guid CharacterId { get; init; }
    
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
}