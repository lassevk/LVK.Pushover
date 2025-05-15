using System.Net.Http.Headers;

namespace Pushover.Net;

internal class PushoverRequestBuilder
{
    private readonly MultipartFormDataContent _content;

    public PushoverRequestBuilder(string? multipartBoundary = null)
    {
        if (string.IsNullOrWhiteSpace(multipartBoundary))
        {
            _content = new MultipartFormDataContent();
        }
        else
        {
            _content = new MultipartFormDataContent(multipartBoundary);
        }
    }

    public void AddIfNotNullOrEmpty(string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            _content.Add(new StringContent(value), key);
        }
    }

    public void AddAttachment(byte[] attachment, string attachmentName, string attachmentType)
    {
        var childContent = new ByteArrayContent(attachment);
        childContent.Headers.ContentType = new MediaTypeHeaderValue(attachmentType);
        _content.Add(childContent, "attachment", attachmentName);
    }

    public HttpContent Content => _content;
}