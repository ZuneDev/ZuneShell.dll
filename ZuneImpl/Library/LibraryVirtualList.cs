#if OPENZUNE

using Microsoft.Iris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Zune.Library
{
    internal class LibraryVirtualList : VirtualList, ISearchableList
    {
        private StrixLibraryDataProviderQuery m_owner;
        private ZuneQueryList m_pQueryList;
        private string m_itemTypeName;
        private object m_itemTypeCookie;
        private bool m_autoRefresh;
        private bool m_useSlowDataRequests;
        private bool m_antialiasImageEdges;

        public LibraryVirtualList(StrixLibraryDataProviderQuery owner, ZuneQueryList pQueryList,
            bool autoRefresh, bool antialiasEdges)
            : base(true)
        {
            try
            {
                this.m_owner = owner;
                base.StoreQueryResults = true;
                this.m_pQueryList = pQueryList;
                //pQueryList.AddRef();
                //this.m_fBlockChanges = false;
                //this.m_fEndBulkArrivedDuringBlock = false;
                this.m_antialiasImageEdges = antialiasEdges;
                this.m_autoRefresh = autoRefresh;
                if (autoRefresh)
                {
                    //this.m_pQueryList.Advise(this);
                }
                //base.Count = this.m_pQueryList.Count;

                Add("Howdy there!");
            }
            catch
            {
                base.Dispose(ModelItemDisposeMode.KeepOwnerReference);
                throw;
            }
        }

        internal void SetItemTypeCookie(string itemTypeName, object itemTypeCookie)
        {
            this.m_itemTypeName = itemTypeName;
            this.m_itemTypeCookie = itemTypeCookie;
        }

        public int SearchForString(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}

#endif