namespace CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;

public class GetCharacterInfoResponse
{
    public required Guid CharacterId { get; set; }
    
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
}