using System.Text.Json.Serialization;

namespace Pushover.Net;

public class PushoverSendMessageResponse : PushoverResponse
{
    [JsonPropertyName("receipt")]
    public string? Receipt { get; set; }
}
