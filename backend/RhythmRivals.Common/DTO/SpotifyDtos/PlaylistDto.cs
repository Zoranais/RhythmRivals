namespace RhythmRivals.Common.DTO.SpotifyDtos;
public class PlaylistDto
{
    public ICollection<PlaylistTrackObjectDto> Items { get; set; } = new List<PlaylistTrackObjectDto>();
}
