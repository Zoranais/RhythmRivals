using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Interfaces;
public interface IGameService
{
    Task<string> CreateGame(CreateGameDto dto);
    Task SubmitAnswer(Answer answer);
    Task StartGame(string gameId);
    Task DistributeQuestion(string gameId);
    Task RevealResults(string gameId);
    Task EndGame(string gameId);
}
