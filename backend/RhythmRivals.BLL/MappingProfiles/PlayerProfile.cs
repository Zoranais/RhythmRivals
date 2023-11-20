using AutoMapper;
using RhythmRivals.Common.DTO;
using RhythmRivals.Common.Models;

namespace RhythmRivals.BLL.MappingProfiles;
public class PlayerProfile: Profile
{
	public PlayerProfile()
	{
		CreateMap<Player, PlayerDto>();
	}
}
