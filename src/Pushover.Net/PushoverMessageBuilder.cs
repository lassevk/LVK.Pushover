using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace Pushover.Net;

[PublicAPI]
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
    private TimeSpan? _ttl;
    private DateTimeOffset? _timestamp;
    private byte[]? _attachment;
    private string? _attachmentName;
    private string? _attachmentType;
    private List<PushoverMessageTag> _tags = [];

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

    public PushoverMessageBuilder WithTimeToLive(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }

    public PushoverMessageBuilder WithTimestamp(DateTimeOffset? timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public PushoverMessageBuilder WithAttachment(FileInfo file) => WithAttachment(file.FullName);
    public PushoverMessageBuilder WithAttachment(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return WithAttachment(Path.GetFileName(filePath), stream);
    }

    public PushoverMessageBuilder WithAttachment(string attachmentName, Stream attachment, string? mimeType = null)
    {
        if (attachment is MemoryStream memoryStream)
        {
            return WithAttachment(attachmentName, memoryStream.ToArray(), mimeType);
        }

        memoryStream = new MemoryStream();
        attachment.CopyTo(memoryStream);
        return WithAttachment(attachmentName, memoryStream.ToArray(), mimeType);
    }

    public PushoverMessageBuilder WithAttachment(string attachmentName, ReadOnlySpan<byte> attachment, string? mimeType = null)
    {
        _attachment = attachment.ToArray();
        _attachmentName = attachmentName;
        _attachmentType = mimeType ?? MimeTypes.GetMimeType(attachmentName);
        return this;
    }

    public PushoverMessageBuilder WithTag(string key, string value) => WithTags(new PushoverMessageTag(key, value));
    public PushoverMessageBuilder WithTags(params PushoverMessageTag[] tags)
    {
        _tags.AddRange(tags);
        return this;
    }

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

        if (_attachment is not null && _attachment.Length > 0)
        {
            if (_attachmentName is null)
            {
                throw new InvalidOperationException("Attachment name is required when specifying an attachment.");
            }
            if (_attachmentType is null)
            {
                throw new InvalidOperationException("Attachment type is required when specifying an attachment.");
            }
        }
    }

    internal void ConfigureRequest(PushoverRequestBuilder builder)
    {
        builder.AddIfNotNullOrEmpty("user", string.Join(",", _recipientKeys));
        builder.AddIfNotNullOrEmpty("message", _message);
        builder.AddIfNotNullOrEmpty("title", _title);

        switch (_messageFormat)
        {
            case PushoverMessageFormat.Plaintext:
                break;

            case PushoverMessageFormat.Monospace:
                builder.AddIfNotNullOrEmpty("monospace", "1");
                break;

            case PushoverMessageFormat.Html:
                builder.AddIfNotNullOrEmpty("html", "1");
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        builder.AddIfNotNullOrEmpty("device", string.Join(",", _deviceIds));
        builder.AddIfNotNullOrEmpty("url", _url);
        builder.AddIfNotNullOrEmpty("url_title", _urlTitle);
        builder.AddIfNotNullOrEmpty("sound", _sound);
        builder.AddIfNotNullOrEmpty("ttl", _ttl != null && _ttl != TimeSpan.Zero ? ((int)_ttl.Value.TotalSeconds).ToString() : null);
        builder.AddIfNotNullOrEmpty("timestamp", _timestamp?.ToUnixTimeSeconds().ToString());

        builder.AddIfNotNullOrEmpty("priority", _priority != PushoverMessagePriority.Default ? ((int)_priority).ToString() : null);;
        builder.AddIfNotNullOrEmpty("retry", _priority == PushoverMessagePriority.Emergency ? ((int)_retryInterval.TotalSeconds).ToString() : null);
        builder.AddIfNotNullOrEmpty("expire", _priority == PushoverMessagePriority.Emergency ? ((int)_expiresAfter.TotalSeconds).ToString() : null);
        builder.AddIfNotNullOrEmpty("callback", _priority == PushoverMessagePriority.Emergency ? _callbackUrl : null);

        builder.AddIfNotNullOrEmpty("tags", _tags.Count > 0 ? string.Join(",", _tags) : null);

        if (_attachment is not null && _attachmentName != null && _attachmentType != null && _attachment.Length > 0)
        {
            builder.AddAttachment(_attachment, _attachmentName, _attachmentType);
        }
    }
}