using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreSkillSet
{
    Task SaveAsync(SkillSet skillSet);

    Task RemoveAsync(Guid skillSetId);
    
    Task<SkillSet> GetByIdAsync(Guid id);
}