namespace Pushover.Net.Tests;

public class PushoverMessageBuilderTests
{
    [TestCase(null)]
    [TestCase("")]
    public void WithMessage_InvalidMessages_ThrowsInvalidOperationMessage(string? message)
    {
        var builder = new PushoverMessageBuilder();
        builder.WithMessage(message!);
        Assert.Throws<InvalidOperationException>(() => builder.Validate());
    }
}