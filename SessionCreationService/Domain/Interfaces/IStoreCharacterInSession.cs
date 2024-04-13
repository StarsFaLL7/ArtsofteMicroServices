using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreCharacterInSession
{
    Task SaveAsync(CharacterInSession characterInSession);

    Task SaveRangeAsync(CharacterInSession[] characterInSession);
    
    Task RemoveByIdAsync(Guid characterInSessionId);
    
    Task<CharacterInSession> GetByUserIdAsync(Guid userId);
    
    Task RemoveBySessionIdAsync(Guid sessionId);
}