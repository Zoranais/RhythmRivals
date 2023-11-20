using RhythmRivals.Common.DTO;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
