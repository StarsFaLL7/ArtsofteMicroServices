﻿namespace IdentityLogic.Notifications.Models;

/// <summary>
/// Модель уведомления для слоя Logic
/// </summary>
public class NotificationLogic
{
    public required Guid Id { get; init; }
    
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
    
    /// <summary>
    /// Было ли уведомление уже прочитано
    /// </summary>
    public required bool WasRead { get; set; }
    
    /// <summary>
    /// Дата и время добавления уведомления
    /// </summary>
    public required DateTime CreatedTime { get; init; }
}