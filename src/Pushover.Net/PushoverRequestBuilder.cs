namespace Pushover.Net;

internal class PushoverRequestBuilder
{
    private readonly MultipartFormDataContent _content = new();

    public PushoverRequestBuilder()
    {
    }

    public void Add(string key, string? value)
    {
        if (value is not null)
        {
            _content.Add(new StringContent(value), key);
        }
    }

    public HttpContent Content => _content;
}