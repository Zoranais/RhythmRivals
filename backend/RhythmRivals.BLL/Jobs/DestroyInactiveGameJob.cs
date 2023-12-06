using Quartz;
using RhythmRivals.BLL.Interfaces;

namespace RhythmRivals.BLL.Jobs;
public class DestroyInactiveGameJob : IJob
{
    private readonly IGameStorage _gameStorage;

    public DestroyInactiveGameJob(IGameStorage gameStorage)
    {
        _gameStorage = gameStorage;
    }

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var gameId = (string)dataMap["gameId"];

        var game = _gameStorage.GetGame(gameId);

        if (game != null && game.Players.Count == 0)
        {
            _gameStorage.DeleteGame(gameId);
        }

        return Task.CompletedTask;
    }

    public static (IJobDetail jobDetail, ITrigger trigger) Create(string gameId, TimeSpan delay)
    {
        var job = JobBuilder
            .Create<DestroyInactiveGameJob>()
            .Build();

        job.JobDataMap.Put("gameId", gameId);

        var trigger = TriggerBuilder
            .Create()
            .StartAt(DateTime.UtcNow + delay)
            .Build();

        return (job, trigger);
    }
}
