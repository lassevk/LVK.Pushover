using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Pushover.Net;

[ExcludeFromCodeCoverage]
public class PushoverSendMessageResponse : PushoverResponse
{
    [JsonPropertyName("receipt")]
    public string? Receipt { get; set; }
}
