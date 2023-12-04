using FluentValidation;
using RhythmRivals.Common.DTO.Game;

namespace RhythmRivals.WebAPI.Validators;

public class CreateGameDtoValidator: AbstractValidator<CreateGameDto>
{
    public CreateGameDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(18);

        RuleFor(x => x.RoundCount)
            .LessThanOrEqualTo(30)
            .GreaterThanOrEqualTo(3);

        RuleFor(x => x.SpotifyUrl)
            .Must(x => x.StartsWith("https://open.spotify.com/playlist/"));
    }
}
