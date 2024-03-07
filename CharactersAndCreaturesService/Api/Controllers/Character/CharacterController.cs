using Api.Controllers.Character.Requests;
using Api.Controllers.Character.Responses;
using Api.Controllers.OtherResponses;
using Api.Controllers.OtherResponses.CharacteristicsSet;
using Api.Controllers.OtherResponses.Items;
using Api.Controllers.OtherResponses.SkillSet;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Api.Responses;

namespace Api.Controllers.Character;

[Route("api/character")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet]
    [ProducesResponseType<CharacterInfoResponse>(200)]
    public async Task<IActionResult> GetCharacter([FromQuery] Guid id)
    {
        var character = await _characterService.GetAggregatedInfoAsync(id);
        return Ok(ConvertHelper.CharacterToInfoResponse(character));
    }
    
    [HttpPost]
    [ProducesResponseType<CharacterInfoResponse>(200)]
    public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateRequest dto)
    {
        var guid = await _characterService.CreateOrUpdateAsync(new Domain.Entities.Character
        {
            UserId = dto.UserId,
            SkillSetId = default,
            CharacteristicsSetId = default,
            CharacteristicsSet = null,
            Name = dto.CharacterName,
            ImagePath = "path/defaultCharacter.jpg",
            Level = 1,
            Experience = 0,
            MaxHealth = 20,
            Health = 20,
            Armor = 10,
            Race = "Human",
            Class = "Warrior",
            Temperament = null,
            Description = null,
            History = null,
            Abilities = null,
            InventoryItems = new List<InventoryItem>(),
            Id = Guid.NewGuid()
        });
        var character = await _characterService.GetAggregatedInfoAsync(guid);
        return Ok(ConvertHelper.CharacterToInfoResponse(character));
    }
    
    [HttpPut]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> UpdateCharacter([FromBody] CharacterUpdateRequest dto)
    {
        await _characterService.CreateOrUpdateAsync(new Domain.Entities.Character
        {
            UserId = default,
            SkillSetId = default,
            CharacteristicsSetId = default,
            CharacteristicsSet = null,
            Name = dto.Name,
            ImagePath = dto.ImagePath,
            Level = dto.Level,
            Experience = dto.Experience,
            MaxHealth = dto.MaxHealth,
            Health = dto.Health,
            Armor = dto.Armor,
            Race = dto.Race,
            Class = dto.Class,
            Temperament = dto.Temperament,
            Description = dto.Description,
            History = dto.History,
            Abilities = null,
            InventoryItems = null,
            Id = dto.CharacterId
        });
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Character updated"
        });
    }

    [HttpDelete]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> DeleteCharacter([FromQuery] Guid id)
    {
        await _characterService.DeleteAsync(id);
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Character deleted"
        });
    }
    
    [HttpPost("{characterId:Guid}/items")]
    [ProducesResponseType<ItemsArrayStatusResponse>(200)]
    public async Task<IActionResult> AddItem([FromRoute] Guid characterId, [FromBody] AddItemRequest dto)
    {
        var items = await _characterService.AddItem(characterId, new InventoryItem
        {
            CharacterId = characterId,
            Title = dto.Title,
            Description = dto.Description,
            Count = dto.Count,
            Id = Guid.NewGuid()
        });
        return Ok(new ItemsArrayStatusResponse
        {
            IsSuccess = true,
            Message = "Item added",
            Items = ConvertHelper.InvItemsToInfoResponseArray(items)
        });
    }
    
    [HttpDelete("{characterId:Guid}/items")]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> RemoveItem([FromRoute] Guid characterId, [FromQuery] Guid itemId)
    {
        await _characterService.RemoveItem(itemId);
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Item deleted"
        });
    }
    
    [HttpPut("{characterId:Guid}/items")]
    [ProducesResponseType<ItemsArrayStatusResponse>(200)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid characterId, [FromBody] UpdateItemRequest itemDto)
    {
        var items = await _characterService.UpdateItem(characterId, new InventoryItem
        {
            CharacterId = characterId,
            Title = itemDto.Title,
            Description = itemDto.Description,
            Count = itemDto.Count,
            Id = itemDto.ItemId
        });
        return Ok(new ItemsArrayStatusResponse
        {
            IsSuccess = true,
            Message = "Item updated",
            Items = ConvertHelper.InvItemsToInfoResponseArray(items)
        });
    }
    
    [HttpPut("{characterId:Guid}/skills")]
    [ProducesResponseType<SkillSetStatusResponse>(200)]
    public async Task<IActionResult> UpdateSkillSet([FromRoute] Guid characterId, [FromBody] UpdateSkillSetRequest dto)
    {
        var res = await _characterService.UpdateSkillSet(characterId, new SkillSet
        {
            Acrobatics = dto.Acrobatics,
            AnimalHandling = dto.AnimalHandling,
            Arcana = dto.Arcana,
            Athletics = dto.Athletics,
            Deception = dto.Deception,
            History = dto.History,
            Insight = dto.Insight,
            Intimidation = dto.Intimidation,
            Investigation = dto.Investigation,
            Medicine = dto.Medicine,
            Nature = dto.Nature,
            Perception = dto.Perception,
            Performance = dto.Performance,
            Persuasion = dto.Perception,
            Religion = dto.Religion,
            SleightOfHands = dto.SleightOfHands,
            Stealth = dto.Stealth,
            Survival = dto.Survival,
            Id = default
        });
        return Ok(new SkillSetStatusResponse
        {
            IsSuccess = true,
            Message = "SkillSet updated",
            SkillSet = ConvertHelper.SkillSetToResponse(res)
        });
    }
    
    [HttpPut("{characterId:Guid}/characteristics")]
    [ProducesResponseType<CharacteristicsSetStatusResponse>(200)]
    public async Task<IActionResult> UpdateCharacteristics([FromRoute] Guid characterId, [FromBody] UpdateCharacteristicsRequest dto)
    {
        var res = await _characterService.UpdateCharacteristicsSet(characterId, new CharacteristicsSet
        {
            Strength = dto.Strength,
            Agility = dto.Agility,
            Endurance = dto.Endurance,
            Wisdom = dto.Wisdom,
            Intelligence = dto.Intelligence,
            Charisma = dto.Charisma,
            Id = default
        });
        return Ok(new CharacteristicsSetStatusResponse
        {
            IsSuccess = true,
            Message = "Characteristics updated",
            Characteristics = ConvertHelper.CharacteristicsSetToResponse(res)
        });
    }
}