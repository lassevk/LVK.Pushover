// ReSharper disable MemberCanBePrivate.Global

using System.Text.RegularExpressions;

namespace Pushover.Net;

public partial class PushoverMessageBuilder
{
    private readonly List<string> _recipientKeys = [];
    private readonly List<string> _deviceIds = [];
    private string? _message;
    private PushoverMessageFormat _messageFormat = PushoverMessageFormat.Default;
    private string? _title;
    private string? _url;
    private string? _urlTitle;

    [GeneratedRegex("^[a-zA-Z0-9]{30}$")]
    private partial Regex UserOrGroupKeyPattern();

    public PushoverMessageBuilder WithRecipient(string userOrGroupKey) => WithRecipients(userOrGroupKey);

    public PushoverMessageBuilder WithRecipients(params ReadOnlySpan<string> userOrGroupKeys)
    {
        foreach (string userOrGroupKey in userOrGroupKeys)
        {
            string? key = ValidationHelper.ValidateUserOrGroupKey(userOrGroupKey);
            if (key is not null)
            {
                _recipientKeys.Add(userOrGroupKey);
            }
        }

        return this;
    }

    public PushoverMessageBuilder WithMessage(string? message, PushoverMessageFormat format = PushoverMessageFormat.Default)
    {
        if (!Enum.IsDefined(_messageFormat))
        {
            throw new InvalidOperationException($"Invalid message format: {_messageFormat}.");
        }

        _message = ValidationHelper.ValidateMessage(message);
        _messageFormat = format;
        return this;
    }

    public PushoverMessageBuilder WithTitle(string? title)
    {
        _title = ValidationHelper.ValidateTitle(title);
        return this;
    }

    public PushoverMessageBuilder WithUrl(string? url)
    {
        _url = ValidationHelper.ValidateUrl(url);
        return this;
    }

    public PushoverMessageBuilder WithUrlTitle(string? title)
    {
        _urlTitle = ValidationHelper.ValidateUrlTitle(title);
        return this;
    }

    public PushoverMessageBuilder WithUrlWithTitle(string? url, string? title)
    {
        _url = ValidationHelper.ValidateUrl(url);
        _urlTitle = ValidationHelper.ValidateUrlTitle(title);
        return this;
    }

    // todo: attachment
    // todo: attachment_base64
    // todo: attachment_type
    // todo: priority
    // todo: sound
    // todo: timestamp
    // todo: ttl

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
        builder.AddIfNotNullOrEmpty("user", string.Join(",", _recipientKeys));
        builder.AddIfNotNullOrEmpty("message", _message);
        builder.AddIfNotNullOrEmpty("title", _title);
        builder.AddIfNotNullOrEmpty("html", _messageFormat == PushoverMessageFormat.Html ? "1" : null);
        builder.AddIfNotNullOrEmpty("device", string.Join(",", _deviceIds));
        builder.AddIfNotNullOrEmpty("url", _url);
        builder.AddIfNotNullOrEmpty("url_title", _urlTitle);
    }
}