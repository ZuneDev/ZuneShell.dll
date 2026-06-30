using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Configuration
{
    internal class FileAssociationHandlerWrapper : IFileAssociationHandler, IDisposable
    {
        public virtual bool CanAssociationBeChanged() { throw new NotImplementedException(); }
        public virtual int GetFileAssociationInfoList(out IList<FileAssociationInfo> fileAssociationInfoList) { fileAssociationInfoList = new List<FileAssociationInfo>(); return -1; }
        public virtual int SetFileAssociationInfo(IList<FileAssociationInfo> fileAssociationInfoList) { throw new NotImplementedException(); }
        ~FileAssociationHandlerWrapper() { }
        internal FileAssociationHandlerWrapper() { }
        internal int FileInfoToStruct(FileAssociationInfo fileAssocInfo, IntPtr pFileAssocInfo) { throw new NotImplementedException(); }
        internal void CleanupFileInfoArray(IntPtr rgFileInfo, uint cFileInfo) { }
        protected virtual void Dispose(bool P_0) { }
        public virtual void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
    }
}