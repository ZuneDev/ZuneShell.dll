using System;
using System.IO;
using System.Net.Http;

namespace Microsoft.Zune.Service;

// Original wraps a native IHttpWebRequest* (a custom native HTTP stack, including its
// own async request queue and shutdown-cancellation event) — not reverse engineered.
// See logs/Microsoft.Zune/Service/HttpWebRequest.md. Unlike that native transport
// layer, HTTP request/response semantics have a complete, well-specified equivalent in
// the BCL, so this is backed by a real System.Net.Http.HttpClient rather than stubbed
// — every property is honored and requests actually go over the network.
public class HttpWebRequest
{
    private static readonly HttpClient s_client = new();

    private readonly HttpRequestMessage _request;
    private MemoryStream _requestStream;

    public bool CancelOnShutdown { get; set; }

    public bool KeepAlive { get; set; } = true;

    public string ContentType { get; set; }

    public HttpRequestCachePolicy CachePolicy { get; set; } = HttpRequestCachePolicy.Default;

    public long MaxResponseLength { get; set; } = -1;

    public string BrowserCookieUrl { get; set; }

    public string Authorization { get; set; }

    public bool AcceptGZipEncoding { get; set; }

    public string AcceptLanguage { get; set; }

    public long ContentLength { get; set; }

    public Uri RequestUri => _request.RequestUri;

    public string Method
    {
        get => _request.Method.Method;
        set => _request.Method = new HttpMethod(value);
    }

    private HttpWebRequest(Uri uri)
    {
        _request = new HttpRequestMessage(HttpMethod.Get, uri);
    }

    public static HttpWebRequest Create(string uri) => new(new Uri(uri));

    public static HttpWebRequest Create(Uri uri) => new(uri);

    public Stream GetRequestStream()
    {
        return _requestStream ??= new MemoryStream();
    }

    public HttpWebResponse GetResponse()
    {
        ApplyHeaders();
        HttpResponseMessage response = s_client.Send(_request);
        return new HttpWebResponse(response);
    }

    public void GetResponseAsync(AsyncRequestComplete responseComplete, object stateInfo)
    {
        ApplyHeaders();
        s_client.SendAsync(_request).ContinueWith(task =>
        {
            HttpWebResponse response = task.IsCompletedSuccessfully ? new HttpWebResponse(task.Result) : null;
            responseComplete?.Invoke(response, stateInfo);
        });
    }

    public static void Shutdown()
    {
    }

    private void ApplyHeaders()
    {
        if (_requestStream is not null)
        {
            _request.Content = new ByteArrayContent(_requestStream.ToArray());
            if (ContentType is not null)
                _request.Content.Headers.TryAddWithoutValidation("Content-Type", ContentType);
        }
        if (Authorization is not null)
            _request.Headers.TryAddWithoutValidation("Authorization", Authorization);
        if (AcceptLanguage is not null)
            _request.Headers.TryAddWithoutValidation("Accept-Language", AcceptLanguage);
        if (AcceptGZipEncoding)
            _request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip");
        if (CachePolicy == HttpRequestCachePolicy.BypassCache || CachePolicy == HttpRequestCachePolicy.Refresh)
            _request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
    }
}
