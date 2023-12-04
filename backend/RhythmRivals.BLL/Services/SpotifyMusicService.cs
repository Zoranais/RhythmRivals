using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.DTO.SpotifyDtos;
using RhythmRivals.Common.Exceptions;

namespace RhythmRivals.BLL.Services;
public class SpotifyMusicService : IMusicService
{
    private const string API_URL = "https://api.spotify.com/v1/playlists";
    private readonly IHttpService _httpService;
    private readonly AccessTokenStorage _tokenStorage;

    public SpotifyMusicService(IHttpService httpService, AccessTokenStorage tokenStorage)
    {
        _httpService = httpService;
        _tokenStorage = tokenStorage;
    }
    public async Task<ICollection<TrackObjectDto>> GetTracks(string playlistUrl)
    {
        var token = await _tokenStorage.GetAccessToken();
        var id = playlistUrl.Replace("https://open.spotify.com/playlist/", string.Empty);
        id = id.Split('?').First();

        PlaylistDto? playlist = null;
        List<TrackObjectDto> tracks = new();

        do
        {
            playlist = await _httpService.SendAsync<object, PlaylistDto>(
                playlist?.Next ?? $"{API_URL}/{id}/tracks?limit=50", 
                null,
                HttpMethod.Get,
                new KeyValuePair<string, string>("Authorization", $"{token.TokenType} {token.AccessToken}"));

            if(playlist is not null)
            {
                tracks.AddRange(playlist.Items.Select(x => x.Track));
            }
        } while (playlist?.Next is not null);

        if (playlist == null || playlist.Items.Count == 0)
        {
            throw new BadRequestException("Invalid playlist url or empty playlist");
        }

        return tracks
            .Where(x => x.PreviewUrl != null)
            .ToList();
    }
}
