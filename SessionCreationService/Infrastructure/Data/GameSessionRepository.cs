using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

internal class GameSessionRepository : IStoreGameSession
{
    private readonly PostgresDbContext _dbContext;

    public GameSessionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(GameSession gameSession)
    {
        if (!_dbContext.GameSessions.Contains(gameSession))
        {
            _dbContext.GameSessions.Add(gameSession);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid gameSessionId)
    {
        var item = _dbContext.GameSessions.First(s => s.Id == gameSessionId);
        _dbContext.GameSessions.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<GameSession> GetByIdAsync(Guid gameSessionId)
    {
        var res = _dbContext.GameSessions
            .Include(s => s.CharactersInSession)
            .Include(s => s.CreaturesInSession)
            .First(s => s.Id == gameSessionId);
        return res;
    }

    public async Task<Guid> GetBySessionIdByCreatorUserIdAsync(Guid creatorId)
    {
        var res = _dbContext.GameSessions.First(s => s.CreatorUserId == creatorId);
        return res.Id;
    }
}