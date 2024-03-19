namespace IdentityConnectionLib.DtoModels.UserNameList;

public class UsernameIdentityApiRequest
{
    public required Guid[] UserIds { get; init; }
}