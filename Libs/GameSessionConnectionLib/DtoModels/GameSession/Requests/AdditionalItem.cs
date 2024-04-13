namespace GameSessionConnectionLib.DtoModels.GameSession.Requests;

public class AdditionalItem
{
    public required string Title { get; set; }
    
    public required string Description { get; set; }
    
    public required int Count { get; set; }
}