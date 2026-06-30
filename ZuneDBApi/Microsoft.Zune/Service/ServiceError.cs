using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ZuneUI;

namespace Microsoft.Zune.Service;

public class ServiceError : IDisposable
{
    private readonly object m_spServiceError; // Stub for CComPtrMgd<IServiceError>
    private IList<PropertyError> m_propertyErrors;

    public IList<PropertyError> PropertyErrors
    {
        get
        {
            if (m_propertyErrors == null)
            {
                m_propertyErrors = new List<PropertyError>();
            }
            return m_propertyErrors;
        }
    }

    public HRESULT RootError
    {
        get
        {
            return HRESULT._S_OK;
        }
    }

    internal ServiceError()
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    ~ServiceError()
    {
    }
}
