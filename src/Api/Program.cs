using System.Reflection;
using Onion.Application;
using Onion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((context, logging) =>
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    logging.ClearProviders();
    logging.AddSerilog();
});

builder.Host.ConfigureServices((context, services) =>
{
    services.AddApplicationServices();
    services.AddInfrastructureServices(context.Configuration);
    services.AddRouteEndpoints(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouteEndpoints();

app.Run();