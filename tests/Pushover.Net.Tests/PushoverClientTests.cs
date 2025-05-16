using System.Net;

using Microsoft.Extensions.Options;

using NSubstitute;

namespace Pushover.Net.Tests;

public class PushoverClientTests
{
    [Test]
    public async Task SendMessageAsync_WithOkResponse_ReturnsExpectedResults()
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.OK, "{\"status\":1,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        PushoverSendMessageResponse response = await client.SendMessageAsync(msg => msg.WithMessage("Hello world!"), CancellationToken.None);
        Assert.That(response.Status, Is.EqualTo(PushoverResponseStatus.Success));
        Assert.That(response.Request, Is.EqualTo(Guid.Parse("257D9399-AB64-4F4F-BF5E-CE9175553A1D")));
    }

    [Test]
    public void SendMessageAsync_WithBadRequest_ReturnsExpectedResults()
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.BadRequest, "{\"status\":0,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        Assert.ThrowsAsync<PushoverApiRequestFailedException>(async () => await client.SendMessageAsync(msg => msg.WithMessage("Hello world!"), CancellationToken.None));
    }

    [Test]
    public async Task ValidateUserOrGroupAsync_WithOkResponse_ReturnsExpectedResults()
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.OK, "{\"status\":1,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        PushoverUserValidationResponse response = await client.ValidateUserOrGroupAsync("userKey00000000000000000000000", "iphone", CancellationToken.None);
        Assert.That(response.Status, Is.EqualTo(PushoverResponseStatus.Success));
        Assert.That(response.Request, Is.EqualTo(Guid.Parse("257D9399-AB64-4F4F-BF5E-CE9175553A1D")));
    }

    [Test]
    public void ValidateUserOrGroupAsync_WithInvalidKey_ThrowsInvalidOperationException()
    {
        HttpMessageHandler? handler = Substitute.For<HttpMessageHandler>();

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(handler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await client.ValidateUserOrGroupAsync("userKey000000000000000000000_", "iphone", CancellationToken.None));
    }

    [Test]
    public void ValidateUserOrGroupAsync_WithMissingKey_ThrowsArgumentException()
    {
        HttpMessageHandler? handler = Substitute.For<HttpMessageHandler>();

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(handler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        Assert.ThrowsAsync<ArgumentException>(async () => await client.ValidateUserOrGroupAsync("", "iphone", CancellationToken.None));
    }

    [Test]
    public async Task GetReceiptStatusAsync_WithOkResponse_ReturnsExpectedResults()
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.OK, "{\"status\":1,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        PushoverReceiptStatusResponse response = await client.GetReceiptStatusAsync("receipt00000000000000000000000", CancellationToken.None);
        Assert.That(response.Status, Is.EqualTo(PushoverResponseStatus.Success));
        Assert.That(response.Request, Is.EqualTo(Guid.Parse("257D9399-AB64-4F4F-BF5E-CE9175553A1D")));
    }

    [Test]
    public async Task CancelRetriesAsync_WithOkResponse_ReturnsExpectedResults()
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.OK, "{\"status\":1,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        PushoverCancelRetriesResponse response = await client.CancelRetriesAsync("000000000000000000000000000000", CancellationToken.None);
        Assert.That(response.Status, Is.EqualTo(PushoverResponseStatus.Success));
        Assert.That(response.Request, Is.EqualTo(Guid.Parse("257D9399-AB64-4F4F-BF5E-CE9175553A1D")));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CancelRetriesAsync_InvalidReceiptId_ThrowsArgumentNullException(string? receiptId)
    {
        var testHandler = new TestHttpMessageHandler();
        testHandler.Returns(HttpStatusCode.OK, "{\"status\":1,\"request\":\"257D9399-AB64-4F4F-BF5E-CE9175553A1D\"}");

        IHttpClientFactory? httpClientFactory = Substitute.For<IHttpClientFactory>();
        var httpClient = new HttpClient(testHandler);
        httpClientFactory.CreateClient().Returns(httpClient);

        PushoverOptions options = new PushoverOptions().WithApiToken("apiToken0000000000000000000000").WithDefaultUser("defaultUser0000000000000000000");
        var client = new PushoverClient(Options.Create(options), httpClientFactory);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.CancelRetriesAsync(receiptId!, CancellationToken.None));
    }
}