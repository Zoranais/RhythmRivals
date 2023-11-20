using RhythmRivals.Common.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.Common.Exceptions;
public class BadRequestException : RequestException
{
    private const HttpStatusCode STATUS_CODE = HttpStatusCode.BadRequest;
    public BadRequestException(string? message) : base(STATUS_CODE, message)
    {
    }
}
