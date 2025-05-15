using System.Text.RegularExpressions;

namespace Pushover.Net;

internal static partial class ValidationHelper
{
    [GeneratedRegex("^[a-zA-Z0-9]{30}$")]
    private static partial Regex KeyOrApiTokenPattern();

    public static string? ValidateMessage(string? message)
    {
        if (message is null)
        {
            return null;
        }

        message = message.Trim();
        return message.Length switch
        {
            0      => null,
            > 1024 => throw new InvalidOperationException("Message cannot be longer than 1024 characters."),
            _      => message,
        };
    }

    public static string? ValidateTitle(string? title)
    {
        if (title is null)
        {
            return null;
        }

        title = title.Trim();
        return title.Length switch
        {
            0     => null,
            > 250 => throw new InvalidOperationException("Title cannot be longer than 250 characters."),
            _     => title,
        };
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