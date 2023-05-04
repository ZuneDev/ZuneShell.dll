using Meziantou.Framework.Win32;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Zune.Service
{
    internal static partial class Escargot
    {
        const string ESCARGOT_NOTRST_URL = "http://login.zunes.me/NotRST.srf";
        const string HEADER_X_USER = "X-User";
        const string HEADER_X_PASS = "X-Password";
        const string HEADER_X_TOKEN = "X-Token";

        internal static readonly HttpClient _client = new();
        private static string _token;

        /// <summary>
        /// Gets the username of the currently authenticated user.
        /// </summary>
        public static string Username { get; private set; }

        public static bool HasToken => _token != null;

        public static void CacheToken()
        {
            if (_token == null)
                return;

            // Ensure that alternative Zune launchers can only access
            // credentials given explicitly to them by the user.
            string appName = Assembly.GetEntryAssembly().FullName;

            CredentialManager.DeleteCredential(appName);
            CredentialManager.WriteCredential(appName, Username, _token, CredentialPersistence.LocalMachine);
        }

        public static bool ReadCachedToken()
        {
            string appName = Assembly.GetEntryAssembly().FullName;

            var cred = CredentialManager.ReadCredential(appName);
            if (cred == null)
                return false;

            Username = cred.UserName;
            _token = cred.Password;
            return true;
        }

        public static void ClearToken()
        {
            _token = null;
            Username = null;
        }

        public static bool TrySignIn(string username, string password)
        {
            HttpRequestMessage request = new(HttpMethod.Post, ESCARGOT_NOTRST_URL);
            request.Headers.TryAddWithoutValidation(HEADER_X_USER, username);
            request.Headers.TryAddWithoutValidation(HEADER_X_PASS, password);

            try
            {
                var response =
#if NET5_0_OR_GREATER
                    _client.Send(request);
#else
                    Task.Run(() => _client.SendAsync(request)).Result;
#endif

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

        private static bool TrySetCredentials(string token, string username)
        {
            if (token == null)
                return false;

            Username = username;
            _token = token;
            return true;
        }
    }
}
