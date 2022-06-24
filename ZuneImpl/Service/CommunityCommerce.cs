using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Zune.Xml.Commerce;

namespace Microsoft.Zune.Service
{
    internal static partial class CommunityCommerce
    {
        static readonly XmlSerializer _signInRequestSerializer = new XmlSerializer(typeof(SignInRequest));
        static readonly XmlSerializer _signInResponseSerializer = new XmlSerializer(typeof(SignInResponse));
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

        public static CultureInfo Culture { get; private set; }

        public static bool IsSignedIn => Escargot.HasToken && MemberInfo != null;

        public static void SignOut() => MemberInfo = null;

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
