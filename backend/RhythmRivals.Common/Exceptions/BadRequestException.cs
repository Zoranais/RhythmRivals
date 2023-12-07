using RhythmRivals.Common.Exceptions.Abstract;
using System.Net;

namespace RhythmRivals.Common.Exceptions;
public class BadRequestException : RequestException
{
    private const HttpStatusCode STATUS_CODE = HttpStatusCode.BadRequest;
    public BadRequestException(string? message) : base(STATUS_CODE, message)
    {
    }
}
