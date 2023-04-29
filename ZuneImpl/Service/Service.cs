namespace Microsoft.Zune.Service
{
    public class Service2
    {
        private static readonly IService _instance = new CommunityService();
        public static IService Instance => _instance;
    }

    public enum EMediaFormat
    {
    }

    public enum EMediaRights
    {
    }
}