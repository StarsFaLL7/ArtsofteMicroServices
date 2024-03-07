using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Модель игрового персонажа DnD
/// </summary>
public class Character : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор пользователя, кто создал персонажа
    /// </summary>
    public required Guid UserId { get; set; }
    
    public required Guid SkillSetId { get; set; }
    /// <summary>
    /// Набор значений владения навыками
    /// </summary>
    public SkillSet SkillSetModel { get; set; }

    public required Guid CharacteristicsSetId { get; set; }
    /// <summary>
    /// Набор значений характеристик
    /// </summary>
    public required CharacteristicsSet CharacteristicsSet { get; set; }

    /// <summary>
    /// Имя персонажа
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Путь к изображдению персонажа
    /// </summary>
    public required string ImagePath { get; set; }
    
    /// <summary>
    /// Уровень персонажа
    /// </summary>
    public required int Level { get; set; }
    
    /// <summary>
    /// Кол-во опыта персонажа на текущем уровне
    /// </summary>
    public required int Experience { get; set; }
    
    /// <summary>
    /// Максимальные очки здоровья
    /// </summary>
    public required int MaxHealth { get; set; }
    
    /// <summary>
    /// Очки здоровья персонажа
    /// </summary>
    public required int Health { get; set; }
    
    /// <summary>
    /// Класс доспехов персонажа
    /// </summary>
    public required int Armor { get; set; }
    
    /// <summary>
    /// Раса персонажа
    /// </summary>
    public required string Race { get; set; }
    
    /// <summary>
    /// Класс персонажа
    /// </summary>
    public required string Class { get; set; }
    
    /// <summary>
    /// Характер (темперамент)
    /// </summary>
    public required string Temperament { get; set; }
    
    /// <summary>
    /// Описание внешности
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Предыстория персонажа
    /// </summary>
    public required string History { get; set; }
    
    /// <summary>
    /// Коллекция способностей
    /// </summary>
    public required ICollection<Ability> Abilities { get; set; }

    /// <summary>
    /// Коллекция предметов в инвентаре
    /// </summary>
    public required ICollection<InventoryItem> InventoryItems { get; set; }

    public async Task SaveAsync(IStoreCharacter storeCharacter)
    {
        await storeCharacter.SaveAsync(this);
    }
}