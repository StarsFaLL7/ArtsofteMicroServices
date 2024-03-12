using Api.Controllers.Ability.Responses;
using Api.Controllers.Ability.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjectCore.Api.Responses;

namespace Api.Controllers.Ability;

[Route("api/ability")]
public class AbilityController : ControllerBase
{
    private readonly IAbilityService _abilityService;
    
    public AbilityController(IAbilityService abilityService)
    {
        _abilityService = abilityService;
    }
    
    
    [HttpPost]
    [ProducesResponseType<AbilityStatusResponse>(200)]
    public async Task<IActionResult> AddNewAbility([FromBody] AbilityCreateRequest dto)
    {
        var guid = await _abilityService.CreateOrUpdateAsync(new Domain.Entities.Ability
        {
            CharacterId = dto.CharacterId,
            Title = dto.Title,
            ImagePath = string.IsNullOrWhiteSpace(dto.ImagePath) ? "/path/defaultAbility.jpg" : dto.ImagePath,
            Description = dto.Description,
            Damage = dto.Damage,
            IsHealing = dto.IsHealing,
            Id = Guid.NewGuid()
        });
        return Ok(new AbilityStatusResponse
        {
            AbilityGuid = guid,
            IsSuccess = true,
            Message = "Ability created"
        });
    }
    
    [HttpPut]
    [ProducesResponseType<AbilityStatusResponse>(200)]
    public async Task<IActionResult> UpdateAbility([FromBody] AbilityUpdateRequest dto)
    {
        var ability = await _abilityService.GetInfoAsync(dto.AbilityId);
        var guid = await _abilityService.CreateOrUpdateAsync(new Domain.Entities.Ability
        {
            CharacterId = ability.CharacterId,
            Title = dto.Title,
            ImagePath = string.IsNullOrWhiteSpace(dto.ImagePath) ? "/path/defaultAbility.jpg" : dto.ImagePath,
            Description = dto.Description,
            Damage = dto.Damage,
            IsHealing = dto.IsHealing,
            Id = dto.AbilityId
        });
        return Ok(new AbilityStatusResponse
        {
            AbilityGuid = guid,
            IsSuccess = true,
            Message = "Ability updated"
        });
    }
    
    [HttpGet]
    [ProducesResponseType<AbilitiesFromCharacterResponse>(200)]
    public async Task<IActionResult> GetAbilitiesByCharacter([FromQuery] Guid characterId)
    {
        var ability = await _abilityService.GetAbilitiesByCharacter(characterId);
       
        return Ok(new AbilitiesFromCharacterResponse
        {
            Abilities = ability.Select(a => new AbilityInfoResponse
            {
                Id = a.Id,
                Title = a.Title,
                ImagePath = a.ImagePath,
                Description = a.Description,
                IsHealing = a.IsHealing
            }).ToArray()
        });
    }
    
    [HttpDelete]
    [ProducesResponseType<StatusResponse>(200)]
    public async Task<IActionResult> DeleteAbility([FromQuery] Guid abilityId)
    {
        await _abilityService.DeleteAsync(abilityId);
       
        return Ok(new StatusResponse
        {
            IsSuccess = true,
            Message = "Ability deleted"
        });
    }
}