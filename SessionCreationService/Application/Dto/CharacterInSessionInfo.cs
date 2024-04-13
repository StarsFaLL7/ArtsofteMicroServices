namespace Application.Dto;

public class CharacterInSessionInfo
{
    public required Guid CharacterId { get; set; }
    
    public required string Name { get; set; }
    
    public required int Health { get; set; }
    
    public required int Armor { get; set; }

    public required string PathToImage { get; set; }
    
    public required int Level { get; set; }
    
    public required int Experience { get; set; }
    
    public required int MaxHealth { get; set; }
    
    public required string Race { get; set; }
    
    public required string Class { get; set; }
}