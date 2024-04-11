using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Игровая сессия, в которой добавлены существа, персонажи и их игроки
/// </summary>
public class GameSession
{
    [Key]
    public required Guid Id { get; set; }
    
    public required Guid CreatorUserId { get; set; }
    
    public required string Title { get; set; }

    public List<CharacterInSession> CharactersInSession { get; set; }
    
    public List<CreatureInSession> CreaturesInSession { get; set; }
}