using RhythmRivals.Common.Enums;

namespace RhythmRivals.Common.Models;
public class Game
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int TotalRounds { get; set; }

    public GameState State { get; set; } = GameState.Waiting;

    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();

    public Round? CurrentRoundEntity => Rounds.FirstOrDefault();
    public int CurrentRound => (TotalRounds - Rounds.Count) + 1;
}
