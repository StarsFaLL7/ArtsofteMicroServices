using Application.Dto;
using Application.Interfaces;
using GameSessionConnectionLib.DtoModels.GameSession.Requests;
using MassTransit;
using SessionCreationService.Controllers.GameSession.Responses;

namespace SessionCreationService.Saga;

public class CreateSessionConsumer : IConsumer<SessionCreationRequest>
{
    private readonly IGameSessionService _gameSessionService;

    public CreateSessionConsumer(IGameSessionService gameSessionService)
    {
        _gameSessionService = gameSessionService;
    }
    
    public async Task Consume(ConsumeContext<SessionCreationRequest> context)
    {
        var dto = context.Message;
        var sessionId = await _gameSessionService.CreateNewSessionAsync(dto.Title, dto.CreatorId, dto.Players.Select(p => new PlayerInSessionAppDto
        {
            UserId = p.UserId,
            CharacterId = p.CharacterId,
            AdditionalItems = p.AdditionalItems.Select(item => new AdditionalItemAppDto
            {
                Title = item.Title,
                Description = item.Title,
                Count = item.Count
            }).ToArray()
        }).ToArray(), dto.Creatures.Select(c => new StartCreatureInSessionAppDto
        {
            CreatureId = c.CreatureId,
            Count = c.Count
        }).ToArray());
        var session = await _gameSessionService.GetAggregateSessionById(sessionId);
        var res = new SessionInfoResponse
        {
            Id = session.Id,
            CreatorId = session.CreatorId,
            Title = session.Title,
            Characters = session.PlayersInSession.Select(p => new CharacterInSessionResponse
            {
                CharacterId = p.CharacterId,
                Name = p.Name,
                Health = p.Health,
                Armor = p.Armor,
                PathToImage = p.PathToImage
            }).ToArray(),
            Creatures = session.Creatures.Select(c => new CreatureInSessionResponse
            {
                CreatureId = c.CreatureId,
                Name = c.Name,
                Health = c.Health,
                Armor = c.Armor,
                PathToImage = c.PathToImage
            }).ToArray()
        };
        await context.RespondAsync<SessionInfoResponse>(res);
    }
}