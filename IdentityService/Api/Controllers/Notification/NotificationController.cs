using IdentityApi.Controllers.Notification.Responses;
using IdentityApi.Controllers.Notification.Requests;
using IdentityConnectionLib.DtoModels.CreateNotifications;
using IdentityLogic.Notifications.Interfaces;
using IdentityLogic.Notifications.Models;
using IdentityLogic.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers.Notification;

[Route("api/notification")]
public class NotificationController : ControllerBase
{
    private readonly INotificationManager _notificationManager;
    private readonly IUserLogicManager _userLogicManager;

    public NotificationController(INotificationManager notificationManager, IUserLogicManager userLogicManager)
    {
        _notificationManager = notificationManager;
        _userLogicManager = userLogicManager;
    }
    
    [ProducesResponseType<NotificationsListResponse>(200)]
    [HttpGet]
    public async Task<IActionResult> GetAllFromUserAsync([FromQuery] Guid userId)
    {
        var logicModels = await _notificationManager.GetUserNotificationsAsync(userId);
        var res = new NotificationsListResponse
        {
            UserId = userId,
            NotificationsList = logicModels.Select(n => new NotificationInfoResponse
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                WasRead = n.WasRead,
                CreatedTime = n.CreatedTime
            }).ToArray()
        };
        return Ok(res);
    }
    
    [ProducesResponseType(200)]
    [HttpPut]
    public async Task<IActionResult> MarkAsReadAsync([FromBody] MarkAsReadNotificationsRequest dto)
    {
        await _notificationManager.MarkNotificationsAsReadAsync(dto.NotificationsIds);
        return Ok();
    }
    
    [ProducesResponseType(200)]
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] Guid notificationId)
    {
        await _notificationManager.DeleteNotificationAsync(notificationId);
        return Ok();
    }
    
    [ProducesResponseType(200)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateNotificationRequest dto)
    {
        var logicModel = new NotificationLogic
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Title = dto.Title,
            Content = dto.Content,
            WasRead = false,
            CreatedTime = DateTime.Now
        };
        await _notificationManager.CreateNewNotificationAsync(logicModel);
        return Ok();
    }
    
    [ProducesResponseType<CreateNotificationResponse>(200)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateForApiAsync([FromBody] CreateNotificationRequest[] dto)
    {
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
        return Ok(res);
    }
}