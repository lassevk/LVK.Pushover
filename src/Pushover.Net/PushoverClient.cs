using Microsoft.Extensions.Options;

namespace Pushover.Net;

internal class PushoverClient : IPushoverClient
{
    private const string _pushoverApiUrl = "https://api.pushover.net/1/messages.json";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly PushoverOptions _options;

    public PushoverClient(IOptions<PushoverOptions> options, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _options.Validate();
    }

    public async Task SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken)
    {
        var messageBuilder = new PushoverMessageBuilder();
        configureMessage(messageBuilder);
        messageBuilder.AddDefaultUserIfNeeded(_options.DefaultUserKey);
        messageBuilder.Validate();

        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.Add("token", _options.ApiToken);
        messageBuilder.ConfigureRequest(requestBuilder);

        HttpClient client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.PostAsync(_pushoverApiUrl, requestBuilder.Content, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}