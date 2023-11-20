using FluentValidation;
using Quartz;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Jobs;
using RhythmRivals.BLL.MappingProfiles;
using RhythmRivals.BLL.Services;
using RhythmRivals.WebAPI.ExceptionHandlers;
using RhythmRivals.WebAPI.Validators;
using Serilog;

namespace RhythmRivals.WebAPI.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IHttpService, HttpService>();
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

    public static void AddEntityMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(GameProfile).Assembly);
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(CreateGameDtoValidator));
    }

}
