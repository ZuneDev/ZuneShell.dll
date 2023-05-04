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
        public static Stream ReadAsStream(this HttpContent content)
        {
            return Task.Run(content.ReadAsStreamAsync).Result;
        }

        public static HttpResponseMessage Send(this HttpClient client, HttpRequestMessage request)
        {
            return Task.Run(() => client.SendAsync(request)).Result;
        }

        public static Task<Stream> GetStreamAsync(this HttpClient client, Uri uri, CancellationToken _)
        {
            return client.GetStreamAsync(uri);
        }
#endif
    }
}
