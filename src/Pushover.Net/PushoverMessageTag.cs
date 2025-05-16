using JetBrains.Annotations;

namespace Pushover.Net;

[PublicAPI]
public readonly record struct PushoverMessageTag(string Key, string Value)
{
    public override string ToString() => $"{Key}={Value}";
}