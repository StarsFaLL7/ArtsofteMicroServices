using Application.Dto;
using Domain.Entities;

namespace Application.Interfaces;

public interface IGameSessionService
{
    Task<Guid> CreateNewSessionAsync(string title, Guid creatorId, PlayerInSessionAppDto[] players, StartCreatureInSessionAppDto[] creaturesAtStart);

    Task<Guid> AddCreatureToSessionAsync(Guid sessionId, Guid creatureId);

    Task<SessionAggregateInfo> GetAggregateSessionById(Guid sessionId);
    
    Task<SessionAggregateInfo> GetAggregateSessionByPlayerUserId(Guid userId);

    Task<SessionAggregateInfo> GetAggregateSessionByCreatorUserId(Guid creatorUserId);

    Task RemoveCreatureFromSession(Guid sessionId, Guid creatureInSessionId);
    
    Task RemoveSession(Guid sessionId);

}