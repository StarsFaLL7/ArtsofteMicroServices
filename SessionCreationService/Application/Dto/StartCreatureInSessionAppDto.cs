namespace Application.Dto;

public class StartCreatureInSessionAppDto
{
    public required Guid CreatureId { get; set; }

    public required int Count { get; set; } = 1;
}