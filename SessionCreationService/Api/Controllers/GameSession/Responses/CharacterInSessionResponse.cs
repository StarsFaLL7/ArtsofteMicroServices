namespace SessionCreationService.Controllers.GameSession.Responses;

public class CharacterInSessionResponse
{
    public required Guid CharacterId { get; set; }
    
    public required string Name { get; set; }
    
    public required int Health { get; set; }
    
    public required int Armor { get; set; }

    public required string PathToImage { get; set; }
}