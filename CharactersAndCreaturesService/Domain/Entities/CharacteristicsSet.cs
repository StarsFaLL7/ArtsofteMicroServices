using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Набор характеристик персонажа или существа
/// </summary>
public class CharacteristicsSet : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Сила
    /// </summary>
    public required int Strength { get; set; }
    
    /// <summary>
    /// Ловкость
    /// </summary>
    public required int Agility { get; set; }
    
    /// <summary>
    /// Выносливость
    /// </summary>
    public required int Endurance { get; set; }
    
    /// <summary>
    /// Мудрость
    /// </summary>
    public required int Wisdom { get; set; }
    
    /// <summary>
    /// Интеллект
    /// </summary>
    public required int Intelligence { get; set; }
    
    /// <summary>
    /// Харизма
    /// </summary>
    public required int Charisma { get; set; }

    public async Task SaveAsync(IStoreCharacteristicsSet characteristicsSetStore)
    {
        await characteristicsSetStore.SaveAsync(this);
    }
}