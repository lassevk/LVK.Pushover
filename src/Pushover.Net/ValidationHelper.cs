using System.Text.RegularExpressions;

namespace Pushover.Net;

internal static partial class ValidationHelper
{
    [GeneratedRegex("^[a-zA-Z0-9]{30}$")]
    private static partial Regex KeyOrApiTokenPattern();

    public static void ValidateMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("Message cannot be null or empty.");
        }

        if (message.Length > 1024)
        {
            throw new InvalidOperationException("Message cannot be longer than 1024 characters.");
        }
    }

    public static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException("Title cannot be null or empty.");
        }

        if (title.Length > 250)
        {
            throw new InvalidOperationException("Title cannot be longer than 250 characters.");
        }
    }

    public static void ValidateUserOrGroupKey(string userOrGroupKey)
    {
        if (string.IsNullOrWhiteSpace(userOrGroupKey))
        {
            throw new InvalidOperationException("User or group key cannot be null or empty.");
        }
        if (!KeyOrApiTokenPattern().IsMatch(userOrGroupKey))
        {
            throw new InvalidOperationException($"Invalid user or group key: {userOrGroupKey}.");
        }
    }

    public static void ValidateApiToken(string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new InvalidOperationException("API token cannot be null or empty.");
        }

        if (!KeyOrApiTokenPattern().IsMatch(apiToken))
        {
            throw new InvalidOperationException($"Invalid API token: {apiToken}.");
        }
    }
}