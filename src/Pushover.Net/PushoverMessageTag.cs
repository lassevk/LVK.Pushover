using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Pushover.Net;

/// <summary>
/// Represents a key-value pair used as a tag in a Pushover message.
/// </summary>
/// <remarks>
/// The <c>PushoverMessageTag</c> can be used to define metadata or additional parameters for Pushover messages
/// that are typically passed to APIs or builders when constructing a message.
/// </remarks>
[PublicAPI]
[ExcludeFromCodeCoverage]
public readonly record struct PushoverMessageTag(string Key, string Value)
{
    /// <inheritdoc />
    public override string ToString() => $"{Key}={Value}";
}