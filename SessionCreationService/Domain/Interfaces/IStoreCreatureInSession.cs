using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCreatureInSession
{
    Task SaveAsync(CreatureInSession creatureInSession);
    
    Task SaveRangeAsync(CreatureInSession[] creatureInSession);
    
    Task<CreatureInSession> GetByIdAsync(Guid creatureInSessionId);
    
    Task RemoveByIdAsync(Guid creatureInSessionId);

    Task RemoveBySessionIdAsync(Guid sessionId);
    
    Task<CreatureInSession[]> GetBySessionIdAsync(Guid sessionId);
}