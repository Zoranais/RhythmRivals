using RhythmRivals.Common.DTO.SpotifyDtos;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace RhythmRivals.BLL.Services.HttpClients;
public class SpotifyApiService
{
    private readonly HttpClient _client;

    public SpotifyApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ICollection<TrackObjectDto>> GetPlaylistById(string id, AccessTokenDto token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", $"{token.TokenType} {token.AccessToken}");
        PlaylistDto? playlist = null;
        List<TrackObjectDto> tracks = new();

        do
        {
            var response = await _client.GetAsync(playlist?.Next ?? $"playlists/{id}/tracks?limit=50");
            playlist = await response.Content.ReadFromJsonAsync<PlaylistDto>();

            if (playlist is not null)
            {
                tracks.AddRange(playlist.Items.Select(x => x.Track));
            }
        }
        while (playlist?.Next is not null);

        return tracks;
    }
}
