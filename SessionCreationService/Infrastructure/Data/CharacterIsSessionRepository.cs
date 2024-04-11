using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CharacterIsSessionRepository : IStoreCharacterInSession
{
    private readonly PostgresDbContext _dbContext;

    public CharacterIsSessionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CharacterInSession characterInSession)
    {
        if (!_dbContext.CharacterInSessions.Contains(characterInSession))
        {
            _dbContext.CharacterInSessions.Add(characterInSession);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveRangeAsync(CharacterInSession[] characterInSession)
    {
        foreach (var character in characterInSession)
        {
            if (!_dbContext.CharacterInSessions.Contains(character))
            {
                _dbContext.CharacterInSessions.Add(character);
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveByIdAsync(Guid characterInSessionId)
    {
        var item = _dbContext.CharacterInSessions.First(c => c.Id == characterInSessionId);
        _dbContext.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CharacterInSession> GetByUserIdAsync(Guid userId)
    {
        var res = _dbContext.CharacterInSessions.First(c => c.UserId == userId);
        return res;
    }

    public async Task RemoveBySessionIdAsync(Guid sessionId)
    {
        var items = _dbContext.CharacterInSessions.First(c => c.SessionId == sessionId);
        _dbContext.CharacterInSessions.RemoveRange(items);
        await _dbContext.SaveChangesAsync();
    }
}