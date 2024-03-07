using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCreature
{
    Task<bool> IsExistAsync(Guid creatureId);
    
    Task SaveAsync(Creature creature);

    Task<Creature[]> GetPlayersCreaturesAsync(Guid userId);

    Task RemoveAsync(Guid creatureId);

    Task<Creature> GetByIdAsync(Guid id);
}