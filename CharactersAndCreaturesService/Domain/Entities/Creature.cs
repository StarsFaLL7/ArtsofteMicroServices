using Domain.Enums;
using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Модель любого существа DnD (кроме персонажей игроков)
/// </summary>
public class Creature : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор пользователя, кто создал существо
    /// </summary>
    public required Guid UserId { get; set; }
    
    /// <summary>
    /// Путь к изображению существа
    /// </summary>
    public required string ImagePath { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Максимальные очки здоровья
    /// </summary>
    public required int MaxHealth { get; set; }
    
    /// <summary>
    /// Очки здоровья
    /// </summary>
    public required int Health { get; set; }
    
    /// <summary>
    /// Класс доспехов
    /// </summary>
    public required int Armor { get; set; }
  
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; set; }
    
    public Guid CharacteristicsSetId { get; set; }
    /// <summary>
    /// Набор характеристик
    /// </summary>
    public CharacteristicsSet CharacteristicsSet { get; set; }

    /// <summary>
    /// Тип враждебности
    /// </summary>
    public required HostilityType Hostility { get; set; }
    
    public async Task SaveAsync(IStoreCreature storeCreature)
    {
        await storeCreature.SaveAsync(this);
    }
}