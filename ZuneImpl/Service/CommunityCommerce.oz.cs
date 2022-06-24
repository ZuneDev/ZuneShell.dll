#if OPENZUNE

using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using Zune.Xml.Commerce;
using ZuneUI;

namespace Microsoft.Zune.Service
{
    partial class CommunityCommerce
    {
        private static readonly HttpClient _client = Escargot._client;

        public static HRESULT TrySignIn()
        {
            if (!Escargot.HasToken)
                return HRESULT._NS_E_SUBSCRIPTIONSERVICE_LOGIN_FAILED;

            try
            {
                HttpRequestMessage request = new(HttpMethod.Post, "https://commerce.zunes.me/v5.0/en-US/account/signin");
                request.Headers.UserAgent.ParseAdd("Zune/5.0");

                var contentStream = GetSignInRequestBody();
                request.Content = new StreamContent(contentStream);
                request.Content.Headers.ContentType = new(Atom.Constants.ATOM_MIMETYPE);
                request.Content.Headers.ContentLength = contentStream.Length;

                Escargot.AuthenticateRequest(request);

                var response = _client.Send(request);
                response.EnsureSuccessStatusCode();

                var responseObj = ReadSignInResponse(response.Content.ReadAsStream());

                uint? errorCode = responseObj?.AccountState?.SignInErrorCode;
                if (errorCode != 0)
                    return new HRESULT(unchecked((int)errorCode));

                MemberInfo = responseObj;
                return HRESULT._S_OK;
            }
            catch
            {
                return HRESULT._NS_E_SUBSCRIPTIONSERVICE_LOGIN_FAILED;
            }
        }
    }
}

#endif
