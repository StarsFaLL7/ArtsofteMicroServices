namespace CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;

public class AddItemToCharacterRequest
{
    public required Guid TraceId { get; set; }
    
    /// <summary>
    /// Уникальный идентификатор персонажа, кому нужно выдать предмет
    /// </summary>
    public required Guid CharacterId { get; set; }
    
    /// <summary>
    /// Название предмета
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Кол-во единиц в инвентаре
    /// </summary>
    public required int Count
    {
        get => _count;
        set => _count = value < 0 ? 0 : value;
    }
    private int _count;
    
    /// <summary>
    /// Описание предмета
    /// </summary>
    public required string Description { get; set; }
}