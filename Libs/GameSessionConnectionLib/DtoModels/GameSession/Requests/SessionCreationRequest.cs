namespace GameSessionConnectionLib.DtoModels.GameSession.Requests;

public class SessionCreationRequest
{
    public required Guid SessionId { get; set; }
    
    public Guid TraceId { get; set; }
    
    public required string Title { get; set; }
    
    public required Guid CreatorId { get; set; }
    
    public required PlayerInSession[] Players { get; set; }
    
    public required StartCreatureInSession[] Creatures { get; set; }
}