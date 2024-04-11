namespace SessionCreationService.Controllers.GameSession.Requests;

public class AddCreatureToSessionRequest
{
    public Guid CreatureId { get; set; }
    
    public Guid SessionId { get; set; }
}