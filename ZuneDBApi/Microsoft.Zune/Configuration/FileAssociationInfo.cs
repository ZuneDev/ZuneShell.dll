using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
    public class FileAssociationInfo
    {
        private string m_extension;
        private string m_progId;
        private string m_description;
        private EMediaTypes m_eMediaType;
        private bool m_isCurrentlyOwned;

        [return: MarshalAs(UnmanagedType.U1)]
        public bool IsCurrentlyOwned
        {
            get { return m_isCurrentlyOwned; }
            set { m_isCurrentlyOwned = value; }
        }

        public EMediaTypes MediaType => m_eMediaType;
        public string Description => m_description;
        public string ProgId => m_progId;
        public string Extension => m_extension;

        internal FileAssociationInfo(string extension, string progId, string description, EMediaTypes mediaType, bool isCurrentlyOwned)
        {
            m_extension = extension;
            m_progId = progId;
            m_description = description;
            m_eMediaType = mediaType;
            m_isCurrentlyOwned = isCurrentlyOwned;
        }
    }
}