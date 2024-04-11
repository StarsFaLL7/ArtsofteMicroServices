namespace Application.Dto;

public class PlayerInSessionAppDto
{
    public required Guid UserId { get; set; }
    
    public required Guid CharacterId { get; set; }
    
    public required AdditionalItemAppDto[] AdditionalItems { get; set; }
}