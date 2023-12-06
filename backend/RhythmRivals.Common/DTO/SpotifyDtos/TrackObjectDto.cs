using System.Text.Json.Serialization;

namespace RhythmRivals.Common.DTO.SpotifyDtos;
public class TrackObjectDto
{
    public string Name { get; set; }
    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; }
    public ICollection<ArtistObjectDto> Artists { get; set; } = new List<ArtistObjectDto>();

}