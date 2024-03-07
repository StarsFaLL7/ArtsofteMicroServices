namespace Api.Controllers.Ability.Requests;

public class AbilityUpdateRequest
{
    public required Guid AbilityId { get; init; }
    
    public required string Title { get; init; }
    
    public required string ImagePath { get; init; }
    
    public required string Description { get; init; }
    
    public string Damage { get; init; }
    
    public required bool IsHealing { get; init; }
}