namespace GameSessionConnectionLib.DtoModels.GameSession.Requests;

public class PlayerInSession
{
    public required Guid UserId { get; set; }
    
    public required Guid CharacterId { get; set; }
    
    public required AdditionalItem[] AdditionalItems { get; set; }
}