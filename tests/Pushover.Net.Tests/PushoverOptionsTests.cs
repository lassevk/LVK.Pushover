namespace Pushover.Net.Tests;

public class PushoverOptionsTests
{
    [TestCase("000000000000000000000000000000")]
    [TestCase("111111111111111111111111111111")]
    public void WithApiToken_WithOkTestCases_StoresToken(string token)
    {
        var options = new PushoverOptions();
        options.WithApiToken(token);

        Assert.That(options.ApiToken, Is.EqualTo(token));
    }

    [TestCase("00000000000000000000000000000", TestName="Less than 30 characters")]
    [TestCase("1111111111111111111111111111111", TestName="More than 30 characters")]
    [TestCase("11111111111111111111111111111_", TestName="Invalid character")]
    public void WithApiToken_WithInvalidTestCases_ThrowsInvalidOperationException(string token)
    {
        var options = new PushoverOptions();
        Assert.Throws<InvalidOperationException>(() => options.WithApiToken(token));
    }

    [TestCase("000000000000000000000000000000")]
    [TestCase("111111111111111111111111111111")]
    public void WithDefaultUser_WithOkTestCases_StoresUserKey(string userKey)
    {
        var options = new PushoverOptions();
        options.WithDefaultUser(userKey);

        Assert.That(options.DefaultUserKey, Is.EqualTo(userKey));
    }

    [TestCase("00000000000000000000000000000", TestName="Less than 30 characters")]
    [TestCase("1111111111111111111111111111111", TestName="More than 30 characters")]
    [TestCase("11111111111111111111111111111_", TestName="Invalid character")]
    public void WithDefaultUser_WithInvalidTestCases_ThrowsInvalidOperationException(string userKey)
    {
        var options = new PushoverOptions();
        Assert.Throws<InvalidOperationException>(() => options.WithDefaultUser(userKey));
    }

    [Test]
    public void Validate_NoApiToken_ThrowsInvalidOperationException()
    {
        var options = new PushoverOptions();
        Assert.Throws<InvalidOperationException>(() => options.Validate());
    }

    [Test]
    public void Validate_GotApiToken_DoesNotThrowInvalidOperationException()
    {
        var options = new PushoverOptions();
        options.WithApiToken("000000000000000000000000000000");
        options.Validate();
    }
}