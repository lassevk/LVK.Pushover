namespace LVK.Pushover.ConsoleSandbox;

public class MainApplication
{
    private readonly IPushoverClient _pushoverClient;

    public MainApplication(IPushoverClient pushoverClient)
    {
        _pushoverClient = pushoverClient ?? throw new ArgumentNullException(nameof(pushoverClient));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        PushoverSendMessageResponse response = await _pushoverClient.SendMessageAsync(msg => msg.WithMessage("Hello world!").WithEmergencyPriority(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10))
           .WithTags([new("a", "123")]), cancellationToken);

        Console.WriteLine("Waiting for 30 seconds before cancelling");
        await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);

        Console.WriteLine("Cancelling");
        PushoverCancelRetriesResponse cancelResponse = await _pushoverClient.CancelRetriesByTagAsync(new("a", "123"), cancellationToken);
    }
}