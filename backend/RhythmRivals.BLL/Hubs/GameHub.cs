using Microsoft.AspNetCore.SignalR;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.Models;
using System.Diagnostics.CodeAnalysis;

namespace RhythmRivals.BLL.Hubs;
public class GameHub: Hub
{
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;

    public GameHub(IGameService gameService, IPlayerService playerService)
    {
        _gameService = gameService;
        _playerService = playerService;
    }

    public async Task Join(string gameId, string name)
    {
        if(Context.Items.ContainsKey("game"))
        {
            await Leave();
        }

        var connectionId = Context.ConnectionId;

        var game = await _playerService.AddPlayer(gameId, name);

        Context.Items.Add("game", game.Id);
        Context.Items.Add("name", name);

        await Groups.AddToGroupAsync(connectionId, game.Id);
        await Clients.Client(connectionId).SendAsync("Connected", game);
    }

    public async Task Leave()
    {
        var playerInfo = GetCurrentPlayerInfo();

        if(playerInfo.IsValid())
        {
            await _playerService.RemovePlayer(playerInfo.GameId, playerInfo.Name);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, playerInfo.GameId);
        }

        ClearCurrentPlayerInfo();
    }

    public async Task StartGame()
    {
        var playerInfo = GetCurrentPlayerInfo();

        if(playerInfo.IsValid())
        {
            // If StartGame would also implement RESTART functionality, it would be security issue (every random player would have a possiblity to drop games)
            await _gameService.StartGame(playerInfo.GameId);
        }
    }

    public async Task Respond(string? value)
    {
        var playerInfo = GetCurrentPlayerInfo();
        if(!playerInfo.IsValid()) 
        {
            await Leave();
            return;
        }

        var answer = new Answer(value, playerInfo.Name, playerInfo.GameId);
        await _gameService.SubmitAnswer(answer);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Leave();
        await base.OnDisconnectedAsync(exception);
    }

    private PlayerInfo GetCurrentPlayerInfo()
    {
        Context.Items.TryGetValue("game", out var id);
        Context.Items.TryGetValue("name", out var name);

        return new PlayerInfo(name as string, id as string);
    }

    private void ClearCurrentPlayerInfo()
    {
        Context.Items.Remove("game");
        Context.Items.Remove("name");
    }
}

internal record PlayerInfo(string? Name, string? GameId)
{
    [MemberNotNullWhen(true, nameof(Name), nameof(GameId))]
    public bool IsValid() =>
        Name is not null && GameId is not null;
};