namespace IdentityLogic.Users.Models;

public class UserUpdateLogic
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Описание о себе, которое пишет пользователь
    /// </summary>
    public required string? Description { get; set; }

    /// <summary>
    /// Ссылка на изображение (аватар) пользователя
    /// </summary>
    public required string AvatarUrl { get; set; } = "images/defaultAvatar.jpg";
}