namespace CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;

public class GetCreatureInfoResponse
{
    public required Guid CreatureId { get; set; }
    
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
}