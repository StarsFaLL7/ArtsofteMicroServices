namespace CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;

public class AddItemToCharacterResponse
{
    public required Guid TraceId { get; set; }
    
    /// <summary>
    /// Id добавленного предмета
    /// </summary>
    public required Guid ItemId { get; set; }
}