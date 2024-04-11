namespace GameSessionConnectionLib.DtoModels.GameSession.Requests;

public class StartCreatureInSession
{
    public required Guid CreatureId { get; set; }

    public required int Count { get; set; } = 1;
}