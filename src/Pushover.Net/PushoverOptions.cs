namespace Pushover.Net;

public class PushoverOptions
{
    public string? ApiToken { get; private set; }

    public string? DefaultUserKey { get; private set; }

    public PushoverOptions WithApiToken(string apiToken)
    {
        ValidationHelper.ValidateApiToken(apiToken);
        ApiToken = apiToken;
        return this;
    }

    public PushoverOptions WithDefaultUser(string userKey)
    {
        ValidationHelper.ValidateUserOrGroupKey(userKey);
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