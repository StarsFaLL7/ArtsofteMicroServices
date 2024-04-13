using System.Collections.Concurrent;
using IdentityDal.Notifications.Interfaces;
using IdentityDal.Notifications.Models;

namespace IdentityDal.Notifications;

public class NotificationRepository : INotificationRepository
{
    private readonly PostgresDbContext _dbContext;

    public NotificationRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NotificationDal[]> GetUserNotificationsAsync(Guid userId, bool onlyNew = false)
    {
        return _dbContext.Notifications
            .Where(u => u.UserId == userId && !(onlyNew && u.WasRead))
            .ToArray();
    }

    public async Task MarkNotificationsAsReadAsync(params Guid[] notificationsIds)
    {
        var notifications = _dbContext.Notifications.Where(n => notificationsIds.Contains(n.Id));
        foreach (var notification in notifications)
        {
            notification.WasRead = true;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteNotificationAsync(Guid notificationId)
    {
        var notification = _dbContext.Notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Guid> CreateNewNotificationAsync(NotificationDal notification)
    {
        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
        return notification.Id;
    }
}