using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCreature
{
    Task<bool> IsExist(Guid creatureId);
    
    Task SaveAsync(Creature creature);

    Task<Creature[]> GetPlayersCreaturesAsync(Guid userId);

    Task RemoveAsync(Creature creature);

    Task<Creature> GetByIdAsync(Guid id);
}