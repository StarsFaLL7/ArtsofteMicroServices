namespace Api.Controllers.OtherResponses.Items;

public class InventoryItemInfoResponse
{
    public required Guid Id { get; init; }
    
    public required string Title { get; init; }
    
    public required int Count { get; init; }
    
    public required string Description { get; init; }
}