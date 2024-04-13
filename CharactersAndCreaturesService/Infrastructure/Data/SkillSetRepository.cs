using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

internal class SkillSetRepository : IStoreSkillSet
{
    private readonly PostgresDbContext _dbContext;

    public SkillSetRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(SkillSet skillSet)
    {
        if (!_dbContext.SkillSets.Contains(skillSet))
        {
            _dbContext.SkillSets.Add(skillSet);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid skillSetId)
    {
        var item = _dbContext.SkillSets.FirstOrDefault(s => s.Id == skillSetId);
        if (item == null)
        {
            return;
        }
        _dbContext.SkillSets.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<SkillSet> GetByIdAsync(Guid id)
    {
        return _dbContext.SkillSets.First(s => s.Id == id);
    }
}