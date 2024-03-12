using Domain.Interfaces;
using ProjectCore.Domain;

namespace Domain.Entities;

/// <summary>
/// Предмет в инвентаре персонажа
/// </summary>
public class InventoryItem : BaseDomainEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор пекрсонажа, у кого есть данный предмет
    /// </summary>
    public required Guid CharacterId { get; set; }
    
    /// <summary>
    /// Название предмета
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Кол-во единиц в инвентаре
    /// </summary>
    public int Count
    {
        get => _count;
        set => _count = value < 0 ? 0 : value;
    }
    private int _count;
    
    /// <summary>
    /// Описание предмета
    /// </summary>
    public required string Description { get; set; }


    public async Task SaveAsync(IStoreInventoryItem storeInventoryItem)
    {
        await storeInventoryItem.SaveAsync(this);
    }
}