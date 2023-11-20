using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RhythmRivals.Common.DTO.SpotifyDtos;
public class AccessTokenDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set;}
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}
