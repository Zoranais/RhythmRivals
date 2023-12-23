using RhythmRivals.Common.DTO.SpotifyDtos;
using System.Net.Http.Json;

namespace RhythmRivals.BLL.Services.HttpClients;
public class SpotifyAuthorizationApiService
{
    private readonly HttpClient _client;
    public SpotifyAuthorizationApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<AccessTokenDto?> GetAccessToken()
    {
        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        };

        var response = await _client.PostAsync("token", new FormUrlEncodedContent(formData));

        return await response.Content.ReadFromJsonAsync<AccessTokenDto>();
    }
}
