using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RhythmRivals.Common.DTO.SpotifyDtos;
public class TrackObjectDto
{
    public string Name { get; set; }
    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; }
    public ICollection<ArtistObjectDto> Artists { get; set; } = new List<ArtistObjectDto>();

}