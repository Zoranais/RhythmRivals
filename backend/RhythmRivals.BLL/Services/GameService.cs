using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using RhythmRivals.BLL.Helpers;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Jobs;
using RhythmRivals.BLL.Services.Abstract;
using RhythmRivals.Common.Constants;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.DTO.SpotifyDtos;
using RhythmRivals.Common.Enums;
using RhythmRivals.Common.Exceptions;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Services;
public class GameService : BaseService, IGameService
{
    private readonly IMusicService _musicService;
    private readonly IGameStorage _gameStorage;
    private readonly ISchedulerFactory _schedulerFactory;

    public GameService(
        IGameStorage gameStorage,
        IMusicService musicService,
        IHubContext<GameHub> hub,
        IMapper mapper,
        ISchedulerFactory schedulerFactory): base(mapper, hub)
    {
        _gameStorage = gameStorage;
        _musicService = musicService;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<string> CreateGame(CreateGameDto dto)
    {
        var game = _gameStorage.CreateGame();
        game.TotalRounds = dto.RoundCount;
        game.Name = dto.Name;

        await InitGame(game, dto.SpotifyUrl);

        await Schedule(DestroyInactiveGameJob
            .Create(game.Id, TimeSpan.FromMinutes(10)));

        return game.Id;
    }

    public async Task SubmitAnswer(Answer answer)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(answer.GameId));
        var player = NotFoundException.ThrowIfNull(game.Players.FirstOrDefault(x => x.Name == answer.PlayerName));
        var currentRound = game.CurrentRoundEntity
            ?? throw new BadRequestException("You can't answer when the round is not in progress.");

        if (!currentRound.SubmitedAnswers.Any(x => x.PlayerName == player.Name))
        {
            currentRound.SubmitedAnswers.Add(answer);

            if (currentRound.SubmitedAnswers.Count == game.Players.Count())
            {
                var scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.UnscheduleJob(TriggerKeyHelper.CreateRevealAnswerKey(game.Id));
                await RevealResults(game.Id);
            }
        }
    }

    public async Task StartGame(string gameId)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));
        if (game.Players.Count() < 2)
        {
            throw new BadRequestException("ZeroFriendsIssue(((");
        }

        if (game.State != GameState.Waiting)
        {
            throw new BadRequestException("Invalid game state");
        }

        game.State = GameState.Running;

        // Call question distribution backround proccess
        await Schedule(DistributeQuestionJob.Create(gameId,
            TimeSpan.FromSeconds(GameConstants.DISTRIBUTE_DELAY_IN_SECONDS)));

        await _hub.Clients.Group(gameId).SendAsync("GameStarting");
    }

    public async Task DistributeQuestion(string gameId)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));
        var roundDto = _mapper.Map<RoundDto>(game.CurrentRoundEntity);

        await _hub.Clients.Group(gameId).SendAsync("DistributeQuestion", roundDto);

        // Schedule reveal
        await Schedule(RevealAnswerJob.Create(gameId,
            TimeSpan.FromSeconds(GameConstants.REVEAL_DELAY_IN_SECONDS)));
    }

    public async Task RevealResults(string gameId)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));
        var round = NotFoundException.ThrowIfNull(game.CurrentRoundEntity);

        var correctAnswer = round.CorrectAnswer;
        var answers = round.SubmitedAnswers
            .OrderBy(x => x.AnsweredAt)
            .Where(x => x.Value == correctAnswer);

        UpdatePlayerScores(game, answers);

        game.Rounds.Remove(round);

        var playerDtos = _mapper.Map<IEnumerable<PlayerDto>>(game.Players);
        await _hub.Clients.Group(gameId).SendAsync("RevealAnswer", correctAnswer, playerDtos);

        if (game.CurrentRoundEntity != null)
        {
            await Schedule(DistributeQuestionJob.Create(gameId,
                TimeSpan.FromSeconds(GameConstants.DISTRIBUTE_DELAY_IN_SECONDS)));
            return;
        }

        await EndGame(gameId);
    }

    public async Task EndGame(string gameId)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));

        game.State = GameState.Ended;
        await _hub.Clients.Group(gameId).SendAsync("GameEnded");
    }

    private async Task InitGame(Game game, string playlistUrl)
    {
        var tracks = (await _musicService.GetTracks(playlistUrl)).ToArray();

        var trackPool = new List<TrackObjectDto>(tracks)
            .OrderBy(x => Random.Shared.Next())
            .ToList();

        for (int i = 0; i < game.TotalRounds; i++)
        {
            var round = new Round();
            if(trackPool.Count == 0) 
            {
                trackPool =
                [..new List<TrackObjectDto>(tracks)
                    .OrderBy(x => Random.Shared.Next())];
            }
            var correctAnswer = trackPool.First();
            var randomTracks = GetRandomUniqueTracks(tracks, correctAnswer, 4);

            trackPool.Remove(correctAnswer);

            round.Answers = randomTracks
                .Select(x => x.Name)
                .OrderBy(x => Random.Shared.Next())
                .ToList();

            round.CorrectAnswer = correctAnswer.Name;
            round.PreviewUrl = correctAnswer.PreviewUrl;

            game.Rounds.Add(round);
        }
    }

    private List<TrackObjectDto> GetRandomUniqueTracks(TrackObjectDto[] tracks, TrackObjectDto correctAnswer, int count)
    {
        var randomTracks = new List<TrackObjectDto>(count) { correctAnswer };

        while (randomTracks.Count < count)
        {
            var item = tracks[Random.Shared.Next(0, tracks.Length)];

            if (!randomTracks.Contains(item))
            {
                randomTracks.Add(item);
            }
        }

        return randomTracks;
    }

    private void UpdatePlayerScores(Game game, IEnumerable<Answer> correctAnswers)
    {
        int answerCount = 1;
        foreach (var answer in correctAnswers)
        {
            var player = NotFoundException.ThrowIfNull(game.Players.FirstOrDefault(x => x.Name == answer.PlayerName));

            player.Score += CalculateScore(answerCount++, game.CurrentRound);
        }
    }

    private int CalculateScore(int answerNum, int round)
    {
        var roundModifier = 1 + round / 10.0;
        var answerNumModifier = 1 + answerNum / 10.0;

        return (int)(GameConstants.BASE_SCORE / answerNumModifier * roundModifier);
    }

    private async Task Schedule((IJobDetail jobDetail, ITrigger trigger) jobInfo)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(jobInfo.jobDetail, jobInfo.trigger);
    }
}
