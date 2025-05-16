namespace LVK.Pushover;

/// <summary>
/// Represents the status of a response received from the Pushover API.
/// </summary>
/// <remarks>
/// This enum is used to identify the result of an operation performed via the API,
/// with values indicating either a successful operation or an error.
/// </remarks>
public enum PushoverResponseStatus
{
    /// <summary>
    /// Indicates an error occurred during the operation.
    /// </summary>
    Error = 0,

    /// <summary>
    /// Indicates the operation was successful.
    /// </summary>
    Success = 1,
}