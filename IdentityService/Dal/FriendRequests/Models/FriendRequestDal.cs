using System.ComponentModel.DataAnnotations.Schema;
using IdentityDal.Notifications.Models;
using IdentityDal.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityDal.FriendRequests.Models;

/// <summary>
/// Запрос, находящийся в ожидании, на добавления в друзья от одного пользователя другому
/// </summary>
[PrimaryKey("SenderId", "RecipientId")]
public class FriendRequestDal
{
    public required Guid SenderId { get; set; }
    [ForeignKey("SenderId")]
    public UserDal SenderUser { get; set; }
    
    public required Guid RecipientId { get; set; }
    [ForeignKey("RecipientId")]
    public UserDal RecipientUser { get; set; }
    public required DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор уведомления, которое создаётся с запросом на добавление в друзья
    /// </summary>
    public required Guid NotificationId { get; set; }
    [ForeignKey("NotificationId")]
    public NotificationDal Notification { get; set; }
}