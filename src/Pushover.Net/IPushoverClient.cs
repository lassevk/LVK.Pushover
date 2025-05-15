namespace Pushover.Net;

public interface IPushoverClient
{
    Task<PushoverResponse> SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken);

    Task<PushoverResponse> SendMessageAsync(string recipientUserOrGroupKey, string message, CancellationToken cancellationToken)
        => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message).WithRecipient(recipientUserOrGroupKey), cancellationToken);

    Task<PushoverResponse> SendMessageAsync(string message, CancellationToken cancellationToken) => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message), cancellationToken);
}