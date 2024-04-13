using CharactersCreaturesConnectionLib.DtoModels.AddItemToCharacter;
using CharactersCreaturesConnectionLib.DtoModels.GetCharacterInfo;
using CharactersCreaturesConnectionLib.DtoModels.GetCreatureInfo;

namespace CharactersCreaturesConnectionLib.Interfaces;

public interface ICharactersCreaturesConnectionService
{
    /// <summary>
    /// Добавить новый предмет в инвентарь персонажа
    /// </summary>
    Task<AddItemToCharacterResponse[]> AddItemToCharacter(AddItemToCharacterRequest[] request);

    /// <summary>
    /// Получить информацию о персонаже
    /// </summary>
    Task<GetCharacterInfoResponse[]> GetCharacterInfo(GetCharacterInfoRequest request);
    
    /// <summary>
    /// Получить информацию о существе
    /// </summary>
    Task<GetCreatureInfoResponse[]> GetCreatureInfo(GetCreatureInfoRequest request);
}