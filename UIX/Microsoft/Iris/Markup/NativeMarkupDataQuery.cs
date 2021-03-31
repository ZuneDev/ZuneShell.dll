// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.NativeMarkupDataQuery
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.CodeModel.Cpp;
using Microsoft.Iris.OS;
using System;

namespace Microsoft.Iris.Markup
{
    internal class NativeMarkupDataQuery : MarkupDataQuery
    {
        private IntPtr _externalQuery;
        private ulong _handleToMe;
        private ulong _typeHandle;
        private ulong _resultTypeHandle;
        private static MarkupDataQueryHandleTable s_handleTable;

        public static void InitializeStatics() => NativeMarkupDataQuery.s_handleTable = new MarkupDataQueryHandleTable();

        public NativeMarkupDataQuery(MarkupDataQuerySchema type, NativeDataProviderWrapper provider)
          : base(type)
        {
            this._handleToMe = NativeMarkupDataQuery.s_handleTable.RegisterProxy(this);
            this._typeHandle = type.UniqueId;
            this._resultTypeHandle = type.ResultType.UniqueId;
            this._externalQuery = provider.ConstructQuery(type.ProviderName, this._typeHandle, this._resultTypeHandle, this._handleToMe);
            this.ApplyDefaultValues();
        }

        public override void NotifyInitialized()
        {
            base.NotifyInitialized();
            int num = (int)NativeApi.SpDataQueryNotifyInitialized(this._externalQuery);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            NativeMarkupDataQuery.s_handleTable.ReleaseProxy(this._handleToMe);
            int num = (int)NativeApi.SpDataBaseObjectSetInternalHandle(this._externalQuery, 0UL);
            NativeApi.SpReleaseExternalObject(this._externalQuery);
        }

        public override void Refresh()
        {
            int num = (int)NativeApi.SpDataQueryRefresh(this._externalQuery);
        }

        public override unsafe object Result
        {
            get
            {
                UIXVariant propertyValue;
                int resultProperty = (int)NativeApi.SpDataQueryGetResultProperty(this._externalQuery, out propertyValue);
                return UIXVariant.GetValue(propertyValue, this.TypeSchema.Owner);
            }
            set
            {
                UIXVariant* uixVariantPtr = stackalloc UIXVariant[sizeof(UIXVariant)];
                UIXVariant.MarshalObject(value, uixVariantPtr);
                int num = (int)NativeApi.SpDataQuerySetResultProperty(this._externalQuery, uixVariantPtr);
            }
        }

        public override DataProviderQueryStatus Status
        {
            get
            {
                DataProviderQueryStatus propertyValue;
                int statusProperty = (int)NativeApi.SpDataQueryGetStatusProperty(this._externalQuery, out propertyValue);
                return propertyValue;
            }
        }

        public override bool Enabled
        {
            get
            {
                bool propertyValue;
                int enabledProperty = (int)NativeApi.SpDataQueryGetEnabledProperty(this._externalQuery, out propertyValue);
                return propertyValue;
            }
            set
            {
                int num = (int)NativeApi.SpDataQuerySetEnabledProperty(this._externalQuery, value);
            }
        }

        protected override bool ExternalObjectGetProperty(string propertyName, out object value)
        {
            UIXVariant propertyValue;
            int property = (int)NativeApi.SpDataBaseObjectGetProperty(this._externalQuery, propertyName, out propertyValue);
            this.TypeSchema.FindPropertyDeep(propertyName);
            value = UIXVariant.GetValue(propertyValue, this.TypeSchema.Owner);
            return true;
        }

        protected override unsafe bool ExternalObjectSetProperty(string propertyName, object value)
        {
            // ISSUE: untyped stack allocation
            UIXVariant* uixVariantPtr = stackalloc UIXVariant[sizeof(UIXVariant)];
            UIXVariant.MarshalObject(value, uixVariantPtr);
            int num = (int)NativeApi.SpDataBaseObjectSetProperty(this._externalQuery, propertyName, uixVariantPtr);
            return true;
        }

        protected override IDataProviderBaseObject ExternalAssemblyObject => (IDataProviderBaseObject)null;

        public override IntPtr ExternalNativeObject => this._externalQuery;

        public ulong Handle => this._handleToMe;

        public static MarkupDataQuery LookupByHandle(ulong handle)
        {
            MarkupDataQuery markupDataQuery;
            NativeMarkupDataQuery.s_handleTable.LookupByHandle(handle, out markupDataQuery);
            return markupDataQuery;
        }
    }
}
