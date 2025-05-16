using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

namespace Pushover.Net;

/// <summary>
/// Represents an exception that occurs when a request to the Pushover API fails.
/// </summary>
/// <remarks>
/// This exception encapsulates the errors returned by the Pushover API and provides additional context
/// related to the failure of the request. Errors can be accessed via the <see cref="Errors"/> property.
/// </remarks>
[PublicAPI]
[ExcludeFromCodeCoverage]
public class PushoverApiRequestFailedException : InvalidOperationException
{
    /// <summary>
    /// Represents an exception thrown when a request to the Pushover API fails.
    /// </summary>
    /// <remarks>
    /// This exception provides access to the errors returned by the Pushover API
    /// through the <see cref="Errors"/> property. It is typically thrown when
    /// the response indicates a failure or an exception occurs during the request.
    /// </remarks>
    public PushoverApiRequestFailedException(PushoverResponse response, Exception? innerException = null)
        : base(innerException?.Message ?? "Pushover request failed.", innerException)
    {
        Errors = response.Errors.ToArray();
    }

    /// <summary>
    /// Gets the collection of error messages returned by the Pushover API.
    /// </summary>
    /// <remarks>
    /// This property contains the errors reported by the Pushover API during the failed request.
    /// Use this information to investigate or log the specific reasons for the failure.
    /// </remarks>
    public string[] Errors { get; private set; }
}