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

    [Test]
    public async Task WithMessage_HtmlMessageFormat_SetsHtmlFlagInRequest()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithMessage("TEST", PushoverMessageFormat.Html);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=message

                                        TEST
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=html

                                        1
                                        --abcdef--
                                        """));
    }

    [Test]
    public async Task WithMessage_MonospaceMessageFormat_SetsMonospaceFlagInRequest()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithMessage("TEST", PushoverMessageFormat.Monospace);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=message

                                        TEST
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=monospace

                                        1
                                        --abcdef--
                                        """));
    }

    [Test]
    public async Task WithMessage_DefaultMessageFormat_DoesNotAddHtmlOrMonospaceFlagsInRequest()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithMessage("TEST");

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=message

                                        TEST
                                        --abcdef--
                                        """));
    }

    [Test]
    public void WithMessage_InvalidMessageFormat_ThrowsArgumentOutOfRangeException()
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<ArgumentOutOfRangeException>(() => messageBuilder.WithMessage("TEST", (PushoverMessageFormat)100));
    }

    [TestCase("000000000000000000000000000000")]
    [TestCase("012345678901234567890123456789")]
    public async Task WithRecipient_SingleRecipient_AddsItToContent(string userOrGroupKey)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithRecipient(userOrGroupKey);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=user

                                       {userOrGroupKey}
                                       --abcdef--
                                       """));
    }
}