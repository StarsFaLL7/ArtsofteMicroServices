using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CharacterRepository : IStoreCharacter
{
    private readonly ConcurrentDictionary<Guid, Character> _store = new ();

    public async Task<bool> IsExistAsync(Guid characterId)
    {
        return _store.ContainsKey(characterId);
    }

    public async Task SaveAsync(Character character)
    {
        _store.TryAdd(character.Id, character);
    }

    public async Task<Character[]> GetPlayersCharactersAsync(Guid userId)
    {
        return _store.Values.Where(c => c.UserId == userId).ToArray();
    }

    public async Task RemoveAsync(Guid characterId)
    {
        _store.TryRemove(characterId, out _);
    }

    public async Task<Character> GetByIdAsync(Guid id)
    {
        return _store[id];
    }

    public async Task<InventoryItem[]> AddItemAsync(Guid characterId, InventoryItem item)
    {
        var character = _store[characterId];
        character.InventoryItems.ToList().Add(item);
        return character.InventoryItems.ToArray();
    }
}