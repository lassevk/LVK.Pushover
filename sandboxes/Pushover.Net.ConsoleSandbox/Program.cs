using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Pushover.Net;
using Pushover.Net.ConsoleSandbox;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddPushoverClient(options
    => options.WithApiToken(builder.Configuration["Pushover:ApiToken"] ?? throw new InvalidOperationException("API token is not configured."))
       .WithDefaultUser(builder.Configuration["Pushover:DefaultUserKey"] ?? throw new InvalidOperationException("Default user key is not configured.")));

IHost host = builder.Build();
await host.StartAsync();
try
{
    var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, _) => cts.Cancel();

    await ActivatorUtilities.CreateInstance<MainApplication>(host.Services).RunAsync(cts.Token);
}
catch (TaskCanceledException)
{
    // Do nothing
}
finally
{
    await host.StopAsync();
}