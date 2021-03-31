// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllLoadResult
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllLoadResult : LoadResult
    {
        private DllLoadResultFactory _source;
        private LoadResultStatus _status;
        private LoadPass _loadPass;
        private IntPtr _schema;
        private Map<uint, TypeSchema> _userDefinedTypes;
        private Map<uint, TypeSchema> _intrinsicTypes;
        private static Map<uint, DllLoadResult.IntrinsicTypeData> s_intrinsicData;
        private ushort _component;
        private static ushort s_nextID = 1;
        private static LoadResult s_objectContext;

        public static void Startup()
        {
            DllLoadResultFactory.Startup();
            DllProxyServices.Startup();
            LoadIntrinsicTypeData();
        }

        private static void LoadIntrinsicTypeData()
        {
            s_intrinsicData = new Map<uint, DllLoadResult.IntrinsicTypeData>();
            s_intrinsicData[4294967294U] = new DllLoadResult.IntrinsicTypeData(BooleanSchema.Type);
            s_intrinsicData[4294967293U] = new DllLoadResult.IntrinsicTypeData(ByteSchema.Type);
            s_intrinsicData[4294967292U] = new DllLoadResult.IntrinsicTypeData(DoubleSchema.Type);
            s_intrinsicData[4294967285U] = new DllLoadResult.IntrinsicTypeData(ListSchema.Type, typeof(DllProxyList));
            s_intrinsicData[4294967284U] = new DllLoadResult.IntrinsicTypeData(ImageSchema.Type);
            s_intrinsicData[4294967283U] = new DllLoadResult.IntrinsicTypeData(Int32Schema.Type);
            s_intrinsicData[4294967282U] = new DllLoadResult.IntrinsicTypeData(Int64Schema.Type);
            s_intrinsicData[4294967280U] = new DllLoadResult.IntrinsicTypeData(ObjectSchema.Type);
            s_intrinsicData[4294967279U] = new DllLoadResult.IntrinsicTypeData(SingleSchema.Type);
            s_intrinsicData[4294967278U] = new DllLoadResult.IntrinsicTypeData(StringSchema.Type);
            s_intrinsicData[4294967277U] = new DllLoadResult.IntrinsicTypeData(VoidSchema.Type);
        }

        public static void Shutdown() => DllProxyServices.Shutdown();

        public static bool CheckNativeReturn(uint hr, string interfaceName)
        {
            if ((int)hr >= 0)
                return true;
            ErrorManager.ReportError("Schema API failure: A method on '{0}' failed with code '0x{1:X8}'", interfaceName, hr);
            return false;
        }

        private bool CheckNativeReturn(uint hr) => CheckNativeReturn(hr, "IUIXTypeSchema");

        public static void PushContext(LoadResult newContext) => s_objectContext = newContext;

        public static LoadResult CurrentContext => s_objectContext;

        public static void PopContext() => s_objectContext = null;

        public static TypeSchema MapType(uint typeID)
        {
            uint schemaComponent = UIXID.GetSchemaComponent(typeID);
            TypeSchema typeSchema = null;
            DllLoadResult dllLoadResult = null;
            if (schemaComponent != ushort.MaxValue)
            {
                dllLoadResult = DllLoadResultFactory.GetLoadResultByID(schemaComponent);
                if (dllLoadResult != null)
                    typeSchema = dllLoadResult.MapLocalType(typeID);
            }
            else if (CurrentContext is DllLoadResult currentContext)
                typeSchema = currentContext.MapIntrinsicType(typeID);
            if (typeSchema == null)
                ErrorManager.ReportError("Unable to find type with ID '0x{0:X8}' in '{1}'", typeID, dllLoadResult != null ? dllLoadResult.Uri : string.Empty);
            return typeSchema;
        }

        private TypeSchema MapIntrinsicType(uint typeID)
        {
            if (this._intrinsicTypes == null)
                this._intrinsicTypes = new Map<uint, TypeSchema>();
            TypeSchema typeSchema;
            DllLoadResult.IntrinsicTypeData intrinsicTypeData;
            if (!this._intrinsicTypes.TryGetValue(typeID, out typeSchema) && s_intrinsicData.TryGetValue(typeID, out intrinsicTypeData))
            {
                typeSchema = !intrinsicTypeData.DemandCreateTypeSchema ? intrinsicTypeData.FrameworkEquivalent : new DllIntrinsicTypeSchema(this, typeID, intrinsicTypeData.FrameworkEquivalent);
                this._intrinsicTypes[typeID] = typeSchema;
            }
            return typeSchema;
        }

        private TypeSchema MapLocalType(uint typeID)
        {
            TypeSchema typeSchema = null;
            this._userDefinedTypes.TryGetValue(typeID, out typeSchema);
            return typeSchema;
        }

        public DllLoadResult(DllLoadResultFactory sourceFactory, IntPtr nativeSchema, string uri)
          : base(uri)
        {
            this._source = sourceFactory;
            this._status = LoadResultStatus.Loading;
            this._schema = nativeSchema;
            this._loadPass = LoadPass.Invalid;
            this._component = s_nextID++;
        }

        public override void Load(LoadPass pass)
        {
            if (pass <= this._loadPass || pass != LoadPass.DeclareTypes)
                return;
            this._status = !this.SetSchemaID() || !this.LoadTypes() ? LoadResultStatus.Error : LoadResultStatus.Success;
            this._loadPass = LoadPass.Done;
        }

        public ushort SchemaComponent => this._component;

        private bool SetSchemaID() => this.CheckNativeReturn(NativeApi.SpSetSchemaID(this._schema, this.SchemaComponent));

        private bool LoadTypes()
        {
            bool flag = false;
            UIXIDVerifier idVerifier = new UIXIDVerifier(this);
            uint typeCount;
            uint enumCount;
            if (this.CheckNativeReturn(NativeApi.SpQueryTypeCount(this._schema, out typeCount)) && this.CheckNativeReturn(NativeApi.SpQueryEnumCount(this._schema, out enumCount)))
            {
                int capacity = (int)typeCount + (int)enumCount;
                if (capacity > 0)
                {
                    this._userDefinedTypes = new Map<uint, TypeSchema>(capacity);
                    if (!this.LoadClasses(typeCount, idVerifier) || !this.LoadEnums(enumCount, idVerifier) || !this.ResolveTypes(idVerifier))
                        goto label_4;
                }
                flag = true;
            }
        label_4:
            return flag;
        }

        private bool LoadClasses(uint numClasses, UIXIDVerifier idVerifier)
        {
            bool flag = false;
            if (numClasses > 0U)
            {
                for (uint index = 0; index < numClasses; ++index)
                {
                    if (!this.GetTypeSchema(index, idVerifier))
                        flag = true;
                }
            }
            return !flag;
        }

        private bool GetTypeSchema(uint index, UIXIDVerifier idVerifier)
        {
            bool flag = false;
            IntPtr type;
            uint ID;
            if (this.CheckNativeReturn(NativeApi.SpGetTypeSchema(this._schema, index, out type, out ID)))
            {
                if (type != IntPtr.Zero)
                {
                    this.StoreType(new DllTypeSchema(this, ID, type), ID, true, idVerifier);
                    flag = idVerifier.RegisterID(ID);
                }
                else
                    ErrorManager.ReportError("NULL object returned from {0}", "IUIXSchema::GetType");
            }
            return flag;
        }

        private void StoreType(TypeSchema schema, uint ID, bool isClass, UIXIDVerifier idVerifier) => this._userDefinedTypes[ID] = schema;

        private static void DEBUG_DumpType(object param)
        {
        }

        private bool LoadEnums(uint numEnums, UIXIDVerifier idVerifier)
        {
            bool flag = false;
            if (numEnums > 0U)
            {
                for (uint index = 0; index < numEnums; ++index)
                {
                    if (!this.GetEnumSchema(index, idVerifier))
                        flag = true;
                }
            }
            return !flag;
        }

        private bool GetEnumSchema(uint index, UIXIDVerifier idVerifier)
        {
            bool flag = false;
            IntPtr enumType;
            uint ID;
            if (this.CheckNativeReturn(NativeApi.SpGetEnumSchema(this._schema, index, out enumType, out ID)))
            {
                if (enumType != IntPtr.Zero)
                {
                    this.StoreType(new DllEnumSchema(this, ID, enumType), ID, false, idVerifier);
                    flag = idVerifier.RegisterID(ID);
                }
                else
                    ErrorManager.ReportError("NULL object returned from {0}", "IUIXSchema::GetEnum");
            }
            return flag;
        }

        private bool ResolveTypes(UIXIDVerifier idVerifier)
        {
            bool flag = false;
            if (this._userDefinedTypes != null && this._userDefinedTypes.Count > 0)
            {
                foreach (TypeSchema typeSchema in this._userDefinedTypes.Values)
                {
                    switch (typeSchema)
                    {
                        case DllTypeSchema dllTypeSchema:
                            if (!dllTypeSchema.Load(idVerifier))
                            {
                                flag = true;
                                continue;
                            }
                            continue;
                        case DllEnumSchema dllEnumSchema:
                            if (!dllEnumSchema.Load())
                            {
                                flag = true;
                                continue;
                            }
                            continue;
                        default:
                            flag = true;
                            continue;
                    }
                }
            }
            return !flag;
        }

        private static void DEBUG_DumpEnum(object param)
        {
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (this._userDefinedTypes != null && this._userDefinedTypes.Count > 0)
            {
                foreach (DisposableObject disposableObject in this._userDefinedTypes.Values)
                    disposableObject.Dispose(this);
            }
            if (this._intrinsicTypes != null && this._intrinsicTypes.Count > 0)
            {
                foreach (TypeSchema typeSchema in this._intrinsicTypes.Values)
                {
                    if (typeSchema is DllIntrinsicTypeSchema intrinsicTypeSchema)
                        intrinsicTypeSchema.Dispose(this);
                }
            }
            NativeApi.SpReleaseExternalObject(this._schema);
            this._schema = IntPtr.Zero;
            this._source.NotifyLoadResultDisposed(this);
        }

        public override TypeSchema FindType(string name)
        {
            if (this._userDefinedTypes != null && this._userDefinedTypes.Count > 0)
            {
                foreach (TypeSchema typeSchema in this._userDefinedTypes.Values)
                {
                    if (typeSchema.Name == name)
                        return typeSchema;
                }
            }
            return null;
        }

        public override LoadResultStatus Status => this._status;

        public override LoadResult[] Dependencies => EmptyList;

        public override bool Cachable => true;

        public static Type RuntimeTypeForMarshalAs(uint marshalAs)
        {
            Type type = null;
            if (marshalAs == uint.MaxValue)
            {
                type = typeof(DllProxyObject);
            }
            else
            {
                DllLoadResult.IntrinsicTypeData intrinsicTypeData;
                if (s_intrinsicData.TryGetValue(marshalAs, out intrinsicTypeData))
                    type = intrinsicTypeData.MarshalAsRuntimeType;
            }
            return type;
        }

        private class IntrinsicTypeData
        {
            private TypeSchema _frameworkType;
            private Type _runtimeType;

            public IntrinsicTypeData(TypeSchema frameworkEquivalent)
              : this(frameworkEquivalent, null)
            {
            }

            public IntrinsicTypeData(TypeSchema frameworkEquivalent, Type runtimeMarshalAsType)
            {
                this._frameworkType = frameworkEquivalent;
                this._runtimeType = runtimeMarshalAsType;
            }

            public TypeSchema FrameworkEquivalent => this._frameworkType;

            public Type MarshalAsRuntimeType => this._runtimeType;

            public bool DemandCreateTypeSchema => this.MarshalAsRuntimeType != null;
        }
    }
}
