using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

internal class CreatureInSessionRepository : IStoreCreatureInSession
{
    private readonly PostgresDbContext _dbContext;

    public CreatureInSessionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CreatureInSession creatureInSession)
    {
        if (!_dbContext.CreatureInSessions.Contains(creatureInSession))
        {
            _dbContext.CreatureInSessions.Add(creatureInSession);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveRangeAsync(CreatureInSession[] creatureInSession)
    {
        foreach (var creature in creatureInSession)
        {
            if (!_dbContext.CreatureInSessions.Contains(creature))
            {
                _dbContext.CreatureInSessions.Add(creature);
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CreatureInSession> GetByIdAsync(Guid creatureInSessionId)
    {
        var res = _dbContext.CreatureInSessions.First(c => c.Id == creatureInSessionId);
        return res;
    }

    public async Task RemoveByIdAsync(Guid creatureInSessionId)
    {
        var item = _dbContext.CreatureInSessions.First(c => c.Id == creatureInSessionId);
        _dbContext.CreatureInSessions.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveBySessionIdAsync(Guid sessionId)
    {
        var items = _dbContext.CreatureInSessions.Where(c => c.SessionId == sessionId);
        _dbContext.RemoveRange(items);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CreatureInSession[]> GetBySessionIdAsync(Guid sessionId)
    {
        var res = _dbContext.CreatureInSessions.Where(c => c.SessionId == sessionId).ToArray();
        return res;
    }
}