using RhythmRivals.WebAPI.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpotifyHttpClient();
builder.Services.AddSpotifyAuthorizationHttpClient(builder.Configuration);

builder.Services.AddCustomServices();
builder.Services.AddSerilogLogging();
builder.Services.AddEntityMapping();
builder.Services.AddCors();
builder.Services.AddValidation();
builder.Services.AddSignalR();

builder.Services.AddExceptionHandling();
builder.Services.AddQuartzScheduler();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.ConfigureCors(false);
app.UseSerilogRequestLogging();

app.UseMiddleware<RhythmRivals.WebAPI.Middlewares.ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapSignalRHubs();
app.MapApiEndpoints();

app.Run();