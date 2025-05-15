namespace Pushover.Net;

public class PushoverSendFailedException : InvalidOperationException
{
    public PushoverSendFailedException(PushoverResponse response, Exception? innerException = null)
        : base(innerException?.Message ?? "Pushover request failed.", innerException)
    {
        Errors = response.Errors.ToArray();
    }

    public string[] Errors { get; private set; }
}