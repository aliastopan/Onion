using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((_, services) =>
{
    services.AddRouteEndpoints(Assembly.GetExecutingAssembly());
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouteEndpoints();

app.Run();