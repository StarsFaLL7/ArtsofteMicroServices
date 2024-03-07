using ProjectCore.Api.Responses;

namespace Api.Controllers.OtherResponses.CharacteristicsSet;

public class CharacteristicsSetStatusResponse : StatusResponse
{
    public required CharacteristicsSetInfoResponse Characteristics { get; init; }
}