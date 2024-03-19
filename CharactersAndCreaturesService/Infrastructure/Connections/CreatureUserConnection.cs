using Domain.AggregateModels;
using Domain.Enums;
using Domain.Interfaces;
using IdentityConnectionLib.DtoModels.UserNameList;
using IdentityConnectionLib.Interfaces;

namespace Infrastructure.Connections;

internal class CreatureUserConnection : ICreatureUserConnection
{
    private readonly IStoreCreature _storeCreature;
    private readonly IIdentityConnectionService _identityConnectionService;

    public CreatureUserConnection(IIdentityConnectionService identityConnectionService, IStoreCreature storeCreature)
    {
        _identityConnectionService = identityConnectionService;
        _storeCreature = storeCreature;
    }

    public async Task<CreatureWithUserName[]> GetCreaturesBySearchAsync(string searchText, int maxCount)
    {
        var creatures = await _storeCreature.GetCreaturesBySearchAsync(searchText, maxCount);
        var requestDto = new UsernameIdentityApiRequest
        {
            UserIds = creatures.Select(c => c.UserId).ToArray()
        };
        var userNamesApiResponse = await _identityConnectionService
            .GetUserNameListAsync(requestDto);

        return creatures.Select((c, i) => new CreatureWithUserName
        {
            UserName = userNamesApiResponse.Usernames[i],
            Id = c.Id,
            UserId = c.UserId,
            ImagePath = c.ImagePath,
            Name = c.Name,
            MaxHealth = c.MaxHealth,
            Health = c.Health,
            Armor = c.Armor,
            Description = c.Description,
            Hostility = c.Hostility,
            CharacteristicsSetId = c.CharacteristicsSetId,
            CharacteristicsSet = c.CharacteristicsSet
        }).ToArray();
    }
}