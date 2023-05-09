using System;
using System.Collections.Generic;

namespace Microsoft.Zune.Library;

public class ZuneQueryList : List<object>, IDisposable
{
    internal ZuneQueryList(EQueryType queryType = EQueryType.eQueryTypeInvalid)
    {
        QueryType = queryType;
    }

    public bool IsEmpty => Count == 0;

    public bool IsDisposed { get; protected set; }

    public EQueryType QueryType { get; set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}
