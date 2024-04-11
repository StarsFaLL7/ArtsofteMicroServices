using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityLogic.Notifications.Interfaces;
using IdentityLogic.Notifications.Models;
using IdentityLogic.Users.Interfaces;
using MassTransit;

namespace IdentityApi.Saga;

public class CreateNotificationsConsumer : IConsumer<CreateNotificationRequest[]>
{
    private readonly IUserLogicManager _userLogicManager;
    private readonly INotificationManager _notificationManager;

    public CreateNotificationsConsumer(IUserLogicManager userLogicManager, INotificationManager notificationManager)
    {
        _userLogicManager = userLogicManager;
        _notificationManager = notificationManager;
    }
    
    public async Task Consume(ConsumeContext<CreateNotificationRequest[]> context)
    {
        var dto = context.Message;
        var userNames = await _userLogicManager.GetUsernamesByIdsAsync(dto.Select(r => r.UserId).ToArray());
        var ids = new List<Guid>();
        foreach (var notif in dto)
        {
            var logicModel = new NotificationLogic
            {
                Id = Guid.NewGuid(),
                UserId = notif.UserId,
                Title = notif.Title,
                Content = notif.Content,
                WasRead = false,
                CreatedTime = DateTime.Now
            };
            ids.Add(await _notificationManager.CreateNewNotificationAsync(logicModel));
        }

        var res = ids.Select((id, i) => new CreateNotificationResponse
        {
            Id = id,
            Username = userNames[i]
        }).ToArray();
        await context.RespondAsync<CreateNotificationResponse[]>(res);
    }
}