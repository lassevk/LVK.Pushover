using Microsoft.Extensions.Options;

namespace Pushover.Net;

internal class PushoverClient : IPushoverClient
{
    private readonly PushoverOptions _options;

    public PushoverClient(IOptions<PushoverOptions> options)
    {
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _options.Validate();
    }
}