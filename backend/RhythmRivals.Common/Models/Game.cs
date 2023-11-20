﻿namespace RhythmRivals.Common.Models;
public class Game
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RoundCount { get; set; }
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
    public Round? CurrentRound => Rounds.FirstOrDefault();
}
