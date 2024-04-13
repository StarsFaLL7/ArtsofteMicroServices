namespace SessionCreationService.Controllers.GameSession.Responses;

public class SessionInfoResponse
{
    public required Guid Id { get; set; }
    
    public required Guid CreatorId { get; set; }
    
    public required string Title { get; set; }
    
    public required CharacterInSessionResponse[] Characters { get; set; }
    
    public required CreatureInSessionResponse[] Creatures { get; set; }
}