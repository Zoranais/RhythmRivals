using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Services.HttpClients;
using RhythmRivals.Common.DTO.SpotifyDtos;
using RhythmRivals.Common.Exceptions;

namespace RhythmRivals.BLL.Services;
public class SpotifyMusicService : IMusicService
{
    private readonly AccessTokenStorage _tokenStorage;
    private readonly SpotifyApiService _spototifyApiService;

    public SpotifyMusicService(
        AccessTokenStorage tokenStorage, 
        SpotifyApiService spototifyApiService)
    {
        _tokenStorage = tokenStorage;
        _spototifyApiService = spototifyApiService;
    }
    public async Task<ICollection<TrackObjectDto>> GetTracks(string playlistUrl)
    {
        var token = await _tokenStorage.GetAccessToken();
        var id = playlistUrl.Replace("https://open.spotify.com/playlist/", string.Empty);
        id = id.Split('?').First();

        var tracks = (await _spototifyApiService.GetPlaylistById(id, token))
            .Where(x => x.PreviewUrl != null)
            .ToList();

        if (tracks.Count == 0)
        {
            throw new BadRequestException("Invalid playlist url or empty playlist");
        }

        return tracks;
    }
}
