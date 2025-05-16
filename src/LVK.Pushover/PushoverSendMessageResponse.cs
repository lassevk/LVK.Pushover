using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace LVK.Pushover;

/// <summary>
/// Represents the response received after sending a message using the Pushover service.
/// </summary>
/// <remarks>
/// This class contains details about the Pushover API response, including the receipt data for tracking message delivery
/// when sent with emergency priority.
/// </remarks>
[ExcludeFromCodeCoverage]
public class PushoverSendMessageResponse : PushoverResponse
{
    /// <summary>
    /// Gets or sets the receipt string provided in the Pushover response. This value is only
    /// returned for emergency priority messages.
    /// </summary>
    /// <remarks>
    /// The receipt is typically used for tracking the status of messages sent with emergency priority.
    /// It can be utilized to retrieve information such as delivery confirmation or status updates
    /// regarding the dispatched message.
    /// </remarks>
    [JsonPropertyName("receipt")]
    public string? Receipt { get; set; }
}
