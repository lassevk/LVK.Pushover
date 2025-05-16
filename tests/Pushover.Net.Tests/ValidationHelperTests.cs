namespace Pushover.Net.Tests;

public class PushoverValidationHelperTests
{
    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("Hello world!", "Hello world!")]
    public void ValidateMessage_WithTestCases_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateMessage(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateMessage_MessageLongerThan1024Characters_ThrowsInvalidOperationException()
    {
        string message = new string('a', 1025);
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateMessage(message));
    }
}