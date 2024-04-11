using Application.Interfaces;
using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;
using CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.TraceIdLogic.Interfaces;

namespace Api.Controllers.ConnectionApi;

[Route("api/internal")]
public class ConnectionLibController : ControllerBase
{
    private readonly ICharacterService _characterService;
    private readonly ICreatureService _creatureService;

    public ConnectionLibController(ICharacterService characterService, ICreatureService creatureService)
    {
        _characterService = characterService;
        _creatureService = creatureService;
    }
    
    [HttpPost("/characters/items")]
    [ProducesResponseType<AddItemToCharacterResponse[]>(200)]
    public async Task<IActionResult> AddItemsToCharacter([FromBody] AddItemToCharacterRequest[] requests, [FromServices] ITraceWriter traceWriter)
    {
        var res = new List<AddItemToCharacterResponse>();
        foreach (var request in requests)
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
            res.Add(new AddItemToCharacterResponse
            {
                ItemId = id,
                TraceId = Guid.Parse(traceWriter.GetValue())
            });
        }
        
        return Ok(res.ToArray());
    }
    
    [HttpPost("/characters")]
    [ProducesResponseType<GetCharacterInfoResponse[]>(200)]
    public async Task<IActionResult> GetCharacterInfo([FromBody] GetCharacterInfoRequest request)
    {

        var res = await _characterService.GetSimpleInfoAsync(request.CharactersIds);
        
        return Ok(res.Select(c => new GetCharacterInfoResponse
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
        }).ToArray());
    }
    
    [HttpPost("/creatures")]
    [ProducesResponseType<GetCreatureInfoResponse[]>(200)]
    public async Task<IActionResult> GetCreaturesInfo([FromBody] GetCreatureInfoRequest request)
    {

        var res = await _creatureService.GetInfoAsync(request.CreatureIds);
        
        return Ok(res.Select(c => new GetCreatureInfoResponse
        {
            ImagePath = c.ImagePath,
            Name = c.Name,
            MaxHealth = c.MaxHealth,
            Health = c.Health,
            Armor = c.Armor,
            CreatureId = c.Id,
        }).ToArray());
    }
}