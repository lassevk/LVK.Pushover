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
        await _pushoverClient.SendMessageAsync(message => message.WithMessage("Hello world!").WithTimestamp(DateTimeOffset.Now.AddHours(-2)), cancellationToken);
    }
}