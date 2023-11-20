using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RhythmRivals.Common.DTO;
public class RoundDto
{
    public ICollection<string> Answers { get; set; }
    public string PreviewUrl { get; set; }
}
