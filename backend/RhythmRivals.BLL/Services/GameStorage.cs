using RhythmRivals.BLL.Helpers;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Services;
public class GameStorage : IGameStorage
{
    private readonly IDictionary<string, Game> _storage;

    public GameStorage()
    {
        _storage = new Dictionary<string, Game>();
    }

    public Game CreateGame()
    {
        string id = string.Empty;
        do
        {
            id = GameIdHelper.GenerateGameId();
        }
        while (_storage.ContainsKey(id));

        var game = new Game { Id = id };
        _storage.Add(id, game);

        return game;
    }

    public void DeleteGame(string id)
    {
        if (_storage.ContainsKey(id))
        {
            _storage.Remove(id);
        }
    }

    public Game? GetGame(string id)
    {
        _storage.TryGetValue(id, out var game);
        return game;
    }
}
