using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.Common.Exceptions.Abstract;
public abstract class RequestException: Exception
{
    public HttpStatusCode StatusCode { get; }
    protected RequestException(HttpStatusCode statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}
