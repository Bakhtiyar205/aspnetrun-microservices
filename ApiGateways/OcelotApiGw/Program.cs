using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
var services = builder.Services;

services.AddOcelot()
    .AddCacheManager(settings =>
     settings.WithDictionaryHandle()
    );
services.AddLogging(configure =>
{
    configure.AddConfiguration(builder.Configuration.GetSection("Logging"));
    configure.AddConsole();
    configure.AddDebug();
});
var app = builder.Build();

app.Logger.LogInformation("Starting OcelotApiGw");
app.MapGet("/", async context => {
    await context.Response.WriteAsync("Hello World!");
});
await app.UseOcelot();
app.Run();
