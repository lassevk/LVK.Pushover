namespace Pushover.Net.Tests;

public class PushoverRequestBuilderTests
{
    [TestCase("message", "Hello world!", "--abcdefg\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=message\r\n\r\nHello world!\r\n--abcdefg--\r\n")]
    [TestCase("message", "Hello \"world!\"", "--abcdefg\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=message\r\n\r\nHello \"world!\"\r\n--abcdefg--\r\n")]
    public async Task Encoding_WithTestCases_BehavesAsExpected(string key, string message, string expected)
    {
        var builder = new PushoverRequestBuilder("abcdefg");
        builder.Add(key, message);

        string result = await builder.Content.ReadAsStringAsync();
        Assert.That(result, Is.EqualTo(expected));
    }
}