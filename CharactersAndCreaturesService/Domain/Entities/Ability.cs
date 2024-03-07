using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Способность персонажа
/// </summary>
public class Ability : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор персонажа, владеющего способностью
    /// </summary>
    public required Guid CharacterId { get; init; }
    
    /// <summary>
    /// Название способности
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Путь к изображению для отображения
    /// </summary>
    public required string ImagePath { get; set; }
    
    /// <summary>
    /// Описание способности
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Наносимый урон / лечение
    /// </summary>
    public required string Damage { get; set; }
    
    /// <summary>
    /// true - способность лечит, false - способность наносит урон
    /// </summary>
    public required bool IsHealing { get; set; }
    
    public async Task SaveAsync(IStoreAbility storeAbility)
    {
        await storeAbility.SaveAsync(this);
    }

    public async Task DeleteAsync(IStoreAbility storeAbility)
    {
        await storeAbility.RemoveAsync(this);
    }
}