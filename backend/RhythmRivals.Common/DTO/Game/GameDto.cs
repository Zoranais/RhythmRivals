using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RhythmRivals.Common.DTO.Game;
public class GameDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RoundCount { get; set; }
    // public int CurrentRound { get; set; } // TEMP REMOVED, CHANGE MAPPING PROFILE ON ADDING
    public ICollection<PlayerDto> Players { get; set; } = new List<PlayerDto>();
}
