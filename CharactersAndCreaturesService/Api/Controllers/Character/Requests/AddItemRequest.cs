namespace Api.Controllers.Character.Requests;

public class AddItemRequest
{
    public required string Title { get; init; }
    
    public required int Count { get; init; }
    
    public required string Description { get; init; }
}