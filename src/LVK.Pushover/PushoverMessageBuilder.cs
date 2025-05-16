using System.Text.RegularExpressions;

namespace LVK.Pushover;

/// <summary>
/// Represents a builder for constructing Pushover messages with customizable parameters.
/// </summary>
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

    /// <summary>
    /// Adds a single recipient user or group key to the message.
    /// </summary>
    /// <param name="userOrGroupKey">The user or group key of the recipient to whom the message will be sent.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithRecipient(string userOrGroupKey) => WithRecipients(userOrGroupKey);

    /// <summary>
    /// Adds multiple recipient user or group keys to the message.
    /// </summary>
    /// <param name="userOrGroupKeys">An array of user or group keys representing the recipients to whom the message will be sent.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithRecipients(params ReadOnlySpan<string> userOrGroupKeys)
    {
        foreach (string userOrGroupKey in userOrGroupKeys)
        {
            string? key = PushoverValidationHelper.ValidateUserOrGroupKey(userOrGroupKey);
            if (key is not null)
            {
                _recipientKeys.Add(userOrGroupKey);
            }
        }

        return this;
    }

    /// <summary>
    /// Sets the message content and its format for the current Pushover message.
    /// </summary>
    /// <param name="message">The message content to be sent. Must be a valid string.</param>
    /// <param name="format">The format of the message. Defaults to <see cref="PushoverMessageFormat.Default"/>.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithMessage(string? message, PushoverMessageFormat format = PushoverMessageFormat.Default)
    {
        if (!Enum.IsDefined(format))
        {
            throw new ArgumentOutOfRangeException(nameof(format), $"Invalid message format: {format}.");
        }

        _message = PushoverValidationHelper.ValidateMessage(message);
        _messageFormat = format;
        return this;
    }

    /// <summary>
    /// Sets the title of the message.
    /// </summary>
    /// <param name="title">The title to be displayed with the message. It should be a brief, descriptive text.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTitle(string? title)
    {
        _title = PushoverValidationHelper.ValidateTitle(title);
        return this;
    }

    /// <summary>
    /// Sets the URL to be included in the message.
    /// </summary>
    /// <param name="url">The URL to include in the message. This URL must be valid and properly formatted.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithUrl(string? url)
    {
        _url = PushoverValidationHelper.ValidateUrl(url);
        return this;
    }

    /// <summary>
    /// Sets a title for the URL in the message.
    /// </summary>
    /// <param name="title">The title to associate with the URL in the message. It can be null.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithUrlTitle(string? title)
    {
        _urlTitle = PushoverValidationHelper.ValidateUrlTitle(title);
        return this;
    }

    /// <summary>
    /// Adds a URL and its associated title to the message.
    /// </summary>
    /// <param name="url">The URL to be included in the message.</param>
    /// <param name="title">The title associated with the URL.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithUrlWithTitle(string? url, string? title)
    {
        _url = PushoverValidationHelper.ValidateUrl(url);
        _urlTitle = PushoverValidationHelper.ValidateUrlTitle(title);
        return this;
    }

    /// <summary>
    /// Specifies the sound to use for the message notification.
    /// </summary>
    /// <param name="sound">The desired sound for the notification from the available <see cref="PushoverMessageSound"/> options.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithSound(PushoverMessageSound sound)
    {
        _sound = sound.ToString().ToLowerInvariant();
        return this;
    }

    /// <summary>
    /// Sets a custom sound for the push notification.
    /// </summary>
    /// <param name="sound">The name of the custom sound to be used for the notification.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithCustomSound(string? sound)
    {
        _sound = sound;
        return this;
    }

    /// <summary>
    /// Sets the message priority to the lowest possible level.
    /// </summary>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithLowestPriority() => WithPriority(PushoverMessagePriority.Lowest);

    /// <summary>
    /// Sets the message priority to low, indicating that it is less important but not the lowest priority.
    /// </summary>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithLowPriority() => WithPriority(PushoverMessagePriority.Low);

    /// <summary>
    /// Sets the priority of the message to normal.
    /// </summary>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithNormalPriority() => WithPriority(PushoverMessagePriority.Normal);

    /// <summary>
    /// Sets the priority of the message to high.
    /// </summary>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithHighPriority() => WithPriority(PushoverMessagePriority.High);

    /// <summary>
    /// Sets the message priority to emergency and specifies retry interval, expiration time, and an optional callback URL for handling receipt confirmations or cancellations.
    /// Emergency priority messages have to be acknowledged by the recipient within the specified time interval,
    /// and will repeatedly be retried until the expiration time is reached.
    /// </summary>
    /// <param name="retryInterval">The time interval after which the message will be retried if unacknowledged by the recipient.</param>
    /// <param name="expiresAfter">The time period after which the retries for the message will stop.</param>
    /// <param name="callbackUrl">The optional callback URL to handle receipt confirmations or cancellations.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithEmergencyPriority(TimeSpan retryInterval, TimeSpan expiresAfter, string? callbackUrl = null) => WithPriority(PushoverMessagePriority.Emergency, retryInterval, expiresAfter, callbackUrl);

    /// <summary>
    /// Sets the priority of the message along with optional parameters for emergency priority handling.
    /// </summary>
    /// <param name="priority">The priority level of the message, as defined by <see cref="PushoverMessagePriority"/>.</param>
    /// <param name="retryInterval">The time interval to retry sending the message for emergency priority. Must be at least 30 seconds. Ignored for non-emergency priorities.</param>
    /// <param name="expiresAfter">The time duration after which retries for emergency priority expire. Required for emergency priority. Ignored for non-emergency priorities.</param>
    /// <param name="callbackUrl">The URL to be called after all retry attempts are completed for emergency priority. Ignored for non-emergency priorities.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided priority is not a valid <see cref="PushoverMessagePriority"/> value.</exception>
    /// <exception cref="ArgumentException">Thrown if required parameters for emergency priority are missing or invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if retry or expiration parameters are provided for non-emergency priorities.</exception>
    public PushoverMessageBuilder WithPriority(PushoverMessagePriority priority, TimeSpan retryInterval = default, TimeSpan expiresAfter = default, string? callbackUrl = null)
    {
        if (!Enum.IsDefined(priority))
        {
            throw new ArgumentOutOfRangeException(nameof(priority), $"Invalid priority: {priority}.");
        }

        if (priority == PushoverMessagePriority.Emergency)
        {
            if (retryInterval == TimeSpan.Zero)
            {
                throw new ArgumentException("Retry interval is required for emergency priority.", nameof(retryInterval));
            }
            if (retryInterval < TimeSpan.FromSeconds(30))
            {
                throw new ArgumentOutOfRangeException(nameof(retryInterval), "Retry interval must be at least 30 seconds for emergency priority.");
            }

            if (expiresAfter == TimeSpan.Zero)
            {
                throw new ArgumentException("Expires after is required for emergency priority.", nameof(expiresAfter));
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

    /// <summary>
    /// Sets the time-to-live (TTL) for the message, which specifies how long the message should remain available for delivery.
    /// </summary>
    /// <param name="ttl">A <see cref="TimeSpan"/> indicating the TTL duration before the message expires. A null or TimeSpan.Zero value will result in no TTL being set.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTimeToLive(TimeSpan? ttl)
    {
        _ttl = ttl;
        return this;
    }

    /// <summary>
    /// Sets a custom timestamp for the message.
    /// </summary>
    /// <param name="timestamp">The timestamp to associate with the message. If null, the current timestamp will be used at the time of submission.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTimestamp(DateTimeOffset? timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    /// <summary>
    /// Attaches a file to the message using a <see cref="FileInfo"/> object.
    /// </summary>
    /// <param name="file">A <see cref="FileInfo"/> object representing the file to be attached.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithAttachment(FileInfo file) => WithAttachment(file.FullName);

    /// <summary>
    /// Adds an attachment to the message from a specified file path.
    /// </summary>
    /// <param name="filePath">The full path to the file to be attached to the message.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithAttachment(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return WithAttachment(Path.GetFileName(filePath), stream);
    }

    /// <summary>
    /// Attaches a file to the Pushover message using the provided stream.
    /// </summary>
    /// <param name="attachmentName">The name of the attachment, including the file extension, e.g., "file.txt".</param>
    /// <param name="attachment">The stream containing the contents of the attachment.</param>
    /// <param name="mimeType">The MIME type of the attachment, e.g., "text/plain". Optional.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
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

    /// <summary>
    /// Attaches a file to the message with the specified name, content, and optional MIME type.
    /// </summary>
    /// <param name="attachmentName">The name of the file to be attached, including its extension. This name will be used in the message.</param>
    /// <param name="attachment">The content of the file to attach, provided as a byte span.</param>
    /// <param name="mimeType">The optional MIME type of the attachment. If null, the MIME type is determined automatically from the file name.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the attachment name is null, empty, or consists only of whitespace, or when the attachment content is empty.</exception>
    public PushoverMessageBuilder WithAttachment(string attachmentName, ReadOnlySpan<byte> attachment, string? mimeType = null)
    {
        if (string.IsNullOrWhiteSpace(attachmentName))
        {
            throw new ArgumentException("Attachment name is required.", nameof(attachmentName));
        }

        if (attachment.Length == 0)
        {
            throw new ArgumentException("Attachment is required.", nameof(attachment));
        }

        _attachment = attachment.ToArray();
        _attachmentName = attachmentName;
        _attachmentType = mimeType ?? MimeTypes.GetMimeType(attachmentName);
        return this;
    }

    /// <summary>
    /// Adds a single tag with the specified key and value to the message.
    /// Tags can be used to cancel emergency messages, instead of using the receipt id.
    /// </summary>
    /// <param name="key">The key of the tag to be added.</param>
    /// <param name="value">The value associated with the specified key.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTag(string key, string value) => WithTags(new PushoverMessageTag(key, value));

    /// <summary>
    /// Adds one or more tags to the message.
    /// Tags can be used to cancel emergency messages, instead of using the receipt id.
    /// </summary>
    /// <param name="tags">The tags to be added, where each tag is represented as a key-value pair.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTags(params PushoverMessageTag[] tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    /// <summary>
    /// Targets a specific device to receive the notification.
    /// </summary>
    /// <param name="deviceId">The identifier of the device to which the notification will be sent.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
    public PushoverMessageBuilder WithTargetDevice(string deviceId) => WithTargetDevices(deviceId);

    /// <summary>
    /// Specifies a list of target devices for the message.
    /// </summary>
    /// <param name="deviceIds">A collection of deviceIds representing the devices to which the message will be sent.</param>
    /// <returns>The current instance of <see cref="PushoverMessageBuilder"/> for method chaining.</returns>
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
        builder.AddIfNotNullOrEmpty("user", string.Join(",", _recipientKeys.Distinct().Order()));
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