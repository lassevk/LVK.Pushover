using JetBrains.Annotations;

namespace LVK.Pushover;

/// <summary>
/// Represents a client for interacting with the Pushover API to send messages, validate users, retrieve receipt statuses, and manage message retries.
/// </summary>
[PublicAPI]
public interface IPushoverClient
{
    /// <summary>
    /// Asynchronously sends a message using the Pushover API.
    /// </summary>
    /// <param name="configureMessage">
    /// An action to configure the message to be sent. The action receives a <see cref="PushoverMessageBuilder"/> instance
    /// that is used to construct the message.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverSendMessageResponse"/>
    /// with the response details from the Pushover API.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="configureMessage"/> is <c>null</c>.
    /// </exception>
    Task<PushoverSendMessageResponse> SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously sends a message to the specified recipient using the Pushover API.
    /// </summary>
    /// <param name="recipientUserOrGroupKey">
    /// The user or group key identifying the recipient of the message.
    /// </param>
    /// <param name="message">
    /// The message content to be sent.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverSendMessageResponse"/>
    /// with the response details from the Pushover API.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="recipientUserOrGroupKey"/> or <paramref name="message"/> is <c>null</c>.
    /// </exception>
    Task<PushoverSendMessageResponse> SendMessageAsync(string recipientUserOrGroupKey, string message, CancellationToken cancellationToken = default)
        => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message).WithRecipient(recipientUserOrGroupKey), cancellationToken);

    /// <summary>
    /// Asynchronously sends a message using the Pushover API to the default user specified in the <see cref="PushoverOptions"/>.
    /// </summary>
    /// <param name="message">
    /// The message content to be sent.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverSendMessageResponse"/>
    /// with the response details from the Pushover API.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="message"/> is <c>null</c>.
    /// </exception>
    Task<PushoverSendMessageResponse> SendMessageAsync(string message, CancellationToken cancellationToken = default)
        => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message), cancellationToken);

    /// <summary>
    /// Asynchronously validates a user's or group's key using the Pushover API.
    /// </summary>
    /// <param name="userKey">
    /// The user or group key to be validated.
    /// </param>
    /// <param name="deviceId">
    /// An optional device identifier to validate the key against a specific device. Defaults to <c>null</c>.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion. Defaults to <c>default</c>.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverUserValidationResponse"/>
    /// with the validation result and associated devices.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="userKey"/> is <c>null</c> or empty or not in the right format.
    /// </exception>
    Task<PushoverUserValidationResponse> ValidateUserOrGroupAsync(string userKey, string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the status of a receipt using the Pushover API.
    /// </summary>
    /// <param name="receiptId">
    /// The unique identifier of the receipt whose status is to be fetched.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverReceiptStatusResponse"/>
    /// with the receipt's status details.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="receiptId"/> is <c>null</c> or empty.
    /// </exception>
    Task<PushoverReceiptStatusResponse> GetReceiptStatusAsync(string receiptId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously cancels pending retries for a message associated with the specified receipt ID using the Pushover API.
    /// </summary>
    /// <param name="receiptId">
    /// The unique identifier of the receipt for the message whose retries are to be canceled.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverCancelRetriesResponse"/>
    /// with the response details from the Pushover API.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="receiptId"/> is <c>null</c>, empty, or consists only of white-space characters.
    /// </exception>
    Task<PushoverCancelRetriesResponse> CancelRetriesAsync(string receiptId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously cancels pending retries for messages associated with the specified tag using the Pushover API.
    /// </summary>
    /// <param name="tag">
    /// The tag consisting of a key-value pair (<see cref="PushoverMessageTag"/>) used to identify the messages whose retries are to be canceled.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to cancel the operation before completion.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a <see cref="PushoverCancelRetriesResponse"/>
    /// with the response details from the Pushover API.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the key or value of <paramref name="tag"/> is <c>null</c>, empty, or consists only of white-space characters.
    /// </exception>
    Task<PushoverCancelRetriesResponse> CancelRetriesByTagAsync(PushoverMessageTag tag, CancellationToken cancellationToken = default);
}