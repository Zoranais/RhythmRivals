using RhythmRivals.Common.DTO.Game;

namespace RhythmRivals.BLL.Interfaces;
public interface IPlayerService
{
    Task<GameDto> AddPlayer(string gameId, string name);
    Task RemovePlayer(string gameId, string playerName);
}
