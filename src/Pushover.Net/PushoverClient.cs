using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pushover.Net;

internal class PushoverClient : IPushoverClient
{
    private const string _pushoverApiBaseUrl = "https://api.pushover.net";
    private const string _pushoverApiSendMessageUrl = $"{_pushoverApiBaseUrl}/1/messages.json";
    private const string _pushoverApiValidateUserUrl = $"{_pushoverApiBaseUrl}/1/users/validate.json";

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

    public async Task<PushoverUserValidationResponse> ValidateUserOrGroupAsync(string userKey, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        requestBuilder.AddIfNotNullOrEmpty("user", userKey);
        requestBuilder.AddIfNotNullOrEmpty("device", deviceId);

        return await PostToApiAsync<PushoverUserValidationResponse>(_pushoverApiValidateUserUrl, requestBuilder.Content, cancellationToken);
    }

    private async Task<T> PostToApiAsync<T>(string url, HttpContent content, CancellationToken cancellationToken)
        where T: PushoverResponse, new()
    {
        HttpClient client = _httpClientFactory.CreateClient();
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            string debug = await content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Sending request to Pushover {Url}: {Debug}", url, debug);
        }

        HttpResponseMessage response = await client.PostAsync(url, content, cancellationToken);
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Response from Pushover {Url}: {Response}", url, responseContent);
        }
        T responseObject = JsonSerializer.Deserialize<T>(responseContent) ?? new();

        try
        {
            response.EnsureSuccessStatusCode();
            return responseObject;
        }
        catch (Exception ex)
        {
            throw new PushoverSendFailedException(responseObject, ex);
        }
    }

    public async Task<PushoverSendMessageResponse> SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken)
    {
        var messageBuilder = new PushoverMessageBuilder();
        configureMessage(messageBuilder);
        messageBuilder.AddDefaultUserIfNeeded(_options.DefaultUserKey);
        messageBuilder.Validate();

        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        messageBuilder.ConfigureRequest(requestBuilder);

        return await PostToApiAsync<PushoverSendMessageResponse>(_pushoverApiSendMessageUrl, requestBuilder.Content, cancellationToken);
    }
}