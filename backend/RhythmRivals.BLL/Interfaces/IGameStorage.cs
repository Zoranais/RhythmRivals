using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Interfaces;
public interface IGameStorage
{
    Game CreateGame();
    Game? GetGame(string id);
    void DeleteGame(string id);
}
