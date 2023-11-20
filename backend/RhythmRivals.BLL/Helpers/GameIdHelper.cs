using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.BLL.Helpers;
public class GameIdHelper
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    public static string GenerateGameId()
    {
        return $"{GeneratePart()}-{GeneratePart()}";
    }

    private static string GeneratePart()
    {
        var random = Random.Shared;
        return new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
