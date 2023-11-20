using Quartz;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Jobs;
public class RevealAnswerJob: IJob
{
    private readonly IGameService _gameService;

    public RevealAnswerJob(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var gameId = (string)dataMap["gameId"];

        await _gameService.RevealResults(gameId);
    }

    public static (IJobDetail jobDetail, ITrigger trigger) Create(string gameId, TimeSpan delay)
    {
        var job = JobBuilder.Create<RevealAnswerJob>()
            .Build();

        job.JobDataMap.Put("gameId", gameId);

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{gameId}-areveal")
            .StartAt(DateTime.UtcNow + delay)
            .Build();

        return (job, trigger);
    }
}
