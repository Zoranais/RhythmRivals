namespace RhythmRivals.Common.DTO;
public class ErrorDetailsDto
{
    public string Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; } = null;

    public ErrorDetailsDto(string message, IDictionary<string, string[]>? errors = null)
    {
        Message = message;
        Errors = errors;
    }
}
