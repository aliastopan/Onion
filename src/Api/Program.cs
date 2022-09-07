using System.Reflection;
using Onion.Api.Logging;
using Onion.Api.Middleware;
using Onion.Api.Security;
using Onion.Application;
using Onion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((context, logging) =>
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .Filter.ByExcluding(x => x.MessageTemplate.Text.Contains(LogMessages.TokenValidationFailed))
        .CreateLogger();

    logging.ClearProviders();
    logging.AddSerilog();
});

builder.Host.ConfigureServices((context, services) =>
{
    services.AddApplicationServices();
    services.AddInfrastructureServices(context.Configuration);
    services.AddRouteEndpoints(Assembly.GetExecutingAssembly());
    services.AddJwtAuthentication();
    services.AddJwtAuthorization();
});

var app = builder.Build();
app.Init();

app.UseMiddleware<ExceptionGuard>();
app.UseHttpsRedirection();
app.UseRouteEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();