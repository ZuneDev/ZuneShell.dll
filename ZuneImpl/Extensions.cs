using System;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Zune;

internal static class Extensions
{
#if NETSTANDARD2_0_OR_GREATER && !NET5_0_OR_GREATER
    public static Stream ReadAsStream(this HttpContent content)
    {
        return AsyncHelper.Run(content.ReadAsStreamAsync);
    }

    public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
    {
        return AsyncHelper.Run(() => client.SendAsync(request));
    }

    public static Task<Stream> GetStreamAsync(this HttpClient client, Uri uri, CancellationToken _)
    {
        return client.GetStreamAsync(uri);
    }
#endif
}
