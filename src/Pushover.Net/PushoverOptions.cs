namespace Pushover.Net;

public class PushoverOptions
{
    public string? ApiToken { get; private set; }

    public string? DefaultUserKey { get; private set; }

    public PushoverOptions WithApiToken(string apiToken)
    {
        ArgumentNullException.ThrowIfNull(apiToken);
        apiToken = apiToken.Trim();
        if (apiToken.Length != 30)
        {
            throw new InvalidOperationException("Invalid API token.");
        }

        ApiToken = apiToken;
        return this;
    }

    public PushoverOptions WithDefaultUser(string userKey)
    {
        ArgumentNullException.ThrowIfNull(userKey);
        userKey = userKey.Trim();
        if (userKey.Length != 30)
        {
            throw new InvalidOperationException("Invalid user key.");
        }

        DefaultUserKey = userKey;
        return this;
    }

    internal void Validate()
    {
        if (ApiToken is null)
        {
            throw new InvalidOperationException("API token is not configured.");
        }
    }
}