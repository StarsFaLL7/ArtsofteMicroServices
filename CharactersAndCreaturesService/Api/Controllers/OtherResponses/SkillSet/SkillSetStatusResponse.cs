using ProjectCore.Api.Responses;

namespace Api.Controllers.OtherResponses.SkillSet;

public class SkillSetStatusResponse : StatusResponse
{
    public required SkillSetInfoResponse SkillSet { get; init; }
}