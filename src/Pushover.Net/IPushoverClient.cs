namespace Pushover.Net;

public interface IPushoverClient
{
    Task<PushoverSendMessageResponse> SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken);

    Task<PushoverSendMessageResponse> SendMessageAsync(string recipientUserOrGroupKey, string message, CancellationToken cancellationToken)
        => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message).WithRecipient(recipientUserOrGroupKey), cancellationToken);

    Task<PushoverSendMessageResponse> SendMessageAsync(string message, CancellationToken cancellationToken) => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message), cancellationToken);
    Task<PushoverUserValidationResponse> ValidateUserOrGroupAsync(string userKey, string? deviceId = null, CancellationToken cancellationToken = default);

    Task<PushoverReceiptStatusResponse> GetReceiptStatusAsync(string receiptId, CancellationToken cancellationToken = default);
}