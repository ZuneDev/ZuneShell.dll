#if OPENZUNE

using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace Microsoft.Zune.Library
{
    // Token: 0x02000271 RID: 625
    internal class StrixLibraryDataProviderQueryResult : DataProviderObject
    {
        // Token: 0x06000F6A RID: 3946 RVA: 0x000E90D0 File Offset: 0x000E84D0
        public StrixLibraryDataProviderQueryResult(StrixLibraryDataProviderQuery owner, LibraryVirtualList virtualListResultSet, object resultTypeCookie) : base(owner, resultTypeCookie)
        {
            this.m_virtualListResultSet = virtualListResultSet;
            if (virtualListResultSet != null)
            {
                foreach (DataProviderMapping dataProviderMapping in base.Mappings.Values)
                {
                    if (dataProviderMapping.UnderlyingCollectionTypeCookie != null)
                    {
                        this.m_virtualListResultSet.SetItemTypeCookie(dataProviderMapping.UnderlyingCollectionTypeName, dataProviderMapping.UnderlyingCollectionTypeCookie);
                    }
                }
            }
        }

        // Token: 0x06000F6B RID: 3947 RVA: 0x000E9160 File Offset: 0x000E8560
        public override object GetProperty(string propertyName)
        {
            DataProviderMapping dataProviderMapping = null;
            if (propertyName == "IsEmpty")
            {
                return this.m_isEmpty;
            }
            if (base.Mappings.TryGetValue(propertyName, out dataProviderMapping))
            {
                return this.m_virtualListResultSet;
            }
            return null;
        }

        // Token: 0x06000F6C RID: 3948 RVA: 0x00101710 File Offset: 0x00100B10
        public override void SetProperty(string propertyName, object value)
        {
            throw new NotSupportedException();
        }

        // Token: 0x06000F6D RID: 3949 RVA: 0x000E91A8 File Offset: 0x000E85A8
        public void SetIsEmpty([MarshalAs(UnmanagedType.U1)] bool isEmpty)
        {
            if (this.m_isEmpty != isEmpty)
            {
                this.m_isEmpty = isEmpty;
                base.FirePropertyChanged("IsEmpty");
            }
        }

        // Token: 0x04000B3C RID: 2876
        protected LibraryVirtualList m_virtualListResultSet;

        // Token: 0x04000B3D RID: 2877
        protected bool m_isEmpty;
    }
}

#endif