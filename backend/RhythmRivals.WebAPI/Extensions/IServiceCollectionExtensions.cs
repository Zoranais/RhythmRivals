using FluentValidation;
using Quartz;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Jobs;
using RhythmRivals.BLL.MappingProfiles;
using RhythmRivals.BLL.Services;
using RhythmRivals.BLL.Services.HttpClients;
using RhythmRivals.WebAPI.ExceptionHandlers;
using RhythmRivals.WebAPI.Validators;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace RhythmRivals.WebAPI.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IMusicService, SpotifyMusicService>();

        services.AddSingleton<IGameStorage, GameStorage>();
        services.AddSingleton<AccessTokenStorage>();
    }

    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<RequestExceptionHandler>();
        services.AddExceptionHandler<InternalExceptionHandler>();
    }

    public static void AddQuartzScheduler(this IServiceCollection services)
    {
        services.AddQuartz();
        services.AddQuartzHostedService();

        services.AddTransient<DistributeQuestionJob>();
        services.AddTransient<RevealAnswerJob>();
    }

    public static void AddSerilogLogging(this IServiceCollection services)
    {
        services.AddSerilog(o =>
        {
            o.MinimumLevel.Information();
            o.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information);
            o.WriteTo.Console();
            o.WriteTo.Debug();
            //o.WriteTo.Seq(configuration["Serilog:SeqUrl"] ?? throw new ConfigurationNotProvidedException("SeqUrl")); // without SEQ for now
            o.Enrich.FromLogContext();
            o.Enrich.WithMachineName();
        });
    }

    public static void AddSpotifyHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<SpotifyApiService>((options) =>
        {
            options.BaseAddress = new Uri("https://api.spotify.com/v1/");
        });
    }
    public static void AddSpotifyAuthorizationHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<SpotifyAuthorizationApiService>((options) =>
        {
            var clientId = configuration["Client_Id"] ?? string.Empty;
            var clientSecret = configuration["Client_Secret"] ?? string.Empty;
            options.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                $"{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))}");
        });
    }

    public static void AddEntityMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(GameProfile).Assembly);
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(CreateGameDtoValidator));
    }

}
