using System.Net;

namespace RhythmRivals.Common.Exceptions.Abstract;
public abstract class RequestException : Exception
{
    public HttpStatusCode StatusCode { get; }
    protected RequestException(HttpStatusCode statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}
