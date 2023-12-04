using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Jobs;
using RhythmRivals.Common.Constants;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.Enums;
using RhythmRivals.Common.Exceptions;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Services;
public class GameService : IGameService
{
    private readonly IMusicService _musicService;
    private readonly IGameStorage _gameStorage;
    private readonly IHubContext<GameHub> _hub;
    private readonly IMapper _mapper;
    private readonly ISchedulerFactory _schedulerFactory;

    public GameService(
        IGameStorage gameStorage, 
        IMusicService musicService, 
        IHubContext<GameHub> hub, 
        IMapper mapper,
        ISchedulerFactory schedulerFactory)
    {
        _gameStorage = gameStorage;
        _musicService = musicService;
        _hub = hub;
        _mapper = mapper;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<string> CreateGame(CreateGameDto dto)
    {
        var game = _gameStorage.CreateGame();
        game.RoundCount = dto.RoundCount;
        game.Name = dto.Name;

        await InitGame(game, dto.SpotifyUrl);

        return game.Id;
    }

    private async Task InitGame(Game game, string playlistUrl)
    {
        var tracks = await _musicService.GetTracks(playlistUrl);

        for (int i = 0; i < game.RoundCount; i++)
        {
            var round = new Round();

            var randomTracks = Random.Shared.GetItems(tracks.ToArray(), 4);
            round.Answers = randomTracks.Select(x => x.Name).ToList();
            
            var correctAnswer = Random.Shared.GetItems(randomTracks, 1).First();

            round.CorrectAnswer = correctAnswer.Name;
            round.PreviewUrl = correctAnswer.PreviewUrl;

            game.Rounds.Add(round);
        }
    }

    public async Task SubmitAnswer(Answer answer)
    {
        var game = _gameStorage.GetGame(answer.GameId) ?? throw new NotFoundException(nameof(Game), answer.GameId);
        var player = game.Players.FirstOrDefault(x => x.Name == answer.PlayerName) ?? throw new NotFoundException(nameof(Player), answer.PlayerName);
        var currentRound = game.CurrentRound ?? throw new BadRequestException("You can't answer when the round is not in progress.");

        if(!currentRound.SubmitedAnswers.Any(x => x.PlayerName == player.Name)) 
        {
            currentRound.SubmitedAnswers.Add(answer);

            if(currentRound.SubmitedAnswers.Count == game.Players.Count())
            {
                var scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.UnscheduleJob(new TriggerKey($"{game.Id}-areveal"));
                await RevealResults(game.Id);
            }
        }
    }

    public async Task StartGame(string gameId)
    {
        var game = _gameStorage.GetGame(gameId) ?? throw new NotFoundException(nameof(Game), gameId);
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
        var game = _gameStorage.GetGame(gameId) ?? throw new NotFoundException(nameof(Game), gameId);
        var roundDto = _mapper.Map<RoundDto>(game.CurrentRound);

        await _hub.Clients.Group(gameId).SendAsync("DistributeQuestion", roundDto);

        // Schedule reveal
        await Schedule(RevealAnswerJob.Create(gameId,
            TimeSpan.FromSeconds(GameConstants.REVEAL_DELAY_IN_SECONDS)));
    }

    public async Task RevealResults(string gameId)
    {
        var game = _gameStorage.GetGame(gameId) ?? throw new NotFoundException(nameof(Game), gameId);
        var round = game.CurrentRound ?? throw new NotFoundException(nameof(Round));

        var correctAnswer = round.CorrectAnswer;
        var answers = round.SubmitedAnswers
            .OrderBy(x => x.AnsweredAt)
            .Where(x => x.Value == correctAnswer)
            .ToArray();

        for (int i = 0; i < answers.Length; i++)
        {
            var answer = answers[i];
            var player = game.Players.FirstOrDefault(x => x.Name == answer.PlayerName) 
                ?? throw new NotFoundException(nameof(Player), answer.PlayerName);

            player.Score += CalculateScore(i + 1);
        }

        game.Rounds.Remove(game.CurrentRound);

        var playerDtos = _mapper.Map<IEnumerable<PlayerDto>>(game.Players);
        await _hub.Clients.Group(gameId).SendAsync("RevealAnswer", correctAnswer, playerDtos);

        if(game.CurrentRound == null)
        {
            await EndGame(gameId);
            return;
        }

        await Schedule(DistributeQuestionJob.Create(gameId,
            TimeSpan.FromSeconds(GameConstants.DISTRIBUTE_DELAY_IN_SECONDS)));
    }

    public async Task EndGame(string gameId)
    {
        var game = _gameStorage.GetGame(gameId) ?? throw new NotFoundException(nameof(Game), gameId);

        game.State = GameState.Ended;
        await _hub.Clients.Group(gameId).SendAsync("GameEnded");
    }

    private int CalculateScore(int answerNum)
    {
        return GameConstants.BASE_SCORE / answerNum;
    }

    private async Task Schedule((IJobDetail jobDetail, ITrigger trigger) jobInfo)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(jobInfo.jobDetail, jobInfo.trigger);
    }
}
