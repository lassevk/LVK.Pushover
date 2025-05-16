using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace LVK.Pushover;

/// <summary>
/// Represents the response received from the Pushover API when validating a user or group key.
/// </summary>
/// <remarks>
/// This class is used to capture the validation result, as well as additional details like
/// associated devices and licenses for the provided user or group key.
/// </remarks>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
[ExcludeFromCodeCoverage]
public class PushoverUserValidationResponse : PushoverResponse
{
    /// <summary>
    /// Gets the list of devices associated with the validated Pushover user or group key.
    /// </summary>
    /// <remarks>
    /// The devices represent the registered devices where notifications can be sent for the specific user
    /// or group key. This property is populated by the Pushover API during the validation response.
    /// </remarks>
    [JsonPropertyName("devices")]
    public List<string> Devices { get; } = [];

    /// <summary>
    /// Gets the list of licenses associated with the validated Pushover user or group key.
    /// </summary>
    /// <remarks>
    /// The licenses represent the entities or features that the user or group key is authorized
    /// to use within the Pushover system. This property is provided by the Pushover API
    /// during the validation response.
    /// </remarks>
    [JsonPropertyName("licenses")]
    public List<string> Licenses { get; } = [];
}