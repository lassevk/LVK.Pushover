using System.Net;

namespace Pushover.Net.Tests;

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