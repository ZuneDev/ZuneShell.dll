using System.Linq;
using System.Net.Http;

namespace Microsoft.Zune.Service
{
    partial class Escargot
    {
        internal static readonly HttpClient _client = new();

        public static bool TrySignIn(string username, string password)
        {
            HttpRequestMessage request = new(HttpMethod.Post, ESCARGOT_NOTRST_URL);
            request.Headers.TryAddWithoutValidation(HEADER_X_USER, username);
            request.Headers.TryAddWithoutValidation(HEADER_X_PASS, password);

            try
            {
                var response = _client.Send(request);
                response.EnsureSuccessStatusCode();

                string token = response.Headers.GetValues(HEADER_X_TOKEN).FirstOrDefault();

                return TrySetCredentials(token, username);
            }
            catch
            {
                return false;
            }
        }

        public static void AuthenticateRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = new("Bearer", _token);
        }
    }
}
