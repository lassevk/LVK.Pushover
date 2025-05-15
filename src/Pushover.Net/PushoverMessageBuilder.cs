// ReSharper disable MemberCanBePrivate.Global

using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Channels;

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
    private string? _sound;
    private PushoverMessagePriority _priority = PushoverMessagePriority.Default;
    private TimeSpan _retryInterval;
    private TimeSpan _expiresAfter;
    private string? _callbackUrl;
    private TimeSpan _ttl;

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

    public PushoverMessageBuilder WithSound(PushoverMessageSound sound)
    {
        _sound = sound.ToString().ToLowerInvariant();
        return this;
    }

    public PushoverMessageBuilder WithCustomSound(string? sound)
    {
        _sound = sound;
        return this;
    }

    public PushoverMessageBuilder WithLowestPriority() => WithPriority(PushoverMessagePriority.Lowest);
    public PushoverMessageBuilder WithLowPriority() => WithPriority(PushoverMessagePriority.Low);
    public PushoverMessageBuilder WithNormalPriority() => WithPriority(PushoverMessagePriority.Normal);
    public PushoverMessageBuilder WithHighPriority() => WithPriority(PushoverMessagePriority.High);
    public PushoverMessageBuilder WithEmergencyPriority(TimeSpan retryInterval, TimeSpan expiresAfter, string? callbackUrl = null) => WithPriority(PushoverMessagePriority.Emergency, retryInterval, expiresAfter, callbackUrl);

    public PushoverMessageBuilder WithPriority(PushoverMessagePriority priority, TimeSpan retryInterval = default, TimeSpan expiresAfter = default, string? callbackUrl = null)
    {
        if (!Enum.IsDefined(priority))
        {
            throw new InvalidOperationException($"Invalid priority: {priority}.");
        }

        if (priority == PushoverMessagePriority.Emergency)
        {
            if (retryInterval == TimeSpan.Zero)
            {
                throw new InvalidOperationException("Retry interval is required for emergency priority.");
            }
            if (retryInterval < TimeSpan.FromSeconds(30))
            {
                throw new InvalidOperationException("Retry interval must be at least 30 seconds for emergency priority.");
            }

            if (expiresAfter == TimeSpan.Zero)
            {
                throw new InvalidOperationException("Expires after is required for emergency priority.");
            }
        }
        else
        {
            if (retryInterval != TimeSpan.Zero)
            {
                throw new InvalidOperationException("Retry interval is not supported for non-emergency priority.");
            }
            if (expiresAfter != TimeSpan.Zero)
            {
                throw new InvalidOperationException("Expires after is not supported for non-emergency priority.");
            }
            if (callbackUrl is not null)
            {
                throw new InvalidOperationException("Callback URL is not supported for non-emergency priority.");
            }
        }

        _priority = priority;
        _retryInterval = retryInterval;
        _expiresAfter = expiresAfter;
        _callbackUrl = callbackUrl;
        return this;
    }

    public PushoverMessageBuilder WithTimeToLive(TimeSpan ttl)
    {
        _ttl = ttl;
        return this;
    }

    // todo: attachment
    // todo: attachment_base64
    // todo: attachment_type
    // todo: timestamp
    // todo: ttl
    // todo: recipts
    // todo: get result from receipt
    // todo: cancel retries
    // todo: validate user or group keys
    // todo: tags?

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
        builder.AddIfNotNullOrEmpty("sound", _sound);
        builder.AddIfNotNullOrEmpty("ttl", _ttl != TimeSpan.Zero ? ((int)_ttl.TotalSeconds).ToString() : null);

        builder.AddIfNotNullOrEmpty("priority", _priority != PushoverMessagePriority.Default ? ((int)_priority).ToString() : null);;
        builder.AddIfNotNullOrEmpty("retry", _priority == PushoverMessagePriority.Emergency ? ((int)_retryInterval.TotalSeconds).ToString() : null);
        builder.AddIfNotNullOrEmpty("expire", _priority == PushoverMessagePriority.Emergency ? ((int)_expiresAfter.TotalSeconds).ToString() : null);
        builder.AddIfNotNullOrEmpty("callback", _priority == PushoverMessagePriority.Emergency ? _callbackUrl : null);
    }
}