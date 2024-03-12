using Domain.AggregateModels;

namespace Domain.Interfaces;

public interface ICreatureUserConnection
{
    Task<CreatureWithUserName[]> GetCreaturesBySearchAsync(string searchText, int maxCount);
}