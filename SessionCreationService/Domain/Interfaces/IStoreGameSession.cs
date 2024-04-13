using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreGameSession
{
    Task SaveAsync(GameSession gameSession);

    Task RemoveByIdAsync(Guid gameSessionId);

    Task<GameSession> GetByIdAsync(Guid gameSessionId);
    
    Task<Guid> GetBySessionIdByCreatorUserIdAsync(Guid creatorId);
}