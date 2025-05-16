using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Pushover.Net;

/// <summary>
/// Represents the response data for a receipt status retrieved from the Pushover API.
/// </summary>
/// <remarks>
/// Contains information regarding a receipt's acknowledgment status, delivery time, expiration,
/// callbacks, and other metadata.
/// </remarks>
[ExcludeFromCodeCoverage]
public class PushoverReceiptStatusResponse : PushoverResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the receipt has been acknowledged by the recipient.
    /// </summary>
    /// <remarks>
    /// This property represents the acknowledgment status of a receipt as retrieved
    /// from the Pushover API. A receipt is considered acknowledged if an acknowledgment
    /// has been explicitly triggered by a user or device.
    /// </remarks>
    [JsonPropertyName("acknowledged")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool IsAcknowledged { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the receipt was acknowledged, represented as a UNIX epoch time.
    /// </summary>
    /// <remarks>
    /// This property represents the time at which the acknowledgment of a receipt occurred.
    /// The value is expressed in seconds since the UNIX epoch (January 1, 1970, 00:00:00 UTC).
    /// Use this value to determine when a specific receipt acknowledgment was made.
    /// </remarks>
    [JsonPropertyName("acknowledged_at")]
    public long AcknowledgedAtValue { get; set; }

    /// <summary>
    /// Gets the timestamp when the receipt was acknowledged.
    /// </summary>
    /// <remarks>
    /// This property represents the moment, in Unix time, when the acknowledgment of the receipt
    /// occurred. If the receipt has not been acknowledged, this value will be null.
    /// </remarks>
    [JsonIgnore]
    public DateTimeOffset? AcknowledgedAt => AcknowledgedAtValue != 0 ? DateTimeOffset.FromUnixTimeSeconds(AcknowledgedAtValue) : null;

    /// <summary>
    /// Gets or sets the user key of the recipient who acknowledged the receipt.
    /// </summary>
    /// <remarks>
    /// This property contains the identifier of the user who explicitly acknowledged
    /// the receipt. The user key can be used to track or validate which user interacted
    /// with the notification.
    /// </remarks>
    [JsonPropertyName("acknowledged_by")]
    public string AcknowledgedByUserKey { get; set; } = "";

    /// <summary>
    /// Gets or sets the unique identifier of the device that acknowledged the receipt.
    /// </summary>
    /// <remarks>
    /// This property indicates the specific device ID from which the acknowledgment
    /// for the receipt was triggered. It provides additional context on the source
    /// of the acknowledgment in multi-device scenarios.
    /// </remarks>
    [JsonPropertyName("acknowledged_by_device")]
    public string AcknowledgedByDeviceId { get; set; } = "";

    /// <summary>
    /// Gets or sets the raw Unix timestamp (in seconds) of the most recent delivery of the notification.
    /// </summary>
    /// <remarks>
    /// This property holds the delivery timestamp as received from the Pushover API. To convert this
    /// value into a human-readable date and time, use the <see cref="LastDeliveredAt"/> property, which
    /// converts the Unix timestamp into a <see cref="DateTimeOffset"/> object.
    /// </remarks>
    [JsonPropertyName("last_delivered_at")]
    public long LastDeliveredAtValue { get; set; }

    /// <summary>
    /// Gets the timestamp representing the most recent delivery of the notification.
    /// </summary>
    /// <remarks>
    /// This property provides the delivery time as a <see cref="DateTimeOffset"/> object,
    /// converted from the raw Unix timestamp received via the Pushover API. It is useful
    /// for interpreting the time in a human-readable format or performing further time calculations.
    /// </remarks>
    [JsonIgnore]
    public DateTimeOffset LastDeliveredAt => DateTimeOffset.FromUnixTimeSeconds(LastDeliveredAtValue);

    /// <summary>
    /// Gets or sets a value indicating whether the receipt has expired.
    /// </summary>
    /// <remarks>
    /// This property represents the expiration status of a receipt as retrieved
    /// from the Pushover API. A receipt is considered expired if its validity period
    /// has elapsed and it is no longer active.
    /// </remarks>
    [JsonPropertyName("expired")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool IsExpired { get; set; }

    /// <summary>
    /// Gets or sets the expiration time of the receipt in Unix timestamp format.
    /// </summary>
    /// <remarks>
    /// This property represents the Unix epoch time at which the receipt is considered expired,
    /// as returned by the Pushover API. The associated expiration time provides a way to determine
    /// the deadline for acknowledgment or other actions tied to the receipt.
    /// </remarks>
    [JsonPropertyName("expires_at")]
    public long ExpiresAtValue { get; set; }

    /// <summary>
    /// Gets the time at which the receipt will expire.
    /// </summary>
    /// <remarks>
    /// This property represents the expiration time of a receipt as provided by the Pushover API.
    /// The value indicates the point in time when the receipt is no longer valid.
    /// </remarks>
    [JsonIgnore]
    public DateTimeOffset ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtValue);

    /// <summary>
    /// Gets or sets a value indicating whether a callback has been triggered for the receipt.
    /// </summary>
    /// <remarks>
    /// This property represents the callback status of a receipt as retrieved from the Pushover API.
    /// A receipt is considered called back if a user-defined callback URL has been triggered upon
    /// delivery or acknowledgment.
    /// </remarks>
    [JsonPropertyName("called_back")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool CalledBack { get; set; }

    /// <summary>
    /// Gets or sets the Unix timestamp representing the time at which a callback was triggered for the receipt.
    /// </summary>
    /// <remarks>
    /// This property stores the Unix timestamp of the callback. It can be converted to a <see cref="DateTimeOffset"/>
    /// object for easier manipulation and readability. If the value is <c>0</c>, it indicates that no callback has occurred.
    /// </remarks>
    [JsonPropertyName("called_back_at")]
    public long CalledBackAtValue { get; set; }

    /// <summary>
    /// Gets the timestamp indicating when the callback associated with the receipt was triggered.
    /// </summary>
    /// <remarks>
    /// This property provides the time of the callback as a Unix timestamp converted to a
    /// nullable <see cref="DateTimeOffset"/>. A `null` value indicates that the callback has not been triggered.
    /// </remarks>
    [JsonIgnore]
    public DateTimeOffset? CalledBackAt => CalledBackAtValue != 0 ? DateTimeOffset.FromUnixTimeSeconds(CalledBackAtValue) : null;
}