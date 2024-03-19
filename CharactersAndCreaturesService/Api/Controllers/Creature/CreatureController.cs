using Api.Controllers.Character.Requests;
using Api.Controllers.Creature.Requests;
using Api.Controllers.Creature.Responses;
using Api.Controllers.OtherResponses.CharacteristicsSet;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Api.Responses;

namespace Api.Controllers.Creature;

[Route("api/creature")]
public class CreatureController : ControllerBase
{
    private readonly ICreatureService _creatureService;

    public CreatureController(ICreatureService creatureService)
    {
        _creatureService = creatureService;
    }
    
    [HttpGet]
    [ProducesResponseType<CreatureInfoResponse>(200)]
    public async Task<IActionResult> GetCreature([FromQuery] Guid id)
    {
        var creature = await _creatureService.GetAggregatedInfoAsync(id);
        return Ok(ConvertHelper.CreatureToInfoResponse(creature));
    }
    
    [HttpGet("all")]
    [ProducesResponseType<CreatureInfoResponse>(200)]
    public async Task<IActionResult> GetCreatureBySearch([FromQuery] string search)
    {
        var creaturesWithUsernames = await _creatureService.GetCreaturesBySearchAsync(search, 10);
        var res = creaturesWithUsernames.Select(c => new CreatureSearchInfoResponse
        {
            Id = c.Id,
            UserName = c.UserName,
            UserId = c.UserId,
            ImagePath = c.ImagePath,
            Name = c.Name,
            MaxHealth = c.MaxHealth,
            Health = c.Health,
            Armor = c.Armor,
            Description = c.Description,
            Hostility = c.Hostility
        }).ToArray();
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType<CreatureInfoResponse>(200)]
    public async Task<IActionResult> CreateCreature([FromBody] CreatureCreateRequest dto)
    {
        var guid = await _creatureService.CreateOrUpdateAsync(new Domain.Entities.Creature
        {
            UserId = dto.UserId,
            ImagePath = "path/defaultCreature.jpg",
            Name = dto.Name,
            MaxHealth = 20,
            Health = 20,
            Armor = 10,
            Description = "",
            Hostility = HostilityType.Neutral,
            Id = Guid.NewGuid()
        });
        var creature = await _creatureService.GetAggregatedInfoAsync(guid);
        return Ok(ConvertHelper.CreatureToInfoResponse(creature));
    }
    
    [HttpPut]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> UpdateCreature([FromBody] CreatureUpdateRequest dto)
    {
        await _creatureService.CreateOrUpdateAsync(new Domain.Entities.Creature
        {
            UserId = default,
            ImagePath = dto.ImagePath,
            Name = dto.Name,
            MaxHealth = dto.MaxHealth,
            Health = dto.Health,
            Armor = dto.Armor,
            Description = dto.Description,
            Hostility = dto.HostilityId,
            Id = dto.Id
        });
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Creature updated"
        });
    }

    [HttpDelete]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> DeleteCreature([FromQuery] Guid id)
    {
        await _creatureService.RemoveAsync(id);
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Creature deleted"
        });
    }
    
    [HttpPut("{creatureId:Guid}/characteristics")]
    [ProducesResponseType<CharacteristicsSetStatusResponse>(200)]
    public async Task<IActionResult> UpdateCharacteristics([FromRoute] Guid creatureId, [FromBody] UpdateCharacteristicsRequest dto)
    {
        var res = await _creatureService.UpdateCharacteristicsSet(creatureId, new CharacteristicsSet
        {
            Strength = dto.Strength,
            Agility = dto.Agility,
            Endurance = dto.Endurance,
            Wisdom = dto.Wisdom,
            Intelligence = dto.Intelligence,
            Charisma = dto.Charisma,
            Id = Guid.NewGuid()
        });
        return Ok(new CharacteristicsSetStatusResponse
        {
            IsSuccess = true,
            Message = "Characteristics updated",
            Characteristics = ConvertHelper.CharacteristicsSetToResponse(res)
        });
    }
}