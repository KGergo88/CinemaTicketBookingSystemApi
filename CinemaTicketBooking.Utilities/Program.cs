using CinemaTicketBooking.Utilities.SeedDatabase;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Reflection;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

ILogger logger = loggerFactory.CreateLogger<Program>();

string? projectName = Assembly.GetExecutingAssembly().GetName().Name;
logger.LogInformation("Starting {ProjectName}", projectName);

var seedDatabaseCommand = new SeedDatabaseCommand();
var SeedDatabaseCommandHandler = new SeedDatabaseCommandHandler(loggerFactory);

seedDatabaseCommand.SetHandler(
    SeedDatabaseCommandHandler.HandleAsync,
    seedDatabaseCommand.ConnectionStringArgument,
    seedDatabaseCommand.JsonPathArgument);

var rootCommand = new RootCommand
{
    seedDatabaseCommand
};

return rootCommand.InvokeAsync(args).Result;
