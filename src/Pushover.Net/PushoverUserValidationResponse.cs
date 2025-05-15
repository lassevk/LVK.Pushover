using System.Text.Json.Serialization;

namespace Pushover.Net;

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class PushoverUserValidationResponse : PushoverResponse
{
    [JsonPropertyName("devices")]
    public List<string> Devices { get; } = [];

    [JsonPropertyName("licenses")]
    public List<string> Licenses { get; } = [];
}