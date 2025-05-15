using System.Text.Json.Serialization;

namespace Pushover.Net;

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class PushoverResponse
{
    [JsonPropertyName("status")]
    public PushoverResponseStatus Status { get; set; }

    [JsonPropertyName("request")]
    public Guid Request { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = [];
}