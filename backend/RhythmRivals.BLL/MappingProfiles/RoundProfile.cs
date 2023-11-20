using AutoMapper;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.MappingProfiles;
public class RoundProfile: Profile
{
    public RoundProfile()
    {
        CreateMap<Round, RoundDto>();
    }
}
