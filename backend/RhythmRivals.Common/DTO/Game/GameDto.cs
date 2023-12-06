using RhythmRivals.Common.Enums;

namespace RhythmRivals.Common.DTO.Game;
public class GameDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RoundCount { get; set; }
    // public int CurrentRound { get; set; } // TEMP REMOVED, CHANGE MAPPING PROFILE ON ADDING
    public GameState State { get; set; }
    public ICollection<PlayerDto> Players { get; set; } = new List<PlayerDto>();
}
