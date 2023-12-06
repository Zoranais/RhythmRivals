using RhythmRivals.Common.DTO.SpotifyDtos;

namespace RhythmRivals.BLL.Interfaces;
public interface IMusicService
{
    Task<ICollection<TrackObjectDto>> GetTracks(string playlistUrl);
}
