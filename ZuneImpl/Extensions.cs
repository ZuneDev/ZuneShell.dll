using System;
using System.IO;
using System.Threading;

#if NETSTANDARD1_0_OR_GREATER
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace Microsoft.Zune
{
    internal static class Extensions
    {
#if NETSTANDARD2_0_OR_GREATER && !NET5_0_OR_GREATER
        // FIXME: Blocking the executing thread is generally a bad idea.
        // Executing some of these methods has a chance of
        // deadlocking the app.

        public static Stream ReadAsStream(this HttpContent content)
        {
            return content.ReadAsStreamAsync().Result;
        }

        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
        {
            return client.SendAsync(request).Result;
        }

        public static Task<Stream> GetStreamAsync(this HttpClient client, Uri uri, CancellationToken _)
        {
            return client.GetStreamAsync(uri);
        }
#endif

#if NETSTANDARD && !NETSTANDARD2_1_OR_GREATER
        public static Task CopyToAsync(this Stream source, Stream destination, CancellationToken _)
        {
            return source.CopyToAsync(destination);
        }
#endif
    }
}
