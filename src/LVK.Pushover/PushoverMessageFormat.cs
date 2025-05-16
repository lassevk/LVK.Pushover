namespace LVK.Pushover;

/// <summary>
/// Specifies that the message should be sent in plaintext format.
/// This is the default format for Pushover messages.
/// </summary>
public enum PushoverMessageFormat
{
    /// <summary>
    /// Interpret the message as plaintext. This is the default.
    /// </summary>
    Plaintext,

    /// <summary>
    /// Show the message as monospace text.
    /// </summary>
    Monospace,

    /// <summary>
    /// Interpret the message as HTML. Note that the html tags supported are limited.
    /// Check the documentation for more information.
    /// </summary>
    Html,

    /// <summary>
    /// Default is <see cref="Plaintext"/>.
    /// </summary>
    Default = Plaintext,
}