using AutoMapper;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.MappingProfiles;
public class GameProfile: Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDto>();
    }
}
