using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Pushover.Net;

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
[ExcludeFromCodeCoverage]
public class PushoverUserValidationResponse : PushoverResponse
{
    [JsonPropertyName("devices")]
    public List<string> Devices { get; } = [];

    [JsonPropertyName("licenses")]
    public List<string> Licenses { get; } = [];
}