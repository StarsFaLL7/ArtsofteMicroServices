namespace IdentityConnectionLib.DtoModels.CreateNotifications;

public class CreateNotificationRequest
{
    /// <summary>
    /// Id пользователя, которому пришло уведомление
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Заголовок уведомления
    /// </summary>
    public required string Title { get; init; }
    
    /// <summary>
    /// Основной текст уведомления
    /// </summary>
    public required string Content { get; init; }
}