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
}

public class TestHttpMessageHandler : HttpMessageHandler
{
    private HttpStatusCode _statusCode = HttpStatusCode.OK;
    private HttpContent? _responseContent;

    public void Returns(HttpStatusCode statusCode, string json)
    {
        _statusCode = statusCode;
        _responseContent = new StringContent(json);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        return new HttpResponseMessage(_statusCode)
        {
            Content = _responseContent
        };
    }
}