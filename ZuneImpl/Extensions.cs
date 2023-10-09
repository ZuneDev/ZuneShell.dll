using System;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

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

    /// <summary>
    /// Returns a value that indicates whether there is an object at the top of the System.Collections.Generic.Stack`1,
    /// and if one is present, copies it to the result parameter, and removes it from
    /// the System.Collections.Generic.Stack`1.
    /// </summary>
    /// <param name="result">
    /// If present, the object at the top of the System.Collections.Generic.Stack`1;
    /// otherwise, the default value of T.
    /// </param>
    /// <returns>
    /// true if there is an object at the top of the System.Collections.Generic.Stack`1;
    /// false if the System.Collections.Generic.Stack`1 is empty.
    /// </returns>
    public static bool TryPop<T>(this Stack<T> stack, [MaybeNullWhen(false)] out T result)
    {
        if (stack.Count > 0)
        {
            result = stack.Pop();
            return true;
        }

        result = default;
        return false;
    }
}
