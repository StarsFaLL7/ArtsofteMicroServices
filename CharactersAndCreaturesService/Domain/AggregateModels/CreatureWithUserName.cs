using Domain.Entities;

namespace Domain.AggregateModels;

public class CreatureWithUserName : Creature
{
    public required string UserName { get; init; }
}