namespace Application.Dto;

public class SessionAggregateInfo
{
    public required Guid Id { get; set; }
    
    public required Guid CreatorId { get; set; }
    
    public required string Title { get; set; }
    
    public required CharacterInSessionInfo[] PlayersInSession { get; set; }
    
    public required CreatureInSessionInfo[] Creatures { get; set; }
}