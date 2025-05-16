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

    [Test]
    public async Task WithRecipient_TwoRecipients_AddsThemBothToContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithRecipients("000000000000000000000000000000", "012345678901234567890123456789");

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=user

                                        000000000000000000000000000000,012345678901234567890123456789
                                        --abcdef--
                                        """));
    }

    [Test]
    public async Task WithRecipient_SameRecipientTwice_AddsRecipientToContentOnlyOnce()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithRecipients("000000000000000000000000000000", "000000000000000000000000000000");

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=user

                                        000000000000000000000000000000
                                        --abcdef--
                                        """));
    }

    [TestCase("This is a title")]
    [TestCase("TEST")]
    public async Task WithTitle_WithTestCases_AddsItToContent(string title)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTitle(title);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=title

                                        {title}
                                        --abcdef--
                                        """));
    }

    [TestCase("https://google.com")]
    [TestCase("https://github.com")]
    public async Task WithUrl_WithTestCases_AddsItToContent(string url)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithUrl(url);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=url

                                        {url}
                                        --abcdef--
                                        """));
    }

    [TestCase("Google")]
    [TestCase("GitHub")]
    public async Task WithUrlTitle_WithTestCases_AddsItToContent(string title)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithUrlTitle(title);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=url_title

                                        {title}
                                        --abcdef--
                                        """));
    }

    [TestCase("https://google.com", "Google")]
    [TestCase("https://github.com", "GitHub")]
    public async Task WithUrlWithTitle_WithTestCases_AddsItToContent(string url, string title)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithUrlWithTitle(url, title);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=url

                                        {url}
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=url_title

                                        {title}
                                        --abcdef--
                                        """));
    }
}