namespace Pushover.Net;

public interface IPushoverClient
{
    Task SendMessageAsync(Action<PushoverMessageBuilder> configureMessage, CancellationToken cancellationToken);

    Task SendMessageAsync(string recipientUserOrGroupKey, string message, CancellationToken cancellationToken)
        => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message).WithRecipient(recipientUserOrGroupKey), cancellationToken);

    Task SendMessageAsync(string message, CancellationToken cancellationToken) => SendMessageAsync(messageBuilder => messageBuilder.WithMessage(message), cancellationToken);
}