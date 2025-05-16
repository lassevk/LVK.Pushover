using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace Pushover.Net;

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
[PublicAPI]
public abstract class PushoverResponse
{
    [JsonPropertyName("status")]
    public PushoverResponseStatus Status { get; set; }

    [JsonPropertyName("request")]
    public Guid Request { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];

    [JsonExtensionData]
    public Dictionary<string, JsonElement> ExtensionData { get; set; } = new();
}