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
        Assert.That(output, Is.EqualTo("""
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
        Assert.That(output, Is.EqualTo("""
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
        Assert.That(output, Is.EqualTo("""
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
        Assert.That(output, Is.EqualTo("""
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
        Assert.That(output, Is.EqualTo("""
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

    [TestCase(PushoverMessageSound.Default, "pushover")]
    [TestCase(PushoverMessageSound.Bike, "bike")]
    public async Task WithSound_WithTestCases_AddsItToContent(PushoverMessageSound sound, string expected)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithSound(sound);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=sound

                                        {expected}
                                        --abcdef--
                                        """));
    }

    [TestCase("plingplong")]
    [TestCase("doink")]
    public async Task WithCustomSound_WithTestCases_AddsItToContent(string sound)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithCustomSound(sound);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=sound

                                        {sound}
                                        --abcdef--
                                        """));
    }

    [Test]
    public async Task WithLowestPriority_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithLowestPriority();

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=priority

                                       -2
                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithLowPriority_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithLowPriority();

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=priority

                                       -1
                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithNormalPriority_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithNormalPriority();

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef

                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithHighPriority_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithHighPriority();

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=priority

                                       1
                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithEmergencyPriority_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithEmergencyPriority(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(600), "https://callback");

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=priority

                                       2
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=retry

                                       30
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=expire

                                       600
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=callback

                                       https://callback
                                       --abcdef--
                                       """));
    }

    [Test]
    public void WithPriority_InvalidPriority_ThrowsArgumentOutOfRangeException()
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<ArgumentOutOfRangeException>(() => messageBuilder.WithPriority((PushoverMessagePriority)100));
    }

    [Test]
    public void WithPriority_EmergencyPriorityButNoRetry_ThrowsInvalidOperationException()
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<ArgumentException>(() => messageBuilder.WithPriority(PushoverMessagePriority.Emergency, TimeSpan.Zero, TimeSpan.FromSeconds(600), "https://callback"));
    }

    [Test]
    public void WithPriority_EmergencyPriorityButNoExpiry_ThrowsInvalidOperationException()
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<ArgumentException>(() => messageBuilder.WithPriority(PushoverMessagePriority.Emergency, TimeSpan.FromSeconds(30), TimeSpan.Zero, "https://callback"));
    }

    [Test]
    public void WithPriority_EmergencyPriorityWithRetryLessThan30Seconds_ThrowsInvalidOperationException()
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<ArgumentOutOfRangeException>(() => messageBuilder.WithPriority(PushoverMessagePriority.Emergency, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(600), "https://callback"));
    }

    [TestCase(PushoverMessagePriority.Lowest)]
    [TestCase(PushoverMessagePriority.Low)]
    [TestCase(PushoverMessagePriority.Normal)]
    [TestCase(PushoverMessagePriority.High)]
    public void WithPriority_NonEmergencyPriorityWithRetry_ThrowsInvalidOperationException(PushoverMessagePriority priority)
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<InvalidOperationException>(() => messageBuilder.WithPriority(priority, TimeSpan.FromSeconds(30), TimeSpan.Zero));
    }

    [TestCase(PushoverMessagePriority.Lowest)]
    [TestCase(PushoverMessagePriority.Low)]
    [TestCase(PushoverMessagePriority.Normal)]
    [TestCase(PushoverMessagePriority.High)]
    public void WithPriority_NonEmergencyPriorityWithExpiry_ThrowsInvalidOperationException(PushoverMessagePriority priority)
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<InvalidOperationException>(() => messageBuilder.WithPriority(priority, TimeSpan.Zero, TimeSpan.FromSeconds(30)));
    }

    [TestCase(PushoverMessagePriority.Lowest)]
    [TestCase(PushoverMessagePriority.Low)]
    [TestCase(PushoverMessagePriority.Normal)]
    [TestCase(PushoverMessagePriority.High)]
    public void WithPriority_NonEmergencyPriorityWithCallback_ThrowsInvalidOperationException(PushoverMessagePriority priority)
    {
        var messageBuilder = new PushoverMessageBuilder();
        Assert.Throws<InvalidOperationException>(() => messageBuilder.WithPriority(priority, TimeSpan.Zero, TimeSpan.Zero, "https://callback"));
    }

    [TestCase(30)]
    [TestCase(600)]
    public async Task WithTimeToLive_OutputsCorrectContent(int ttlSeconds)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTimeToLive(TimeSpan.FromSeconds(ttlSeconds));

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=ttl

                                        {ttlSeconds}
                                        --abcdef--
                                        """));
    }

    [Test]
    public async Task WithTimestamp_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTimestamp(new DateTimeOffset(2012, 3, 8, 17, 34, 22, TimeSpan.FromHours(-6)));

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=timestamp

                                        1331249662
                                        --abcdef--
                                        """));
    }

    [TestCase(".txt", "text/plain")]
    [TestCase(".jpg", "image/jpeg")]
    public async Task WithAttachment_ByFilename_OutputsCorrectContent(string extension, string mimeType)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), "rather-unique-file" + extension);
        File.WriteAllBytes(tempFilePath, [1, 2, 3, 4]);

        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithAttachment(tempFilePath);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: {mimeType}
                                        Content-Disposition: form-data; name=attachment; filename=rather-unique-file{extension}; filename*=utf-8''rather-unique-file{extension}

                                        {"\u0001\u0002\u0003\u0004"}
                                        --abcdef--
                                        """));
    }

    [TestCase(".txt", "text/plain")]
    [TestCase(".jpg", "image/jpeg")]
    public async Task WithAttachment_ByFileInfo_OutputsCorrectContent(string extension, string mimeType)
    {
        string tempFilePath = Path.Combine(Path.GetTempPath(), "rather-unique-file" + extension);
        File.WriteAllBytes(tempFilePath, [1, 2, 3, 4]);

        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithAttachment(new FileInfo(tempFilePath));

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: {mimeType}
                                        Content-Disposition: form-data; name=attachment; filename=rather-unique-file{extension}; filename*=utf-8''rather-unique-file{extension}

                                        {"\u0001\u0002\u0003\u0004"}
                                        --abcdef--
                                        """));
    }

    [TestCase(".txt", "text/plain")]
    [TestCase(".jpg", "image/jpeg")]
    public async Task WithAttachment_ByStream_OutputsCorrectContent(string extension, string mimeType)
    {
        var messageBuilder = new PushoverMessageBuilder();
        var stream = new MemoryStream([1, 2, 3, 4]);
        messageBuilder.WithAttachment("rather-unique-file" + extension, stream, mimeType);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: {mimeType}
                                        Content-Disposition: form-data; name=attachment; filename=rather-unique-file{extension}; filename*=utf-8''rather-unique-file{extension}

                                        {"\u0001\u0002\u0003\u0004"}
                                        --abcdef--
                                        """));
    }

    [TestCase(".txt", "text/plain")]
    [TestCase(".jpg", "image/jpeg")]
    public async Task WithAttachment_BySpan_OutputsCorrectContent(string extension, string mimeType)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithAttachment("rather-unique-file" + extension, [1, 2, 3, 4], mimeType);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: {mimeType}
                                        Content-Disposition: form-data; name=attachment; filename=rather-unique-file{extension}; filename*=utf-8''rather-unique-file{extension}

                                        {"\u0001\u0002\u0003\u0004"}
                                        --abcdef--
                                        """));
    }

    [TestCase("a", "abc")]
    [TestCase("someTag", "someValue")]
    public async Task WithTag_OutputsCorrectContent(string key, string value)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTag(key, value);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=tags

                                       {key}={value}
                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithTags_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTags(new("k", "1"), new("a", "abc"));

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=tags

                                       k=1,a=abc
                                       --abcdef--
                                       """));
    }

    [Test]
    public async Task WithTargetDevices_OutputsCorrectContent()
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTargetDevices("iphone", "desktop");

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo("""
                                       --abcdef
                                       Content-Type: text/plain; charset=utf-8
                                       Content-Disposition: form-data; name=device

                                       iphone,desktop
                                       --abcdef--
                                       """));
    }

    [TestCase("iphone")]
    [TestCase("desktop")]
    public async Task WithTargetDevice_OutputsCorrectContent(string device)
    {
        var messageBuilder = new PushoverMessageBuilder();
        messageBuilder.WithTargetDevice(device);

        var requestBuilder = new PushoverRequestBuilder("abcdef");
        messageBuilder.ConfigureRequest(requestBuilder);

        string output = (await requestBuilder.Content.ReadAsStringAsync()).TrimEnd();
        Assert.That(output, Is.EqualTo($"""
                                        --abcdef
                                        Content-Type: text/plain; charset=utf-8
                                        Content-Disposition: form-data; name=device

                                        {device}
                                        --abcdef--
                                        """));
    }
}