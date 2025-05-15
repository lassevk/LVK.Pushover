// ReSharper disable MemberCanBePrivate.Global
namespace Pushover.Net;

public class PushoverMessageBuilder
{
    private readonly List<string> _recipientKeys = [];
    private string? _message;

    public PushoverMessageBuilder WithRecipient(string userOrGroupKey) => WithRecipients(userOrGroupKey);
    public PushoverMessageBuilder WithRecipients(params ReadOnlySpan<string> userOrGroupKeys)
    {
        _recipientKeys.AddRange(userOrGroupKeys);
        return this;
    }

    public PushoverMessageBuilder WithMessage(string message)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));
        return this;
    }

    internal void AddDefaultUserIfNeeded(string? defaultUserKey)
    {
        if (_recipientKeys.Count == 0 && defaultUserKey is not null)
        {
            _recipientKeys.Add(defaultUserKey);
        }
    }

    internal void Validate()
    {
        if (_recipientKeys.Count > 50)
        {
            throw new InvalidOperationException("Too many recipients, max of 50 allowed when specifying recipient keys.");
        }

        if (_recipientKeys.Count == 0)
        {
            throw new InvalidOperationException("No recipients specified, and no default user configured.");
        }

        if (string.IsNullOrWhiteSpace(_message))
        {
            throw new InvalidOperationException("Message is required.");
        }
    }

    internal void ConfigureRequest(PushoverRequestBuilder builder)
    {
        builder.Add("user", string.Join(",", _recipientKeys));
        builder.Add("message", _message);
    }
}