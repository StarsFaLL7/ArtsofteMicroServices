using Application.Interfaces;
using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using Domain.Entities;
using MassTransit;
using Newtonsoft.Json;

namespace Api.Saga;

public class AddItemsToCharacterConsumer : IConsumer<AddItemToCharacterRequest[]>
{
    private readonly ICharacterService _characterService;

    public AddItemsToCharacterConsumer(ICharacterService characterService)
    {
        _characterService = characterService;
    }
    
    public async Task Consume(ConsumeContext<AddItemToCharacterRequest[]> context)
    {
        var dto = context.Message;
        var result = new List<AddItemToCharacterResponse>();
        foreach (var request in dto)
        {
            var id = Guid.NewGuid();
            await _characterService.AddItem(request.CharacterId, new InventoryItem
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
        await context.RespondAsync<AddItemToCharacterResponse[]>(result.ToArray());
    }
}