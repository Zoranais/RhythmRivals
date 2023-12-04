using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RhythmRivals.BLL.Hubs;
using RhythmRivals.BLL.Interfaces;
using RhythmRivals.Common.DTO.Game;
using RhythmRivals.WebAPI.Validators;

namespace RhythmRivals.WebAPI.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void ConfigureCors(this IApplicationBuilder app, bool isInDevelopment)
    {
        if(isInDevelopment)
        {
            app.UseCors(builder => builder
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Token-Expired")
               .AllowAnyOrigin());
        }
        else
        {
            var origin = app.ApplicationServices.GetRequiredService<IConfiguration>()["AllowedOrigin"]
                ?? throw new ArgumentNullException("AllowedOrigin");

            var test = app.ApplicationServices.GetRequiredService<ILogger<IConfiguration>>();
            test.LogInformation($"Origin: {origin}");

            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Token-Expired")
                .AllowCredentials()
                .WithOrigins(origin, "http://localhost", "https://localhost"));
        }
    }
}
