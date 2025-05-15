namespace Pushover.Net.ConsoleSandbox;

public class MainApplication
{
    private readonly IPushoverClient _pushoverClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public MainApplication(IPushoverClient pushoverClient, IHttpClientFactory httpClientFactory)
    {
        _pushoverClient = pushoverClient ?? throw new ArgumentNullException(nameof(pushoverClient));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        byte[] image = await GetRandomImageAsync(cancellationToken);
        await _pushoverClient.SendMessageAsync(message => message.WithMessage("Hello world!")
               .WithAttachment("example.jpg", image), cancellationToken);
    }

    private async Task<byte[]> GetRandomImageAsync(CancellationToken cancellationToken)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        return await client.GetByteArrayAsync("https://picsum.photos/200/300.jpg", cancellationToken);
    }
}