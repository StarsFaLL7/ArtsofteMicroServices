using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SessionCreationService.Controllers.GameSession.Requests;
using SessionCreationService.Controllers.GameSession.Responses;

namespace SessionCreationService.Controllers.GameSession;

[Route("/api/sessions")]
public class GameSessionController : ControllerBase
{
    private readonly IGameSessionService _gameSessionService;

    public GameSessionController(IGameSessionService gameSessionService)
    {
        _gameSessionService = gameSessionService;
    }
    
    [HttpPost("add-creature")]
    [ProducesResponseType<SessionInfoResponse>(200)]
    public async Task<IActionResult> AddCreatureToSession([FromBody] AddCreatureToSessionRequest dto)
    {
        var sessionId = await _gameSessionService.AddCreatureToSessionAsync(dto.SessionId, dto.CreatureId);
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
        return Ok(res);
    }
    
    [HttpGet]
    [ProducesResponseType<SessionInfoResponse>(200)]
    public async Task<IActionResult> GetActiveSessionByPlayerUserId([FromQuery] Guid userId)
    {
        var info = await _gameSessionService.GetAggregateSessionByPlayerUserId(userId);
        var res = new SessionInfoResponse
        {
            Id = info.Id,
            CreatorId = info.CreatorId,
            Title = info.Title,
            Characters = info.PlayersInSession.Select(p => new CharacterInSessionResponse
            {
                CharacterId = p.CharacterId,
                Name = p.Name,
                Health = p.Health,
                Armor = p.Armor,
                PathToImage = p.PathToImage
            }).ToArray(),
            Creatures = info.Creatures.Select(c => new CreatureInSessionResponse
            {
                CreatureId = c.CreatureId,
                Name = c.Name,
                Health = c.Health,
                Armor = c.Armor,
                PathToImage = c.PathToImage
            }).ToArray()
        };
        return Ok(res);
    }
    
    [HttpGet("creator")]
    [ProducesResponseType<SessionInfoResponse>(200)]
    public async Task<IActionResult> GetActiveSessionByCreatorUserId([FromQuery] Guid userId)
    {
        var info = await _gameSessionService.GetAggregateSessionByPlayerUserId(userId);
        var res = new SessionInfoResponse
        {
            Id = info.Id,
            CreatorId = info.CreatorId,
            Title = info.Title,
            Characters = info.PlayersInSession.Select(p => new CharacterInSessionResponse
            {
                CharacterId = p.CharacterId,
                Name = p.Name,
                Health = p.Health,
                Armor = p.Armor,
                PathToImage = p.PathToImage
            }).ToArray(),
            Creatures = info.Creatures.Select(c => new CreatureInSessionResponse
            {
                CreatureId = c.CreatureId,
                Name = c.Name,
                Health = c.Health,
                Armor = c.Armor,
                PathToImage = c.PathToImage
            }).ToArray()
        };
        return Ok(res);
    }
}