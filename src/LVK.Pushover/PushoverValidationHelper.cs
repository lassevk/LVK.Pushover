using System.Text.RegularExpressions;

namespace LVK.Pushover;

internal static partial class PushoverValidationHelper
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

    public static string? ValidateUserOrGroupKey(string? userOrGroupKey)
    {
        if (userOrGroupKey is null)
        {
            return null;
        }

        userOrGroupKey = userOrGroupKey.Trim();
        if (userOrGroupKey.Length == 0)
        {
            return null;
        }

        if (KeyOrApiTokenPattern().IsMatch(userOrGroupKey))
        {
            return userOrGroupKey;
        }

        throw new InvalidOperationException($"Invalid user or group key: {userOrGroupKey}.");
    }

    public static string? ValidateApiToken(string? apiToken)
    {
        if (apiToken is null)
        {
            return null;
        }

        apiToken = apiToken.Trim();
        if (apiToken.Length == 0)
        {
            return null;
        }

        if (KeyOrApiTokenPattern().IsMatch(apiToken))
        {
            return apiToken;
        }

        throw new InvalidOperationException($"Invalid API token: {apiToken}.");
    }

    public static string? ValidateUrl(string? url)
    {
        if (url is null)
        {
            return null;
        }

        url = url.Trim();
        return url.Length switch
        {
            0     => null,
            > 256 => throw new InvalidOperationException("Url cannot be longer than 256 characters."),
            _     => url,
        };
    }

    public static string? ValidateUrlTitle(string? title)
    {
        if (title is null)
        {
            return null;
        }

        title = title.Trim();
        return title.Length switch
        {
            0     => null,
            > 250 => throw new InvalidOperationException("Url title cannot be longer than 250 characters."),
            _     => title,
        };
    }
}