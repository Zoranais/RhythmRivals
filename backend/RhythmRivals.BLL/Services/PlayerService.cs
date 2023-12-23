using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.BLL.Services.Abstract;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.Exceptions;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.Services;
public class PlayerService : BaseService, IPlayerService
{
    private readonly IGameStorage _gameStorage;

    public PlayerService(
        IHubContext<GameHub> hub, 
        IMapper mapper, 
        IGameStorage gameStorage): base(mapper, hub)
    {
        _gameStorage = gameStorage;
    }

    public async Task<GameDto> AddPlayer(string gameId, string name)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));

        if (game.Players.Where(x => x.Name == name).Any())
        {
            throw new BadRequestException("User with this name already joined this game");
        }

        var player = new Player { Name = name, Score = 0 };
        game.Players.Add(player);

        await _hub.Clients.Group(gameId).SendAsync("PlayerJoined", _mapper.Map<PlayerDto>(player));

        return _mapper.Map<GameDto>(game);
    }

    public async Task RemovePlayer(string gameId, string playerName)
    {
        var game = NotFoundException.ThrowIfNull(_gameStorage.GetGame(gameId));
        var player = NotFoundException.ThrowIfNull(game.Players.FirstOrDefault(x => x.Name == playerName));

        game.Players.Remove(player);

        if (game.Players.Any())
        {
            await _hub.Clients.Group(gameId).SendAsync("PlayerDisconnected", player);
        }
        else
        {
            _gameStorage.DeleteGame(gameId);
        }
    }
}
