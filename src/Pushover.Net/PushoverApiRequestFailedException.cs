namespace Pushover.Net;

public class PushoverApiRequestFailedException : InvalidOperationException
{
    public PushoverApiRequestFailedException(PushoverResponse response, Exception? innerException = null)
        : base(innerException?.Message ?? "Pushover request failed.", innerException)
    {
        Errors = response.Errors.ToArray();
    }

    public string[] Errors { get; private set; }
}