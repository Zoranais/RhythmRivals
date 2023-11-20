using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.DTO.SpotifyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.BLL.Services;
public class AccessTokenStorage
{
    private const string API_URL = "https://accounts.spotify.com/api/token";

    private readonly string clientId;
    private readonly string clientSecret;
    private readonly IServiceProvider _serviceProvider;

    private AccessTokenDto accessToken;
    public AccessTokenStorage(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        clientId = configuration["Client_Id"] ?? string.Empty;
        clientSecret = configuration["Client_Secret"] ?? string.Empty;
        _serviceProvider = serviceProvider;
    }

    public async Task<AccessTokenDto> GetAccessToken()
    {
        if(accessToken == null)
        {
            return await Renew();
        }
        if((DateTime.UtcNow - accessToken.ReceivedAt.AddSeconds(accessToken.ExpiresIn)).Minutes < 1)
        {
            return await Renew();
        }
        return accessToken;
    }

    public async Task<AccessTokenDto> Renew()
    {
        using var scope = _serviceProvider.CreateScope();
        var httpService = scope.ServiceProvider.GetRequiredService<IHttpService>();

        var message = new HttpRequestMessage(HttpMethod.Post, API_URL);
        message.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))}");
        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        };
        message.Content = new FormUrlEncodedContent(formData);

        return await httpService.SendAsync<AccessTokenDto>(message) ?? throw new ArgumentNullException();
    }
}
