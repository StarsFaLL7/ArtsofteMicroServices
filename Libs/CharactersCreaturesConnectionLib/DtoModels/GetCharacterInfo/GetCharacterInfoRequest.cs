namespace CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;

public class GetCharacterInfoRequest
{
    /// <summary>
    /// Уникальные идентификаторы персонажей
    /// </summary>
    public required Guid[] CharactersIds { get; set; }
}