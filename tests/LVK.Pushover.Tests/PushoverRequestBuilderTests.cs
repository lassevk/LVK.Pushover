namespace LVK.Pushover.Tests;

public class PushoverRequestBuilderTests
{
    [Test]
    public async Task Constructor_WithNullBoundary_AssignsMultipartBoundaryAutomatically()
    {
        var builder1 = new PushoverRequestBuilder();
        builder1.AddIfNotNullOrEmpty("key", "value");

        var builder2 = new PushoverRequestBuilder();
        builder2.AddIfNotNullOrEmpty("key", "value");

        string content1 = await builder1.Content.ReadAsStringAsync();
        string content2 = await builder2.Content.ReadAsStringAsync();

        Assert.That(content1, Is.Not.EqualTo(content2));
    }

    [TestCase("message", "Hello world!", "--abcdefg\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=message\r\n\r\nHello world!\r\n--abcdefg--\r\n")]
    [TestCase("message", "Hello \"world!\"", "--abcdefg\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=message\r\n\r\nHello \"world!\"\r\n--abcdefg--\r\n")]
    public async Task Encoding_WithTestCases_BehavesAsExpected(string key, string message, string expected)
    {
        var builder = new PushoverRequestBuilder("abcdefg");
        builder.AddIfNotNullOrEmpty(key, message);

        string result = await builder.Content.ReadAsStringAsync();
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public async Task AddAttachment_WithBasicAttachment_ProducesCorrectOutput()
    {
        var builder = new PushoverRequestBuilder("abcdefg");
        var attachment = "Hello"u8;
        builder.AddAttachment(attachment, "test.txt", "text/plain");

        string result = await builder.Content.ReadAsStringAsync();
        string expected = "--abcdefg\r\n" +
                         "Content-Type: text/plain\r\n" +
                         "Content-Disposition: form-data; name=attachment; filename=test.txt; filename*=utf-8''test.txt\r\n\r\n" +
                         "Hello\r\n" +
                         "--abcdefg--\r\n";

        Assert.That(result, Is.EqualTo(expected));
    }
}