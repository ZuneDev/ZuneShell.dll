using System;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

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

    public static int HashToInt32(this string id)
    {
        var hashBytes = id.Hash();
        return BitConverter.ToInt32(hashBytes, 0) ^ BitConverter.ToInt32(hashBytes, 4);
    }

    public static Guid HashToGuid(this string id)
    {
        var hashBytes = id.Hash();
        return new Guid(hashBytes);
    }

    public static long HashToInt64(this string id)
    {
        var hashBytes = id.Hash();
        return BitConverter.ToInt64(hashBytes, 0);
    }

    public static byte[] Hash(this string id)
    {
        MD5 md5 = MD5.Create();
        return md5.ComputeHash(Encoding.Default.GetBytes(id));
    }
}
