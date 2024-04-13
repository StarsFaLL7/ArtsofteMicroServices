using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Игровой персонаж внутри запущенной сессии
/// </summary>
public class CharacterInSession
{
    [Key]
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор пользователя, который владеет персонажем
    /// </summary>
    public required Guid UserId { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор сесии, в которой находится персонаж
    /// </summary>
    public required Guid SessionId { get; set; }
    [ForeignKey("SessionId")]
    public GameSession Session { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор персонаж из коллекции персонажей пользователя, кто создаёт сессию
    /// </summary>
    public required Guid CharacterId { get; set; }
}