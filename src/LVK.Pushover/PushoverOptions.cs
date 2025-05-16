using JetBrains.Annotations;

namespace LVK.Pushover;

/// <summary>
/// Represents the configuration options for the Pushover API integration.
/// </summary>
/// <remarks>
/// This class is used to configure and validate the necessary settings for interacting with the Pushover API.
/// It provides methods to set the API token and the default user key.
/// </remarks>
[PublicAPI]
public class PushoverOptions
{
    /// <summary>
    /// Gets the Pushover API token used to authenticate requests to the Pushover service.
    /// </summary>
    /// <remarks>
    /// The API token is a unique identifier required to make authorized API calls to the Pushover system.
    /// This property is set using the <c>WithApiToken</c> method, which ensures the token's validity through
    /// internal validation mechanisms. If the token is not configured, an exception will be thrown upon
    /// attempting to use Pushover services.
    /// </remarks>
    public string? ApiToken { get; private set; }

    /// <summary>
    /// Gets the default user key used for Pushover message delivery when no specific user key is provided.
    /// </summary>
    /// <remarks>
    /// The default user key is a unique identifier associated with the intended recipient of Pushover messages.
    /// It can be set using the <c>WithDefaultUser</c> method and is validated to ensure it meets the requirements
    /// for a valid user or group key. If the default user key is not configured, calling methods that rely on it
    /// may result in an error or require a user key to be explicitly defined.
    /// </remarks>
    public string? DefaultUserKey { get; private set; }

    /// <summary>
    /// Sets the API token for the Pushover client and performs validation on the provided token.
    /// </summary>
    /// <param name="apiToken">The API token to be used for the Pushover client. Must be a valid token that passes validation.</param>
    /// <returns>The updated <see cref="PushoverOptions"/> instance with the API token set.</returns>
    public PushoverOptions WithApiToken(string apiToken)
    {
        PushoverValidationHelper.ValidateApiToken(apiToken);
        ApiToken = apiToken;
        return this;
    }

    /// <summary>
    /// Sets the default user key for the Pushover client and validates the provided key.
    /// </summary>
    /// <param name="userKey">The user key to be set as the default for the Pushover client. Must be a valid user or group key that passes validation.</param>
    /// <returns>The updated <see cref="PushoverOptions"/> instance with the default user key set.</returns>
    public PushoverOptions WithDefaultUser(string userKey)
    {
        PushoverValidationHelper.ValidateUserOrGroupKey(userKey);
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