// ReSharper disable MemberCanBePrivate.Global

using System.Text.RegularExpressions;

namespace Pushover.Net;

public partial class PushoverMessageBuilder
{
    private readonly List<string> _recipientKeys = [];
    private readonly List<string> _deviceIds = [];
    private string? _message;
    private string? _title;

    [GeneratedRegex("^[a-zA-Z0-9]{30}$")]
    private partial Regex UserOrGroupKeyPattern();

    public PushoverMessageBuilder WithRecipient(string userOrGroupKey) => WithRecipients(userOrGroupKey);

    public PushoverMessageBuilder WithRecipients(params ReadOnlySpan<string> userOrGroupKeys)
    {
        foreach (string userOrGroupKey in userOrGroupKeys)
        {
            ValidationHelper.ValidateUserOrGroupKey(userOrGroupKey);
            _recipientKeys.Add(userOrGroupKey);
        }

        return this;
    }

    public PushoverMessageBuilder WithMessage(string? message)
    {
        _message = ValidationHelper.ValidateMessage(message);
        return this;
    }

    public PushoverMessageBuilder WithTitle(string? title)
    {
        _title = ValidationHelper.ValidateTitle(title);
        return this;
    }

    // todo: attachment
    // todo: attachment_base64
    // todo: attachment_type
    // todo: html
    // todo: priority
    // todo: sound
    // todo: timestamp
    // todo: title
    // todo: ttl
    // todo: url
    // todo: url_title

    public PushoverMessageBuilder WithTargetDevice(string deviceId) => WithTargetDevices(deviceId);
    public PushoverMessageBuilder WithTargetDevices(params ReadOnlySpan<string> deviceIds)
    {
        _deviceIds.AddRange(deviceIds);
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
        builder.Add("device", string.Join(",", _deviceIds));
    }
}