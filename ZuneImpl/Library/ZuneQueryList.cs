using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Zune.Library
{
    public class ZuneQueryList : IDisposable
    {
        internal ZuneQueryList(EQueryType queryType)
        {

        }

        public bool IsEmpty { get; protected set; }

        public bool IsDisposed { get; protected set; }

        public int Count { get; protected set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
