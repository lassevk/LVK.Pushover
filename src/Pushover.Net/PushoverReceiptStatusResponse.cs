using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Pushover.Net;

[ExcludeFromCodeCoverage]
public class PushoverReceiptStatusResponse : PushoverResponse
{
    [JsonPropertyName("acknowledged")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool IsAcknowledged { get; set; }

    [JsonPropertyName("acknowledged_at")]
    public long AcknowledgedAtValue { get; set; }

    [JsonIgnore]
    public DateTimeOffset? AcknowledgedAt => AcknowledgedAtValue != 0 ? DateTimeOffset.FromUnixTimeSeconds(AcknowledgedAtValue) : null;

    [JsonPropertyName("acknowledged_by")]
    public string AcknowledgedByUserKey { get; set; } = "";

    [JsonPropertyName("acknowledged_by_device")]
    public string AcknowledgedByDeviceId { get; set; } = "";

    [JsonPropertyName("last_delivered_at")]
    public long LastDeliveredAtValue { get; set; }

    [JsonIgnore]
    public DateTimeOffset LastDeliveredAt => DateTimeOffset.FromUnixTimeSeconds(LastDeliveredAtValue);

    [JsonPropertyName("expired")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool IsExpired { get; set; }

    [JsonPropertyName("expires_at")]
    public long ExpiresAtValue { get; set; }

    [JsonIgnore]
    public DateTimeOffset ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtValue);

    [JsonPropertyName("called_back")]
    [JsonConverter(typeof(BooleanIntJsonConverter))]
    public bool CalledBack { get; set; }

    [JsonPropertyName("called_back_at")]
    public long CalledBackAtValue { get; set; }

    [JsonIgnore]
    public DateTimeOffset? CalledBackAt => CalledBackAtValue != 0 ? DateTimeOffset.FromUnixTimeSeconds(CalledBackAtValue) : null;
}