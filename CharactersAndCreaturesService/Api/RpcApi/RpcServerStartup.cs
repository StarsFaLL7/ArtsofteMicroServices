using Application.Interfaces;
using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;
using CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;
using Domain.Entities;
using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityConnectionLib.DtoModels.UserNameList;
using Newtonsoft.Json;
using ProjectCore.RPCLogic.Interfaces;

namespace Api.RpcApi;

public static class RpcServerStartup
{
    public static IApplicationBuilder UseRpcServerConnectionLib(this IApplicationBuilder builder)
    {
        var config = builder.ApplicationServices.GetRequiredService<IConfiguration>();
        var configRpcSection = config.GetSection("CharactersCreaturesConnectionLib")
            .GetSection("Rpc");
        var queueAddItemToCharacter = configRpcSection.GetValue<string>("QueueAddItemToCharacter");
        var queueGetCharacterInfo = configRpcSection.GetValue<string>("QueueGetCharacterInfo");
        var queueGetCreatureInfo = configRpcSection.GetValue<string>("QueueGetCreatureInfo");

        if (queueGetCharacterInfo is null || queueGetCreatureInfo is null || queueAddItemToCharacter is null)
        {
            throw new Exception("Ошибка конфигурации. Не заданы одно или несколько значений очередей сообщений для rpc.");
        }
        
        var serverAddItemToCharacter = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        serverAddItemToCharacter.StartAsync(queueAddItemToCharacter, async (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<AddItemToCharacterRequest[]>(msg);
            var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
            var result = new List<AddItemToCharacterResponse>();
            foreach (var request in dto)
            {
                var id = Guid.NewGuid();
                await characterService.AddItem(request.CharacterId, new InventoryItem
                {
                    CharacterId = request.CharacterId,
                    Title = request.Title,
                    Description = request.Description,
                    Count = request.Count,
                    Id = id
                });
                result.Add(new AddItemToCharacterResponse
                {
                    ItemId = id,
                    TraceId = dto[0].TraceId
                });
            }
            return JsonConvert.SerializeObject(result);
        });
        
        var serverGetCharacterInfo = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        serverGetCharacterInfo.StartAsync(queueGetCharacterInfo, async (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<GetCharacterInfoRequest>(msg);
            var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();

            var res = await characterService.GetSimpleInfoAsync(dto.CharactersIds);
            var result = res.Select(c => new GetCharacterInfoResponse
            {
                Name = c.Name,
                ImagePath = c.ImagePath,
                Level = c.Level,
                Experience = c.Experience,
                MaxHealth = c.MaxHealth,
                Health = c.Health,
                Armor = c.Armor,
                Race = c.Race,
                Class = c.Class,
                CharacterId = c.Id
            }).ToArray();
            return JsonConvert.SerializeObject(result);
        });
        
        var serverGetCreatureInfo = builder.ApplicationServices.GetRequiredService<IRpcServer>();
        serverGetCreatureInfo.StartAsync(queueGetCreatureInfo, async (msg, scope) =>
        {
            var dto = JsonConvert.DeserializeObject<GetCreatureInfoRequest>(msg);
            var creatureService = scope.ServiceProvider.GetRequiredService<ICreatureService>();

            var res = await creatureService.GetInfoAsync(dto.CreatureIds);
            var result = res.Select(c => new GetCreatureInfoResponse
            {
                ImagePath = c.ImagePath,
                Name = c.Name,
                MaxHealth = c.MaxHealth,
                Health = c.Health,
                Armor = c.Armor,
                CreatureId = c.Id,
            }).ToArray();
            return JsonConvert.SerializeObject(result);
        });
        
        return builder;
    }
}