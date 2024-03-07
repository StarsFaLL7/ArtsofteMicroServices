namespace Api.Controllers.Character.Requests;

public class UpdateItemRequest
{
    public required Guid ItemId { get; init; }
    
    public required string Title { get; init; }
    
    public required int Count { get; init; }
    
    public required string Description { get; init; }
}