using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Существо, находящееся в сессии
/// </summary>
public class CreatureInSession
{
    [Key]
    public required Guid Id { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор существа из коллекции существ игрока, кто создал сессию, в которой находится это существо
    /// </summary>
    public required Guid CreatureId { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор сессии, в которой находится существо
    /// </summary>
    public required Guid SessionId { get; set; }
    [ForeignKey("SessionId")]
    public GameSession Session { get; set; }
    
    public required int Health { get; set; }
    
    public required int Armor { get; set; }
}