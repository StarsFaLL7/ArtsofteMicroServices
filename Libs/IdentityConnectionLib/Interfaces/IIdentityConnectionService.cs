using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityConnectionLib.DtoModels.UserNameList;

namespace IdentityConnectionLib.Interfaces;

public interface IIdentityConnectionService
{
    Task<UsernameIdentityApiResponse> GetUserNameListAsync(UsernameIdentityApiRequest apiRequest);

    Task<CreateNotificationResponse[]> CreateNotificationForUserAsync(CreateNotificationRequest[] apiRequest);
}