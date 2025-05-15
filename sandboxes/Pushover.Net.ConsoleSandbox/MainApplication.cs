namespace Pushover.Net.ConsoleSandbox;

public class MainApplication
{
    private readonly IPushoverClient _pushoverClient;

    public MainApplication(IPushoverClient pushoverClient)
    {
        _pushoverClient = pushoverClient ?? throw new ArgumentNullException(nameof(pushoverClient));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        PushoverReceiptStatusResponse response = await _pushoverClient.GetReceiptStatusAsync("r9wbkx2vfxz4r6wg2f1hiktp26vini", cancellationToken);
    }
}