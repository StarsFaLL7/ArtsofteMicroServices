using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Набор навыков персонажа, значение свойства представляет уровень владения (0 - не владеет)
/// </summary>
public class SkillSet : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Акробатика
    /// </summary>
    public required int Acrobatics { get; set; }
    
    /// <summary>
    /// Обращение с животными
    /// </summary>
    public required int AnimalHandling { get; set; }
    
    /// <summary>
    /// Магия
    /// </summary>
    public required int Arcana { get; set; }
    
    /// <summary>
    /// Атлетика
    /// </summary>
    public required int Athletics { get; set; }
    
    /// <summary>
    /// Обман
    /// </summary>
    public required int Deception { get; set; }
    
    /// <summary>
    /// История
    /// </summary>
    public required int History { get; set; }
    
    /// <summary>
    /// Проницательность
    /// </summary>
    public required int Insight { get; set; }
    
    /// <summary>
    /// Запугивание
    /// </summary>
    public required int Intimidation { get; set; }
    
    /// <summary>
    /// Расследование
    /// </summary>
    public required int Investigation { get; set; }
    
    /// <summary>
    /// Медицина
    /// </summary>
    public required int Medicine { get; set; }
    
    /// <summary>
    /// Природа
    /// </summary>
    public required int Nature { get; set; }
    
    /// <summary>
    /// Восприятие
    /// </summary>
    public required int Perception { get; set; }
    
    /// <summary>
    /// Выступление
    /// </summary>
    public required int Performance { get; set; }
    
    /// <summary>
    /// Убеждение
    /// </summary>
    public required int Persuasion { get; set; }
    
    /// <summary>
    /// Религия
    /// </summary>
    public required int Religion { get; set; }
    
    /// <summary>
    /// Ловкость рук
    /// </summary>
    public required int SleightOfHands { get; set; }
    
    /// <summary>
    /// Скрытность
    /// </summary>
    public required int Stealth { get; set; }
    
    /// <summary>
    /// Выживание
    /// </summary>
    public required int Survival { get; set; }
    
    public async Task SaveAsync(IStoreSkillSet storeSkillSet)
    {
        await storeSkillSet.SaveAsync(this);
    }
}