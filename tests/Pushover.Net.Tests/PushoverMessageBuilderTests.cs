namespace Pushover.Net.Tests;

public class PushoverMessageBuilderTests
{
    private readonly PushoverMessageBuilder _builder = new();

    [TestCase(null)]
    [TestCase("")]
    public void WithMessage_InvalidMessages_ThrowsInvalidOperationMessage(string? message)
    {
        Assert.Throws<InvalidOperationException>(() => _builder.WithMessage(message!));
    }
}