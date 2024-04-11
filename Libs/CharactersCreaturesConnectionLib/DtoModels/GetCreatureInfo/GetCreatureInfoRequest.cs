namespace CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;

public class GetCreatureInfoRequest
{
    /// <summary>
    /// Уникальные идентификаторы существ
    /// </summary>
    public required Guid[] CreatureIds { get; set; }
}