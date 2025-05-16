using System.Diagnostics.CodeAnalysis;

namespace Pushover.Net;

/// <summary>
/// Represents the response returned by the Pushover API when a request to cancel retries
/// of a previously sent emergency-priority message is made.
/// </summary>
/// <remarks>
/// This class inherits from <see cref="PushoverResponse"/> and serves as a container for the details
/// of the cancellation response provided by the API. Typically, it is used in conjunction with the
/// <c>CancelRetriesAsync</c> method in the <see cref="IPushoverClient"/> interface.
/// </remarks>
[ExcludeFromCodeCoverage]
public class PushoverCancelRetriesResponse : PushoverResponse
{

}