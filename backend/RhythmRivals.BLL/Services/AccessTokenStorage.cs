using Microsoft.Extensions.DependencyInjection;
using RhythmRivals.BLL.Services.HttpClients;
using RhythmRivals.Common.DTO.SpotifyDtos;

namespace RhythmRivals.BLL.Services;
public class AccessTokenStorage
{
    private readonly IServiceProvider _serviceProvider;

    private AccessTokenDto accessToken;
    public AccessTokenStorage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<AccessTokenDto> GetAccessToken()
    {
        if (accessToken == null)
        {
            return await Renew();
        }
        if ((DateTime.UtcNow - accessToken.ReceivedAt.AddSeconds(accessToken.ExpiresIn)).Minutes < 1)
        {
            return await Renew();
        }
        return accessToken;
    }

    public async Task<AccessTokenDto> Renew()
    {
        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<SpotifyAuthorizationApiService>();

        return await authService.GetAccessToken() ?? throw new Exception("Invalid Access Token");
    }
}
