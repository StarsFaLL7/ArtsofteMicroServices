using ProjectCore.Api.Responses;

namespace Api.Controllers.Ability.Responses;

public class AbilityStatusResponse : StatusResponse
{
    public required Guid AbilityGuid { get; init; }
}