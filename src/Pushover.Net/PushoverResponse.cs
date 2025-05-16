using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace Pushover.Net;

/// <summary>
/// Represents a generic response returned by the Pushover API.
/// </summary>
/// <remarks>
/// The <see cref="PushoverResponse"/> class serves as the base class for various
/// specific types of responses provided by the Pushover API. It contains
/// common properties applicable to all response types, such as the response
/// status, request identifier, and any additional errors or metadata.
/// </remarks>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
[PublicAPI]
[ExcludeFromCodeCoverage]
public abstract class PushoverResponse
{
    /// <summary>
    /// Gets or sets the response status of a Pushover operation.
    /// </summary>
    /// The status indicates the result of the operation, represented as a value from the
    /// <see cref="PushoverResponseStatus"/> enumeration. A status of `Success` (1) typically
    /// indicates that the operation was successful, while `Error` (0) signifies a failure.
    [JsonPropertyName("status")]
    public PushoverResponseStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the Pushover API request.
    /// </summary>
    /// The request identifier is represented as a GUID and can be used to track or reference
    /// a specific API request associated with a Pushover response.
    [JsonPropertyName("request")]
    public Guid Request { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the Pushover API response. This value
    /// is only returned in case of errors, and will typically just contain the string 'invalid'.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Gets or sets the collection of error messages returned by the Pushover API.
    /// </summary>
    /// The errors provide detailed information about any issues that occurred during the operation.
    /// This property represents a list of error messages, typically populated when the
    /// <see cref="Status"/> is `Error`. Each entry in the list corresponds to a specific
    /// issue or violation detected by the Pushover API.
    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];
}