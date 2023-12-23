using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using RhythmRivals.BLL.Hubs;

namespace RhythmRivals.BLL.Services.Abstract;
public abstract class BaseService
{
    private protected readonly IMapper _mapper;
    private protected readonly IHubContext<GameHub> _hub;

    protected BaseService(IMapper mapper, IHubContext<GameHub> hub)
    {
        _hub = hub;
        _mapper = mapper;
    }
}
