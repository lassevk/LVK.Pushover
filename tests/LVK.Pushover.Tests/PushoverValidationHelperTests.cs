namespace LVK.Pushover.Tests;

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
    public void ValidateMessage_MessageWithLengthOf1024Characters_DoesNotThrowInvalidOperationException()
    {
        string message = new string('a', 1024);
        PushoverValidationHelper.ValidateMessage(message);
    }

    [Test]
    public void ValidateMessage_MessageLongerThan1024Characters_ThrowsInvalidOperationException()
    {
        string message = new string('a', 1025);
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateMessage(message));
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("Hello world!", "Hello world!")]
    public void ValidateTitle_WithTestCases_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateTitle(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateTitle_TitleWithLengthOf250Characters_DoesNotThrowInvalidOperationException()
    {
        string title = new string('a', 250);
        PushoverValidationHelper.ValidateTitle(title);
    }

    [Test]
    public void ValidateTitle_TitleLongerThan250Characters_ThrowsInvalidOperationException()
    {
        string title = new string('a', 251);
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateTitle(title));
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("000000000000000000000000000000", "000000000000000000000000000000")]
    [TestCase("111111111111111111111111111111", "111111111111111111111111111111")]
    public void ValidateUserOrGroupKey_WithTestCases_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateUserOrGroupKey(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase("000000000000000000000000000000", "000000000000000000000000000000")]
    [TestCase("012345678901234567890123456789", "012345678901234567890123456789")]
    public void ValidateUserOrGroupKey_WithOkTestCases_ReturnsExpectedResults(string input, string expected)
    {
        string? output = PushoverValidationHelper.ValidateUserOrGroupKey(input);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase("00000000000000000000000000000", TestName="Less than 30 characters")]
    [TestCase("0123456789012345678901234567891", TestName="More than 30 characters")]
    [TestCase("01234567890123456789012345678_", TestName="Invalid character")]
    public void ValidateUserOrGroupKey_WithInvalidTestCases_ThrowsInvalidOperationException(string input)
    {
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateUserOrGroupKey(input));
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("https://google.com", "https://google.com")]
    public void ValidateUrl_WithTestCases_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateUrl(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateUrl_UrlWithLengthOf256Characters_DoesNotThrowInvalidOperationException()
    {
        string title = new string('a', 256);
        PushoverValidationHelper.ValidateUrl(title);
    }

    [Test]
    public void ValidateUrl_UrlLongerThan256Characters_ThrowsInvalidOperationException()
    {
        string title = new string('a', 257);
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateUrl(title));
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("Hello world!", "Hello world!")]
    public void ValidateUrlTitle_WithTestCases_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateUrlTitle(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void ValidateUrlTitle_TitleWithLengthOf250Characters_DoesNotThrowInvalidOperationException()
    {
        string title = new string('a', 250);
        PushoverValidationHelper.ValidateUrlTitle(title);
    }

    [Test]
    public void ValidateUrlTitle_TitleLongerThan250Characters_ThrowsInvalidOperationException()
    {
        string title = new string('a', 251);
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateUrlTitle(title));
    }

    [TestCase(null, null)]
    [TestCase("", null)]
    [TestCase("000000000000000000000000000000", "000000000000000000000000000000")]
    public void ValidateMessage_ValidateApiToken_ReturnsExpectedResults(string? input, string? expected)
    {
        string? output = PushoverValidationHelper.ValidateApiToken(input);

        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase("000000000000000000000000000000", "000000000000000000000000000000")]
    [TestCase("012345678901234567890123456789", "012345678901234567890123456789")]
    public void ValidateApiToken_WithOkTestCases_ReturnsExpectedResults(string input, string expected)
    {
        string? output = PushoverValidationHelper.ValidateApiToken(input);
        Assert.That(output, Is.EqualTo(expected));
    }

    [TestCase("00000000000000000000000000000", TestName="Less than 30 characters")]
    [TestCase("0123456789012345678901234567891", TestName="More than 30 characters")]
    [TestCase("01234567890123456789012345678_", TestName="Invalid character")]
    public void ValidateApiToken_WithInvalidTestCases_ThrowsInvalidOperationException(string input)
    {
        Assert.Throws<InvalidOperationException>(() => PushoverValidationHelper.ValidateApiToken(input));
    }
}