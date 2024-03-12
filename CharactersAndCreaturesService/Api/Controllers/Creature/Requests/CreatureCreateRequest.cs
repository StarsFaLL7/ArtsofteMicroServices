namespace Api.Controllers.Creature.Requests;

public class CreatureCreateRequest
{
    public required Guid UserId { get; init; }
    
    public required string Name { get; init; }
}