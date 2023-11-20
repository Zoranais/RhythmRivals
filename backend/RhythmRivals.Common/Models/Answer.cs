namespace RhythmRivals.Common.Models;
public class Answer
{
    public string? Value { get; set; }
    public string PlayerName { get; set; }
    public string GameId { get; set; }
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

    public Answer(string? value, string name, string gameId)
    {
        Value = value;
        PlayerName = name;
        GameId = gameId;
    }
}
