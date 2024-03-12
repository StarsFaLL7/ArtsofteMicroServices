namespace Api.Controllers.Character.Requests;

public class UpdateCharacteristicsRequest
{
    public required int Strength { get; init; }
    
    public required int Agility { get; init; }
    
    public required int Endurance { get; init; }
    
    public required int Wisdom { get; init; }

    public required int Intelligence { get; init; }
    
    public required int Charisma { get; init; }
}