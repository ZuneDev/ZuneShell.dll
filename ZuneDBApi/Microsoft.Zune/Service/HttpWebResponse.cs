using System;
using System.IO;
using System.Net;

namespace Microsoft.Zune.Service;

// Original wraps a native IStream* obtained from a custom native HTTP stack
// (IHttpWebRequest). Unlike the rest of that stack (see
// logs/Microsoft.Zune/Service/HttpWebRequest.md), HTTP response handling has a
// straightforward, fully-specified equivalent in the BCL — this wraps a real
// System.Net.Http.HttpResponseMessage instead of stubbing.
public class HttpWebResponse : IDisposable
{
    private readonly System.Net.Http.HttpResponseMessage _response;
    private Stream _stream;

    public HttpStatusCode StatusCode => _response.StatusCode;

    public DateTime Expires => _response.Content.Headers.Expires?.UtcDateTime ?? DateTime.MinValue;

    public string ContentType => _response.Content.Headers.ContentType?.ToString();

    public long ContentLength => _response.Content.Headers.ContentLength ?? -1;

    public Version ProtocolVersion => _response.Version;

    public Uri ResponseUri => _response.RequestMessage?.RequestUri;

    internal HttpWebResponse(System.Net.Http.HttpResponseMessage response)
    {
        _response = response;
    }

    public Stream GetResponseStream()
    {
        return _stream ??= _response.Content.ReadAsStream();
    }

    public void Close()
    {
        Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stream?.Dispose();
            _response.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
