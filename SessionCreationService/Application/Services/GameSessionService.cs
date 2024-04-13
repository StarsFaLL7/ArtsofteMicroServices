using Application.Dto;
using Application.Interfaces;
using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;
using CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;
using CharactersCreaturesConnectionLib.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityConnectionLib.Interfaces;

namespace Application.Services;

public class GameSessionService : IGameSessionService
{
    private readonly IStoreGameSession _storeGameSession;
    private readonly IStoreCharacterInSession _storeCharacterInSession;
    private readonly IStoreCreatureInSession _storeCreatureInSession;
    private readonly ICharactersCreaturesConnectionService _charactersCreaturesConnectionService;

    public GameSessionService(IStoreGameSession storeGameSession, IStoreCharacterInSession characterInSession, 
        IStoreCreatureInSession storeCreatureInSession, 
        ICharactersCreaturesConnectionService charactersCreaturesConnectionService)
    {
        _storeGameSession = storeGameSession;
        _storeCharacterInSession = characterInSession;
        _storeCreatureInSession = storeCreatureInSession;
        _charactersCreaturesConnectionService = charactersCreaturesConnectionService;
    }

    public async Task<Guid> CreateNewSessionAsync(string title, Guid creatorId, PlayerInSessionAppDto[] players,
        StartCreatureInSessionAppDto[] creaturesAtStart)
    {
        var session = new GameSession
        {
            Id = Guid.NewGuid(),
            CreatorUserId = creatorId,
            Title = title
        };
        
        var charactersInSession = new List<CharacterInSession>();
        foreach (var player in players)
        {
            charactersInSession.Add(new CharacterInSession
            {
                Id = Guid.NewGuid(),
                UserId = player.UserId,
                SessionId = session.Id,
                CharacterId = player.CharacterId,
            });
        }
        
        var startCreatures = new List<CreatureInSession>();
        var gotCreaturesInfos = await _charactersCreaturesConnectionService.GetCreatureInfo(new GetCreatureInfoRequest
        {
            CreatureIds = creaturesAtStart.Select(c => c.CreatureId).ToArray()
        });
        foreach (var creature in creaturesAtStart)
        {
            var creatureInfo = gotCreaturesInfos.First(c => c.CreatureId == creature.CreatureId);
            for (var i = 0; i < creature.Count; i++)
            {
                startCreatures.Add(new CreatureInSession
                {
                    Id = Guid.NewGuid(),
                    CreatureId = creature.CreatureId,
                    SessionId = session.Id,
                    Health = creatureInfo.Health,
                    Armor = creatureInfo.Armor,
                });
            }
        }

        await _storeGameSession.SaveAsync(session);
        await _storeCreatureInSession.SaveRangeAsync(startCreatures.ToArray());
        await _storeCharacterInSession.SaveRangeAsync(charactersInSession.ToArray());
        
        return session.Id;
    }

    public async Task<Guid> AddCreatureToSessionAsync(Guid sessionId, Guid creatureId)
    {
        var session = await _storeGameSession.GetByIdAsync(sessionId);
        var gotCreaturesInfos = await _charactersCreaturesConnectionService.GetCreatureInfo(new GetCreatureInfoRequest
        {
            CreatureIds = new []{creatureId}
        });
        session.CreaturesInSession.Add(new CreatureInSession
        {
            Id = Guid.NewGuid(),
            CreatureId = creatureId,
            SessionId = sessionId,
            Health = gotCreaturesInfos[0].Health,
            Armor = gotCreaturesInfos[0].Armor
        });
        return sessionId;
    }

    public async Task<SessionAggregateInfo> GetAggregateSessionById(Guid sessionId)
    {
        var session = await _storeGameSession.GetByIdAsync(sessionId);
        var gotCharacterInfos = await _charactersCreaturesConnectionService.GetCharacterInfo(new GetCharacterInfoRequest
        {
            CharactersIds = session.CharactersInSession.Select(c => c.CharacterId).ToArray()
        });
        var gotCreaturesInfos = await _charactersCreaturesConnectionService.GetCreatureInfo(new GetCreatureInfoRequest
        {
            CreatureIds = session.CreaturesInSession.Select(c => c.CreatureId).ToArray()
        });

        var creaturesInSession = new List<CreatureInSessionInfo>();

        foreach (var creatureInfo in session.CreaturesInSession)
        {
            var currentInSessionInfo = gotCreaturesInfos.First(c => c.CreatureId == creatureInfo.CreatureId);
            creaturesInSession.Add(new CreatureInSessionInfo
            {
                CreatureId = creatureInfo.CreatureId,
                Name = currentInSessionInfo.Name,
                Health = creatureInfo.Health,
                Armor = creatureInfo.Armor,
                PathToImage = currentInSessionInfo.ImagePath,
                MaxHealth = currentInSessionInfo.MaxHealth
            });
        }
        
        var result = new SessionAggregateInfo
        {
            Id = session.Id,
            CreatorId = session.CreatorUserId,
            Title = session.Title,
            PlayersInSession = gotCharacterInfos.Select(c => new CharacterInSessionInfo
            {
                CharacterId = c.CharacterId,
                Name = c.Name,
                Health = c.Health,
                Armor = c.Armor,
                PathToImage = c.ImagePath,
                Level = c.Level,
                Experience = c.Experience,
                MaxHealth = c.MaxHealth,
                Race = c.Race,
                Class = c.Class
            }).ToArray(),
            Creatures = creaturesInSession.ToArray()
        };
        return result;
    }

    public async Task<SessionAggregateInfo> GetAggregateSessionByPlayerUserId(Guid userId)
    {
        var characterInSession = await _storeCharacterInSession.GetByUserIdAsync(userId);
        return await GetAggregateSessionById(characterInSession.SessionId);
    }

    public async Task<SessionAggregateInfo> GetAggregateSessionByCreatorUserId(Guid creatorUserId)
    {
        var sessionId = await _storeGameSession.GetBySessionIdByCreatorUserIdAsync(creatorUserId);
        return await GetAggregateSessionById(sessionId);
    }

    public async Task RemoveCreatureFromSession(Guid sessionId, Guid creatureInSessionId)
    {
        await _storeCreatureInSession.RemoveByIdAsync(sessionId);
    }

    public async Task RemoveSession(Guid sessionId)
    {
        await _storeCreatureInSession.RemoveBySessionIdAsync(sessionId);
        await _storeCharacterInSession.RemoveBySessionIdAsync(sessionId);
        await _storeGameSession.RemoveByIdAsync(sessionId);
    }
}