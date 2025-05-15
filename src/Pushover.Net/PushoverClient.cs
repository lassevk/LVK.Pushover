using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pushover.Net;

internal class PushoverClient : IPushoverClient
{
    private const string _pushoverApiUrl = "https://api.pushover.net/1/messages.json";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<PushoverClient> _logger;
    private readonly PushoverOptions _options;

    public PushoverClient(IOptions<PushoverOptions> options, IHttpClientFactory httpClientFactory, ILogger<PushoverClient> logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _options.Validate();
    }

    // todo: recipts
    // todo: get result from receipt
    // todo: cancel retries
    // todo: validate user or group keys

    public async Task<PushoverResponse> SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken)
    {
        var messageBuilder = new PushoverMessageBuilder();
        configureMessage(messageBuilder);
        messageBuilder.AddDefaultUserIfNeeded(_options.DefaultUserKey);
        messageBuilder.Validate();

        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        messageBuilder.ConfigureRequest(requestBuilder);

        HttpClient client = _httpClientFactory.CreateClient();
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            string debug = await requestBuilder.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Sending message to Pushover: {Debug}", debug);
        }

        HttpResponseMessage response = await client.PostAsync(_pushoverApiUrl, requestBuilder.Content, cancellationToken);
        try
        {
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<PushoverResponse>(responseContent) ?? new();
        }
        catch (Exception ex)
        {
            string errorResponseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            PushoverResponse responseObject = JsonSerializer.Deserialize<PushoverResponse>(errorResponseContent) ?? new();
            throw new PushoverSendFailedException(responseObject, ex);
        }
    }
}