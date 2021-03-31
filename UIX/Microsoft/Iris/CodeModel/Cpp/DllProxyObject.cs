// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllProxyObject
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.RenderAPI;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllProxyObject : NotifyObjectBase, ISchemaInfo, IDisposableObject
    {
        private IntPtr _nativeObject;
        private ulong _handle;
        private DllTypeSchemaBase _type;
        private static object s_finalizeLock = new object();
        private static bool s_pendingAppThreadRelease = false;
        private static Vector<DllProxyObject.AppThreadReleaseEntry> s_pendingReleases;
        private static SimpleCallback s_releaseOnAppThread = new SimpleCallback(DllProxyObject.ReleaseFinalizedObjects);
        private static DllProxyObjectHandleTable s_handleTable;

        public static void CreateHandleTable() => DllProxyObject.s_handleTable = new DllProxyObjectHandleTable();

        public static void ReleaseOutstandingProxies()
        {
            DllProxyObject.s_pendingAppThreadRelease = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            foreach (IDisposableObject disposableObject in s_handleTable)
                disposableObject.Dispose(disposableObject);
            DllProxyObject.ReleaseFinalizedObjects();
        }

        public static DllProxyObject Wrap(IntPtr nativeObject)
        {
            DllProxyObject dllProxyObject = null;
            uint typeID;
            if (!NativeApi.SUCCEEDED(NativeApi.SpGetTypeID(nativeObject, out typeID)))
                return null;
            TypeSchema type = DllLoadResult.MapType(typeID);
            if (type != null)
            {
                dllProxyObject = DllProxyObject.GetExistingProxy(nativeObject) ?? DllProxyObject.WrapNewObject(nativeObject, type);
                NativeApi.SpReleaseExternalObject(nativeObject);
            }
            return dllProxyObject;
        }

        private static DllProxyObject GetExistingProxy(IntPtr nativeObject)
        {
            DllProxyObject dllProxyObject = null;
            ulong state;
            NativeApi.SpGetStateCache(nativeObject, out state);
            if (state != 0UL && !DllProxyObject.s_handleTable.LookupByHandle(state, out dllProxyObject))
                ErrorManager.ReportError("IUIXObject::GetStateCache retrieved unexpected value");
            return dllProxyObject;
        }

        private static DllProxyObject WrapNewObject(IntPtr nativeObject, TypeSchema type)
        {
            DllProxyObject dllProxyObject = null;
            uint marshalAs;
            IntPtr nativeImpl;
            if (DllProxyObject.DetermineProxyInterfaceForObject(nativeObject, type, out marshalAs, out nativeImpl))
            {
                switch (marshalAs)
                {
                    case 4294967281:
                    case 4294967286:
                    case 4294967287:
                    case 4294967288:
                    case 4294967289:
                    case 4294967290:
                    case 4294967291:
                        dllProxyObject = new DllProxyObject();
                        break;
                    case 4294967285:
                        dllProxyObject = new DllProxyList();
                        break;
                    case uint.MaxValue:
                        dllProxyObject = new DllProxyObject();
                        break;
                    default:
                        dllProxyObject = new DllProxyObject();
                        break;
                }
                dllProxyObject.Load(nativeObject, nativeImpl, (DllTypeSchemaBase)type);
            }
            return dllProxyObject;
        }

        private static bool DetermineProxyInterfaceForObject(
          IntPtr nativeObject,
          TypeSchema type,
          out uint marshalAs,
          out IntPtr nativeImpl)
        {
            bool flag = false;
            DllTypeSchemaBase dllTypeSchemaBase = (DllTypeSchemaBase)type;
            marshalAs = dllTypeSchemaBase.MarshalAs;
            if (marshalAs == uint.MaxValue)
            {
                flag = true;
                nativeImpl = IntPtr.Zero;
            }
            else if (DllProxyObject.CheckNativeReturn(NativeApi.SpQueryForMarshalAsInterface(nativeObject, marshalAs, out nativeImpl)) && nativeImpl != IntPtr.Zero)
                flag = true;
            else
                ErrorManager.ReportError("Object didn't implement expected interface '{0}'", marshalAs);
            return flag;
        }

        protected DllProxyObject() => this._handle = 0UL;

        ~DllProxyObject() => DllProxyObject.RegisterAppThreadRelease(new DllProxyObject.AppThreadReleaseEntry(this._nativeObject, this._handle, this.OwningLoadResult));

        protected static void RegisterAppThreadRelease(DllProxyObject.AppThreadReleaseEntry entry)
        {
            lock (DllProxyObject.s_finalizeLock)
            {
                if (DllProxyObject.s_pendingReleases == null)
                    DllProxyObject.s_pendingReleases = new Vector<DllProxyObject.AppThreadReleaseEntry>();
                DllProxyObject.s_pendingReleases.Add(entry);
                if (DllProxyObject.s_pendingAppThreadRelease)
                    return;
                DllProxyObject.s_pendingAppThreadRelease = true;
                DeferredCall.Post(DispatchPriority.Idle, DllProxyObject.s_releaseOnAppThread);
            }
        }

        private void Load(IntPtr nativeObject, IntPtr marshalAs, DllTypeSchemaBase type)
        {
            this._type = type;
            this._nativeObject = nativeObject;
            this.OwningLoadResult.RegisterProxyUsage();
            this._handle = DllProxyObject.s_handleTable.RegisterProxy(this);
            NativeApi.SpSetStateCache(this._nativeObject, this._handle);
            NativeApi.SpAddRefExternalObject(this._nativeObject);
            this.LoadWorker(nativeObject, marshalAs);
        }

        protected virtual void LoadWorker(IntPtr nativeObject, IntPtr nativeMarshalAs)
        {
        }

        public static HRESULT OnChangeNotification(IntPtr nativeObject, uint id)
        {
            HRESULT hresult = new HRESULT(0);
            DllProxyObject existingProxy = DllProxyObject.GetExistingProxy(nativeObject);
            if (existingProxy != null)
            {
                string id1 = null;
                if (existingProxy._type is DllTypeSchema type)
                    id1 = type.MapChangeID(id);
                if (id1 != null)
                {
                    existingProxy.FireNotification(id1);
                }
                else
                {
                    ErrorManager.ReportError("Invalid UIXID '0x{0:X8}' passed to NotifyChange", id);
                    hresult = new HRESULT(-2147024809);
                }
            }
            return hresult;
        }

        public ulong Handle => this._handle;

        public IntPtr NativeObject => this._nativeObject;

        public TypeSchema TypeSchema => _type;

        protected DllLoadResult OwningLoadResult => this._type.Owner as DllLoadResult;

        private static bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXObject");

        bool IDisposableObject.IsDisposed => this._nativeObject == IntPtr.Zero;

        void IDisposableObject.DeclareOwner(object owner)
        {
        }

        void IDisposableObject.TransferOwnership(object owner)
        {
        }

        void IDisposableObject.Dispose(object owner)
        {
            this.OnDispose();
            new DllProxyObject.AppThreadReleaseEntry(this._nativeObject, this._handle, this.OwningLoadResult).Release();
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {
        }

        private static void ReleaseFinalizedObjects()
        {
            Vector<DllProxyObject.AppThreadReleaseEntry> pendingReleases;
            lock (DllProxyObject.s_finalizeLock)
            {
                pendingReleases = DllProxyObject.s_pendingReleases;
                DllProxyObject.s_pendingReleases = null;
                DllProxyObject.s_pendingAppThreadRelease = false;
            }
            if (pendingReleases == null || pendingReleases.Count == 0)
                return;
            foreach (DllProxyObject.AppThreadReleaseEntry threadReleaseEntry in pendingReleases)
                threadReleaseEntry.Release();
            lock (DllProxyObject.s_finalizeLock)
            {
                if (DllProxyObject.s_pendingAppThreadRelease)
                    return;
                pendingReleases.Clear();
                DllProxyObject.s_pendingReleases = pendingReleases;
            }
        }

        private static string DEBUG_FormatIntPtr(IntPtr ptr) => (string)null;

        public string DEBUG_Description() => string.Empty;

        public override string ToString() => this._type is DllTypeSchema type ? type.InvokeToString(this) : this.GetType().Name;

        internal struct AppThreadReleaseEntry
        {
            private bool _releaseHandle;
            public IntPtr _nativeObject;
            public ulong _handle;
            public DllLoadResult _loadResult;

            public AppThreadReleaseEntry(IntPtr nativeObject, ulong handle, DllLoadResult loadResult)
            {
                this._nativeObject = nativeObject;
                this._handle = handle;
                this._loadResult = loadResult;
                this._releaseHandle = true;
            }

            public AppThreadReleaseEntry(IntPtr nativeObject)
            {
                this._nativeObject = nativeObject;
                this._releaseHandle = false;
                this._handle = 0UL;
                this._loadResult = null;
            }

            public void Release()
            {
                if (this._releaseHandle)
                {
                    DllProxyObject.s_handleTable.ReleaseProxy(this._handle);
                    ulong state;
                    NativeApi.SpGetStateCache(this._nativeObject, out state);
                    if ((long)this._handle == (long)state)
                        NativeApi.SpSetStateCache(this._nativeObject, 0UL);
                }
                NativeApi.SpReleaseExternalObject(this._nativeObject);
                if (!this._releaseHandle)
                    return;
                this._loadResult.UnregisterProxyUsage();
            }
        }
    }
}
