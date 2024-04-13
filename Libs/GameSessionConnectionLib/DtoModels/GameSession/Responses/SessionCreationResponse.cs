namespace GameSessionConnectionLib.DtoModels.GameSession.Responses;

public class SessionCreationResponse
{
    public required Guid Id { get; set; }
    
    public required string Status { get; set; }
}