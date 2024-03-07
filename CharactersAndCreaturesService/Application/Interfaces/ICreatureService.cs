using Domain.Entities;

namespace Application.Interfaces;

public interface ICreatureService
{
    Task<Guid> CreateOrUpdateAsync(Creature creature);

    Task<Creature> GetInfoAsync(Guid id);

    Task DeleteAsync(Creature creature);
}