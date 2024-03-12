namespace Api.Controllers.Ability.Responses;

public class AbilitiesFromCharacterResponse
{
    public required AbilityInfoResponse[] Abilities { get; init; }
}