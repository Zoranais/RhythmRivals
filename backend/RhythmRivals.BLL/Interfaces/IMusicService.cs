using RhythmRivals.Common.DTO.SpotifyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.BLL.Interfaces;
public interface IMusicService
{
    Task<ICollection<TrackObjectDto>> GetTracks(string playlistUrl);
}
