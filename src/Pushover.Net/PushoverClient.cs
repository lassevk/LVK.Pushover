using System.Text.Json;

using Microsoft.Extensions.Options;

namespace Pushover.Net;

internal class PushoverClient : IPushoverClient
{
    private const string _pushoverApiBaseUrl = "https://api.pushover.net";
    private const string _pushoverApiSendMessageUrl = $"{_pushoverApiBaseUrl}/1/messages.json";
    private const string _pushoverApiValidateUserUrl = $"{_pushoverApiBaseUrl}/1/users/validate.json";
    private const string _pushoverApiReceiptsBaseUrl = $"{_pushoverApiBaseUrl}/1/receipts";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly PushoverOptions _options;

    public PushoverClient(IOptions<PushoverOptions> options, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = (options ?? throw new ArgumentNullException(nameof(options))).Value;
        _options.Validate();
    }

    // todo: cancel retries

    public async Task<PushoverUserValidationResponse> ValidateUserOrGroupAsync(string userKey, string? deviceId = null, CancellationToken cancellationToken = default)
    {
        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        requestBuilder.AddIfNotNullOrEmpty("user", userKey);
        requestBuilder.AddIfNotNullOrEmpty("device", deviceId);

        return await SendRequestAsync<PushoverUserValidationResponse>(client => client.PostAsync(_pushoverApiValidateUserUrl, requestBuilder.Content, cancellationToken), cancellationToken);
    }

    public async Task<PushoverReceiptStatusResponse> GetReceiptStatusAsync(string receiptId, CancellationToken cancellationToken = default)
    {
        string url = $"{_pushoverApiReceiptsBaseUrl}/{receiptId}.json?token={_options.ApiToken}";
        return await SendRequestAsync<PushoverReceiptStatusResponse>(client => client.GetAsync(url, cancellationToken), cancellationToken);
    }

    public async Task<PushoverCancelRetriesResponse> CancelRetriesAsync(string receiptId, CancellationToken cancellationToken = default)
    {
        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        string url = $"{_pushoverApiReceiptsBaseUrl}/{receiptId}/cancel.json";

        return await SendRequestAsync<PushoverCancelRetriesResponse>(client => client.PostAsync(url, requestBuilder.Content, cancellationToken), cancellationToken);
    }

    public async Task<PushoverCancelRetriesResponse> CancelRetriesByTagAsync(PushoverMessageTag tag, CancellationToken cancellationToken = default)
    {
        var requestBuilder = new PushoverRequestBuilder();
        requestBuilder.AddIfNotNullOrEmpty("token", _options.ApiToken);
        string url = $"{_pushoverApiReceiptsBaseUrl}/cancel_by_tag/{tag}.json";

        return await SendRequestAsync<PushoverCancelRetriesResponse>(client => client.PostAsync(url, requestBuilder.Content, cancellationToken), cancellationToken);
    }

    private async Task<T> SendRequestAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> sendRequestAsync, CancellationToken cancellationToken)
        where T: PushoverResponse, new()
    {
        HttpClient client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await sendRequestAsync(client);
        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        T responseObject = JsonSerializer.Deserialize<T>(responseContent) ?? new();

        try
        {
            response.EnsureSuccessStatusCode();
            return responseObject;
        }
        catch (Exception ex)
        {
            throw new PushoverApiRequestFailedException(responseObject, ex);
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

        return await SendRequestAsync<PushoverSendMessageResponse>(client => client.PostAsync(_pushoverApiSendMessageUrl, requestBuilder.Content, cancellationToken), cancellationToken);
    }
}