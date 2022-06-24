#if !OPENZUNE

using System.Net;

namespace Microsoft.Zune.Service
{
    partial class Escargot
    {
        public static bool TrySignIn(string username, string password)
        {
            var request = WebRequest.Create(ESCARGOT_NOTRST_URL);
            request.Headers.Add(HEADER_X_USER, username);
            request.Headers.Add(HEADER_X_PASS, password);

            try
            {
                var response = request.GetResponse();
                string token = response.Headers[HEADER_X_TOKEN];

                return TrySetCredentials(token, username);
            }
            catch
            {
                return false;
            }
        }
    }
}

#endif
