namespace RhythmRivals.Common.Models;
public class Round
{
    public ICollection<string> Answers { get; set; } = new List<string>();
    public ICollection<Answer> SubmitedAnswers { get; set; } = new List<Answer>();
    public string PreviewUrl { get; set; }
    public string CorrectAnswer { get; set; }
}