namespace Api.Controllers.Character.Requests;

public class CharacterCreateRequest
{
    public required Guid UserId { get; init; }
    
    public required string CharacterName { get; init; }
}