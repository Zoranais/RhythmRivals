using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Jobs;
public class DistributeQuestionJob : IJob
{
    private readonly IGameService _gameService;

    public DistributeQuestionJob(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var gameId = (string)dataMap["gameId"];

        await _gameService.DistributeQuestion(gameId);
    }

    public static (IJobDetail jobDetail, ITrigger trigger) Create(string gameId, TimeSpan delay)
    {
        var job = JobBuilder.Create<DistributeQuestionJob>()
            .Build();

        job.JobDataMap.Put("gameId", gameId);

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{gameId}-qdist")
            .StartAt(DateTime.UtcNow + delay)
            .Build();

        return (job, trigger);
    }
}