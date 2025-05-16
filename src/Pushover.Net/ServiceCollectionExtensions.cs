using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Pushover.Net;

/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/> for registering and configuring the <see cref="IPushoverClient"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers and configures the <see cref="IPushoverClient"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to register the client into.
    /// </param>
    /// <param name="configureOptions">
    /// An action that will be called with a configuration object for the <see cref="IPushoverClient"/>.
    /// </param>
    /// <returns>
    /// The <paramref name="services"/>, for fluent interface coding.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> is <c>null</c>.
    /// - or -
    /// <paramref name="configureOptions"/> is <c>null</c>.
    /// </exception>
    public static IServiceCollection AddPushoverClient(this IServiceCollection services, Action<PushoverOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.Configure(configureOptions);
        services.AddTransient<IPushoverClient, PushoverClient>();
        services.AddHttpClient();

        return services;
    }
}