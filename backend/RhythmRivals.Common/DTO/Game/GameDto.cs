﻿using RhythmRivals.Common.Enums;

namespace RhythmRivals.Common.DTO.Game;
public class GameDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RoundCount { get; set; }
    public GameState State { get; set; }
    public ICollection<PlayerDto> Players { get; set; } = new List<PlayerDto>();
}
