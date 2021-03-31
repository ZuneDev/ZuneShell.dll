// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.NativeMarkupDataType
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.CodeModel.Cpp;
using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup
{
    internal class NativeMarkupDataType : MarkupDataType
    {
        private IntPtr _externalObject;
        private ulong _handleToMe;
        private ulong _typeHandle;
        private static object s_finalizeLock;
        private static bool s_pendingAppThreadRelease;
        private static Vector<NativeMarkupDataType.AppThreadReleaseEntry> s_pendingReleases;
        private static SimpleCallback s_releaseOnAppThread;
        private static MarkupDataTypeHandleTable s_handleTable;

        public static void InitializeStatics()
        {
            NativeMarkupDataType.s_handleTable = new MarkupDataTypeHandleTable();
            NativeMarkupDataType.s_finalizeLock = new object();
            NativeMarkupDataType.s_pendingAppThreadRelease = false;
            NativeMarkupDataType.s_releaseOnAppThread = new SimpleCallback(NativeMarkupDataType.ReleaseFinalizedObjects);
        }

        private NativeMarkupDataType(MarkupDataTypeSchema type, IntPtr externalObject)
          : base(type)
        {
            this._externalObject = externalObject;
            this._handleToMe = NativeMarkupDataType.s_handleTable.RegisterProxy((object)this);
            this._typeHandle = type.UniqueId;
            NativeApi.SpAddRefExternalObject(this._externalObject);
            int num = (int)NativeApi.SpDataBaseObjectSetInternalHandle(this._externalObject, this._handleToMe);
            this.TypeSchema.Owner.RegisterProxyUsage();
        }

        protected override void OnDispose()
        {
            NativeMarkupDataType.ReleaseNativeObject(this._externalObject, this._handleToMe, this._typeHandle);
            GC.SuppressFinalize((object)this);
            base.OnDispose();
        }

        ~NativeMarkupDataType()
        {
            lock (NativeMarkupDataType.s_finalizeLock)
            {
                if (NativeMarkupDataType.s_pendingReleases == null)
                    NativeMarkupDataType.s_pendingReleases = new Vector<NativeMarkupDataType.AppThreadReleaseEntry>();
                NativeMarkupDataType.s_pendingReleases.Add(new NativeMarkupDataType.AppThreadReleaseEntry(this._externalObject, this._handleToMe, this._typeHandle));
                if (NativeMarkupDataType.s_pendingAppThreadRelease)
                    return;
                NativeMarkupDataType.s_pendingAppThreadRelease = true;
                DeferredCall.Post(DispatchPriority.Idle, NativeMarkupDataType.s_releaseOnAppThread);
            }
        }

        protected override bool ExternalObjectGetProperty(string propertyName, out object value)
        {
            UIXVariant propertyValue;
            int property = (int)NativeApi.SpDataBaseObjectGetProperty(this._externalObject, propertyName, out propertyValue);
            this.TypeSchema.FindPropertyDeep(propertyName);
            value = UIXVariant.GetValue(propertyValue, this.TypeSchema.Owner);
            return true;
        }

        protected override unsafe bool ExternalObjectSetProperty(string propertyName, object value)
        {
            // ISSUE: untyped stack allocation
            UIXVariant* uixVariantPtr = stackalloc UIXVariant[sizeof(UIXVariant)];
            UIXVariant.MarshalObject(value, uixVariantPtr);
            int num = (int)NativeApi.SpDataBaseObjectSetProperty(this._externalObject, propertyName, uixVariantPtr);
            return true;
        }

        protected override IDataProviderBaseObject ExternalAssemblyObject => (IDataProviderBaseObject)null;

        public override IntPtr ExternalNativeObject => this._externalObject;

        public static NativeMarkupDataType LookupByHandle(ulong handle)
        {
            MarkupDataType markupDataType;
            NativeMarkupDataType.s_handleTable.LookupByHandle(handle, out markupDataType);
            return (NativeMarkupDataType)markupDataType;
        }

        public static NativeMarkupDataType Create(
          ulong typeHandle,
          IntPtr nativeObject)
        {
            return new NativeMarkupDataType((MarkupDataTypeSchema)TypeSchema.LookupById(typeHandle), nativeObject);
        }

        public static void ReleaseOutstandingProxies()
        {
            NativeMarkupDataType.s_pendingAppThreadRelease = true;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            foreach (IDisposableObject disposableObject in (DllProxyHandleTable)NativeMarkupDataType.s_handleTable)
                disposableObject.Dispose((object)disposableObject);
            NativeMarkupDataType.ReleaseFinalizedObjects();
        }

        private static void ReleaseFinalizedObjects()
        {
            Vector<NativeMarkupDataType.AppThreadReleaseEntry> pendingReleases;
            lock (NativeMarkupDataType.s_finalizeLock)
            {
                pendingReleases = NativeMarkupDataType.s_pendingReleases;
                NativeMarkupDataType.s_pendingReleases = (Vector<NativeMarkupDataType.AppThreadReleaseEntry>)null;
                NativeMarkupDataType.s_pendingAppThreadRelease = false;
            }
            if (pendingReleases == null || pendingReleases.Count == 0)
                return;
            foreach (NativeMarkupDataType.AppThreadReleaseEntry threadReleaseEntry in pendingReleases)
                NativeMarkupDataType.ReleaseNativeObject(threadReleaseEntry._nativeObject, threadReleaseEntry._handle, threadReleaseEntry._typeHandle);
            lock (NativeMarkupDataType.s_finalizeLock)
            {
                if (NativeMarkupDataType.s_pendingAppThreadRelease)
                    return;
                pendingReleases.Clear();
                NativeMarkupDataType.s_pendingReleases = pendingReleases;
            }
        }

        private static void ReleaseNativeObject(
          IntPtr nativeObject,
          ulong proxyHandle,
          ulong typeHandle)
        {
            NativeMarkupDataType.s_handleTable.ReleaseProxy(proxyHandle);
            ulong frameworkQuery;
            int internalHandle = (int)NativeApi.SpDataBaseObjectGetInternalHandle(nativeObject, out frameworkQuery);
            if ((long)proxyHandle == (long)frameworkQuery)
            {
                int num = (int)NativeApi.SpDataBaseObjectSetInternalHandle(nativeObject, 0UL);
            }
            NativeApi.SpReleaseExternalObject(nativeObject);
            TypeSchema.LookupById(typeHandle).Owner.UnregisterProxyUsage();
        }

        internal struct AppThreadReleaseEntry
        {
            public IntPtr _nativeObject;
            public ulong _handle;
            public ulong _typeHandle;

            public AppThreadReleaseEntry(IntPtr nativeObject, ulong handle, ulong typeHandle)
            {
                this._nativeObject = nativeObject;
                this._handle = handle;
                this._typeHandle = typeHandle;
            }
        }
    }
}
