using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
    public class FileAssociationInfo
    {
        [return: MarshalAs(UnmanagedType.U1)]
        public bool IsCurrentlyOwned { get; set; }
        public EMediaTypes MediaType { get; }
        public string Description { get; }
        public string ProgId { get; }
        public string Extension { get; }

        internal FileAssociationInfo(string extension, string progId, string description, EMediaTypes mediaType, bool isCurrentlyOwned)
        {
            Extension = extension;
            ProgId = progId;
            Description = description;
            MediaType = mediaType;
            IsCurrentlyOwned = isCurrentlyOwned;
        }
    }
}