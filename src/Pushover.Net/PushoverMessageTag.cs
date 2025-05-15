namespace Pushover.Net;

public record PushoverMessageTag(string Key, string Value)
{
    public override string ToString() => $"{Key}={Value}";
}