using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.DTO.Game;

namespace RhythmRivals.WebAPI.Extensions;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapSignalRHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<GameHub>("/GameHub");
    }

    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/game", async ([FromBody] CreateGameDto dto, IGameService gameService, [FromServices] IValidator<CreateGameDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new Common.Exceptions.ValidationException("Validation error occured", validationResult.ToDictionary());
            }

            return Results.Created("api/game", await gameService.CreateGame(dto));
        });
    }
}
