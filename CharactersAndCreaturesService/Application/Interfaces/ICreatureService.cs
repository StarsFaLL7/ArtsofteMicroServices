﻿using Domain.AggregateModels;
using Domain.Entities;

namespace Application.Interfaces;

public interface ICreatureService
{
    Task<Guid> CreateOrUpdateAsync(Creature creature);

    Task<Creature[]> GetInfoAsync(params Guid[] id);

    Task<CreatureWithUserName[]> GetCreaturesBySearchAsync(string searchText, int maxCount);
    
    Task<Creature> GetAggregatedInfoAsync(Guid id);

    Task RemoveAsync(Guid creatureId);
    
    Task<CharacteristicsSet> UpdateCharacteristicsSet(Guid creatureId, CharacteristicsSet characteristicsSet);
}