using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Zune.Xml.Commerce;
using ZuneUI;

namespace Microsoft.Zune.Service
{
    internal static partial class CommunityCommerce
    {
        static readonly XmlSerializer _signInRequestSerializer = new(typeof(SignInRequest));
        static readonly XmlSerializer _signInResponseSerializer = new(typeof(SignInResponse));
        static readonly HttpClient _client = Escargot._client;
        static SignInResponse _memberInfo;

        public static SignInResponse MemberInfo
        {
            get => _memberInfo;
            private set
            {
                _memberInfo = value;

                Culture = new CultureInfo(value.AccountInfo.Locale);
            }
        }

        public static CultureInfo Culture { get; private set; } = CultureInfo.CurrentCulture;

        public static bool IsSignedIn => Escargot.HasToken && MemberInfo != null;

        public static void SignOut() => MemberInfo = null;

        public static HRESULT TrySignIn()
        {
            if (!Escargot.HasToken)
                return HRESULT._NS_E_SUBSCRIPTIONSERVICE_LOGIN_FAILED;

            try
            {
                string url = Service2.Instance.GetEndPointUri(EServiceEndpointId.SEID_Commerce) + $"/account/signin";
                HttpRequestMessage request = new(HttpMethod.Post, url);
                request.Headers.UserAgent.ParseAdd(CommunityService.AppUserAgent);

                var contentStream = GetSignInRequestBody();
                request.Content = new StreamContent(contentStream);
                request.Content.Headers.ContentType = new(Atom.Constants.ATOM_MIMETYPE);
                request.Content.Headers.ContentLength = contentStream.Length;

                Escargot.AuthenticateRequest(request);

                var response =
#if NET5_0_OR_GREATER
                    _client.Send(request);
#else
                    Task.Run(() => _client.SendAsync(request)).Result;
#endif

                response.EnsureSuccessStatusCode();

                System.IO.Stream responseStream =
#if NET5_0_OR_GREATER
                    response.Content.ReadAsStream();
#else
                    Task.Run(response.Content.ReadAsStreamAsync).Result;
#endif

                var responseObj = ReadSignInResponse(responseStream);

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

        private static Stream GetSignInRequestBody()
        {
            var requestObj = new SignInRequest
            {
                TunerInfo = new TunerInfo
                {
                    Version = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString()
                }
            };
            var contentStream = new MemoryStream();
            _signInRequestSerializer.Serialize(contentStream, requestObj);

            contentStream.Position = 0;
            return contentStream;
        }

        private static SignInResponse ReadSignInResponse(Stream stream)
        {
            return _signInResponseSerializer.Deserialize(stream) as SignInResponse;
        }
    }
}
