using RhythmRivals.Common.Exceptions.Abstract;
using System.Net;

namespace RhythmRivals.Common.Exceptions;
public class NotFoundException : RequestException
{
    private const HttpStatusCode STATUS_CODE = HttpStatusCode.NotFound;
    public NotFoundException() : base(STATUS_CODE, "Entity does not exist!")
    {
    }

    public NotFoundException(string entityName) : base(STATUS_CODE, $"{entityName} does not exist!")
    {
    }

    public NotFoundException(string entityName, string id): base(STATUS_CODE, $"{entityName} with {id} id does not exist!")
    {
        
    }
}
