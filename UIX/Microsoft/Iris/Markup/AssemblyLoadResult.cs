// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.AssemblyLoadResult
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Drawing;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Iris.Markup
{
    internal class AssemblyLoadResult : LoadResult
    {
        private const string c_AssemblyProtocol = "assembly://";
        private Assembly _assembly;
        private string _namespace;
        private string _namespacePrefix;
        public static TypeSchema ObjectTypeSchema;
        public static TypeSchema ListTypeSchema;
        public static TypeSchema EnumeratorTypeSchema;
        public static TypeSchema DictionaryTypeSchema;
        public static TypeSchema CommandTypeSchema;
        public static TypeSchema ValueRangeTypeSchema;
        private static Map s_typeCache = new Map(32);
        private static Map<AssemblyLoadResult.MapAssemblyKey, AssemblyLoadResult> s_assemblyCache = new Map<AssemblyLoadResult.MapAssemblyKey, AssemblyLoadResult>();

        private static LoadResult Create(string uri)
        {
            LoadResult loadResult = null;
            string valueName = uri.Substring("assembly://".Length);
            if (valueName.IndexOf('/') == -1)
            {
                ErrorManager.ReportError("Invalid assembly reference '{0}'.  URI must contain a forward slash after the assembly name", uri);
                return null;
            }
            string leftName;
            string rightName;
            AssemblyLoadResult.SplitAtLastWhack(valueName, out leftName, out rightName);
            AssemblyName name = null;
            try
            {
                name = new AssemblyName(leftName);
            }
            catch (COMException ex)
            {
            }
            catch (FileLoadException ex)
            {
            }
            if (name == null)
            {
                name = new AssemblyName();
                name.CodeBase = leftName;
            }
            if (name != null)
            {
                Exception assemblyLoadException;
                Assembly assembly = AssemblyLoadResult.FindAssembly(name, out assemblyLoadException);
                if (assembly != null)
                    loadResult = AssemblyLoadResult.MapAssembly(assembly, rightName);
                else if (assemblyLoadException != null)
                    ErrorManager.ReportError("Failure loading assembly: '{0}'", assemblyLoadException.Message);
                else
                    ErrorManager.ReportError("Failure loading assembly");
            }
            return loadResult;
        }

        public static void Startup()
        {
            AssemblyObjectProxyHelper.InitializeStatics();
            MarkupSystem.RegisterFactoryByProtocol("assembly://", new CreateLoadResultHandler(AssemblyLoadResult.Create));
            Map typeCache1 = AssemblyLoadResult.s_typeCache;
            Type type1 = typeof(object);
            FrameworkCompatibleAssemblyPrimitiveTypeSchema primitiveTypeSchema;
            AssemblyLoadResult.ObjectTypeSchema = primitiveTypeSchema = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(ObjectSchema.Type);
            TypeSchema typeA1 = primitiveTypeSchema;
            typeCache1[type1] = primitiveTypeSchema;
            TypeSchema.RegisterTwoWayEquivalence(typeA1, ObjectSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(void)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(VoidSchema.Type)), VoidSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(bool)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(BooleanSchema.Type)), BooleanSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(byte)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(ByteSchema.Type)), ByteSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(char)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(CharSchema.Type)), CharSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(double)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(DoubleSchema.Type)), DoubleSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(string)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(StringSchema.Type)), StringSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(float)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(SingleSchema.Type)), SingleSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(int)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(Int32Schema.Type)), Int32Schema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(long)] = new FrameworkCompatibleAssemblyPrimitiveTypeSchema(Int64Schema.Type)), Int64Schema.Type);
            Map typeCache2 = AssemblyLoadResult.s_typeCache;
            Type type2 = typeof(IList);
            FrameworkCompatibleAssemblyTypeSchema assemblyTypeSchema1;
            AssemblyLoadResult.ListTypeSchema = assemblyTypeSchema1 = new FrameworkCompatibleAssemblyTypeSchema(typeof(IList), typeof(IList), typeof(ArrayList));
            TypeSchema typeA2 = assemblyTypeSchema1;
            typeCache2[type2] = assemblyTypeSchema1;
            TypeSchema.RegisterTwoWayEquivalence(typeA2, ListSchema.Type);
            Map typeCache3 = AssemblyLoadResult.s_typeCache;
            Type type3 = typeof(IEnumerator);
            FrameworkCompatibleAssemblyTypeSchema assemblyTypeSchema2;
            AssemblyLoadResult.EnumeratorTypeSchema = assemblyTypeSchema2 = new FrameworkCompatibleAssemblyTypeSchema(typeof(IEnumerator), typeof(IEnumerator));
            TypeSchema typeA3 = assemblyTypeSchema2;
            typeCache3[type3] = assemblyTypeSchema2;
            TypeSchema.RegisterTwoWayEquivalence(typeA3, EnumeratorSchema.Type);
            Map typeCache4 = AssemblyLoadResult.s_typeCache;
            Type type4 = typeof(IDictionary);
            FrameworkCompatibleAssemblyTypeSchema assemblyTypeSchema3;
            AssemblyLoadResult.DictionaryTypeSchema = assemblyTypeSchema3 = new FrameworkCompatibleAssemblyTypeSchema(typeof(IDictionary), AssemblyObjectProxyHelper.ProxyDictionaryType, typeof(Dictionary<object, object>));
            TypeSchema producer1 = assemblyTypeSchema3;
            typeCache4[type4] = assemblyTypeSchema3;
            TypeSchema.RegisterOneWayEquivalence(producer1, DictionarySchema.Type);
            Map typeCache5 = AssemblyLoadResult.s_typeCache;
            Type type5 = typeof(ICommand);
            FrameworkCompatibleAssemblyTypeSchema assemblyTypeSchema4;
            AssemblyLoadResult.CommandTypeSchema = assemblyTypeSchema4 = new FrameworkCompatibleAssemblyTypeSchema(typeof(ICommand), AssemblyObjectProxyHelper.ProxyCommandType);
            TypeSchema producer2 = assemblyTypeSchema4;
            typeCache5[type5] = assemblyTypeSchema4;
            TypeSchema.RegisterOneWayEquivalence(producer2, CommandSchema.Type);
            Map typeCache6 = AssemblyLoadResult.s_typeCache;
            Type type6 = typeof(IValueRange);
            FrameworkCompatibleAssemblyTypeSchema assemblyTypeSchema5;
            AssemblyLoadResult.ValueRangeTypeSchema = assemblyTypeSchema5 = new FrameworkCompatibleAssemblyTypeSchema(typeof(IValueRange), AssemblyObjectProxyHelper.ProxyValueRangeType);
            TypeSchema producer3 = assemblyTypeSchema5;
            typeCache6[type6] = assemblyTypeSchema5;
            TypeSchema.RegisterOneWayEquivalence(producer3, ValueRangeSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Group)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Group), typeof(IUIGroup))), GroupSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Image)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Image), typeof(UIImage))), ImageSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Type)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Type), typeof(TypeSchema), null, AssemblyLoadResult.ObjectTypeSchema)), TypeSchemaDefinition.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(VideoStream)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(VideoStream))), VideoStreamSchema.Type);
            TypeSchema producer4;
            AssemblyLoadResult.s_typeCache[typeof(Microsoft.Iris.Choice)] = (FrameworkCompatibleAssemblyTypeSchema)(producer4 = new FrameworkCompatibleAssemblyTypeSchema(typeof(Microsoft.Iris.Choice)));
            TypeSchema.RegisterOneWayEquivalence(producer4, ChoiceSchema.Type);
            TypeSchema.RegisterOneWayEquivalence(producer4, ValueRangeSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Microsoft.Iris.BooleanChoice)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Microsoft.Iris.BooleanChoice))), BooleanChoiceSchema.Type);
            TypeSchema producer5;
            AssemblyLoadResult.s_typeCache[typeof(Microsoft.Iris.RangedValue)] = (FrameworkCompatibleAssemblyTypeSchema)(producer5 = new FrameworkCompatibleAssemblyTypeSchema(typeof(Microsoft.Iris.RangedValue)));
            TypeSchema.RegisterOneWayEquivalence(producer5, RangedValueSchema.Type);
            TypeSchema.RegisterOneWayEquivalence(producer5, ValueRangeSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Microsoft.Iris.IntRangedValue)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Microsoft.Iris.IntRangedValue))), IntRangedValueSchema.Type);
            TypeSchema.RegisterOneWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(Microsoft.Iris.ByteRangedValue)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(Microsoft.Iris.ByteRangedValue))), ByteRangedValueSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(DataProviderQuery)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(DataProviderQuery), typeof(MarkupDataQuery))), MarkupDataQueryInstanceSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(DataProviderObject)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(DataProviderObject), typeof(MarkupDataType))), MarkupDataTypeInstanceSchema.Type);
            TypeSchema.RegisterTwoWayEquivalence((TypeSchema)(AssemblyLoadResult.s_typeCache[typeof(DataProviderQueryStatus)] = new FrameworkCompatibleAssemblyTypeSchema(typeof(DataProviderQueryStatus))), UIXLoadResultExports.DataQueryStatusType);
        }

        public static void Shutdown()
        {
            foreach (SharedDisposableObject disposableObject in AssemblyLoadResult.s_assemblyCache.Values)
                disposableObject.UnregisterUsage(s_assemblyCache);
            AssemblyLoadResult.s_assemblyCache.Clear();
            AssemblyLoadResult.s_assemblyCache = null;
            foreach (AssemblyTypeSchema assemblyTypeSchema in AssemblyLoadResult.s_typeCache.Values)
            {
                AssemblyLoadResult owner = (AssemblyLoadResult)assemblyTypeSchema.Owner;
                assemblyTypeSchema.Dispose(owner);
            }
            AssemblyLoadResult.s_typeCache.Clear();
            AssemblyLoadResult.s_typeCache = null;
        }

        public string Namespace => this._namespace;

        public override LoadResultStatus Status => LoadResultStatus.Success;

        public override TypeSchema FindType(string name)
        {
            Type type = this._assembly.GetType(this._namespacePrefix + name, false);
            if (type == null)
                return null;
            if (type.IsVisible)
                return AssemblyLoadResult.MapType(type);
            ErrorManager.ReportError("Type '{0}' is not public in '{1}'", name, _assembly);
            return null;
        }

        public static AssemblyLoadResult MapAssembly(Assembly assembly, string ns)
        {
            AssemblyLoadResult.MapAssemblyKey key = new AssemblyLoadResult.MapAssemblyKey(assembly, ns);
            AssemblyLoadResult assemblyLoadResult;
            if (!AssemblyLoadResult.s_assemblyCache.TryGetValue(key, out assemblyLoadResult))
            {
                string uri = "assembly://" + assembly.FullName;
                if (ns != null)
                    uri = uri + "/" + ns;
                assemblyLoadResult = new AssemblyLoadResult(assembly, ns, uri);
                AssemblyLoadResult.s_assemblyCache[key] = assemblyLoadResult;
                assemblyLoadResult.RegisterUsage(s_assemblyCache);
            }
            return assemblyLoadResult;
        }

        public static AssemblyTypeSchema MapType(Type type)
        {
            object obj;
            AssemblyTypeSchema assemblyTypeSchema;
            if (AssemblyLoadResult.s_typeCache.TryGetValue(type, out obj))
            {
                assemblyTypeSchema = (AssemblyTypeSchema)obj;
            }
            else
            {
                assemblyTypeSchema = AssemblyObjectProxyHelper.CreateProxySchema(type);
                AssemblyLoadResult.s_typeCache[type] = assemblyTypeSchema;
            }
            return assemblyTypeSchema;
        }

        public static Type MapType(TypeSchema typeSchema)
        {
            if (typeSchema is AssemblyTypeSchema assemblyTypeSchema)
                return assemblyTypeSchema.InternalType;
            for (TypeSchema typeSchema1 = typeSchema; typeSchema1 != null; typeSchema1 = typeSchema1.Base)
            {
                if (typeSchema1.Equivalents != null)
                {
                    foreach (TypeSchema equivalent in typeSchema1.Equivalents)
                    {
                        if (equivalent is AssemblyTypeSchema assemblyTypeSchemaB)
                            return assemblyTypeSchemaB.InternalType;
                    }
                }
            }
            return null;
        }

        internal static Type[] MapTypeList(TypeSchema[] typeSchemaList)
        {
            Type[] typeArray = new Type[typeSchemaList.Length];
            for (int index = 0; index < typeSchemaList.Length; ++index)
            {
                typeArray[index] = AssemblyLoadResult.MapType(typeSchemaList[index]);
                if (typeArray[index] == null)
                    return null;
            }
            return typeArray;
        }

        internal static TypeSchema[] MapTypeList(Type[] typeList)
        {
            TypeSchema[] typeSchemaArray = new TypeSchema[typeList.Length];
            for (int index = 0; index < typeList.Length; ++index)
            {
                typeSchemaArray[index] = AssemblyLoadResult.MapType(typeList[index]);
                if (typeSchemaArray[index] == null)
                    return null;
            }
            return typeSchemaArray;
        }

        internal static object WrapObject(TypeSchema typeSchema, object instance) => AssemblyObjectProxyHelper.WrapObject(typeSchema, instance);

        internal static object WrapObject(object instance) => AssemblyObjectProxyHelper.WrapObject(null, instance);

        internal static object UnwrapObject(object instance) => AssemblyObjectProxyHelper.UnwrapObject(instance);

        internal static object[] UnwrapObjectList(object[] instanceList)
        {
            if (instanceList == null)
                return null;
            object[] objArray = new object[instanceList.Length];
            for (int index = 0; index < objArray.Length; ++index)
                objArray[index] = AssemblyLoadResult.UnwrapObject(instanceList[index]);
            return objArray;
        }

        private static void SplitAtLastWhack(
          string valueName,
          out string leftName,
          out string rightName)
        {
            int length = valueName.LastIndexOf('/');
            if (length != -1)
            {
                leftName = valueName.Substring(0, length);
                rightName = valueName.Substring(length + 1);
            }
            else
            {
                leftName = valueName;
                rightName = null;
            }
        }

        public static Assembly FindAssembly(
          AssemblyName name,
          out Exception assemblyLoadException)
        {
            Assembly assembly = null;
            assemblyLoadException = null;
            try
            {
                assembly = Assembly.Load(name);
            }
            catch (FileLoadException ex)
            {
                assemblyLoadException = ex;
            }
            catch (BadImageFormatException ex)
            {
                assemblyLoadException = ex;
            }
            catch (FileNotFoundException ex)
            {
                assemblyLoadException = ex;
            }
            return assembly;
        }

        public override string GetCompilerReferenceName() => base.GetCompilerReferenceName() ?? string.Format("{0}{1}/{2}", "assembly://", _assembly.GetName().Name, _namespace);

        internal Assembly Assembly => this._assembly;

        private AssemblyLoadResult(Assembly assembly, string ns, string uri)
          : base(uri)
        {
            this._assembly = assembly;
            this._namespace = ns;
            if (ns == null)
                return;
            this._namespacePrefix = ns + ".";
        }

        internal struct MapAssemblyKey
        {
            private Assembly _assembly;
            private string _namespace;

            public MapAssemblyKey(Assembly assembly, string ns)
            {
                this._assembly = assembly;
                this._namespace = ns;
            }

            public override bool Equals(object obj)
            {
                AssemblyLoadResult.MapAssemblyKey mapAssemblyKey = (AssemblyLoadResult.MapAssemblyKey)obj;
                return this._assembly == mapAssemblyKey._assembly && this._namespace == mapAssemblyKey._namespace;
            }

            public override int GetHashCode()
            {
                int hashCode = this._assembly.GetHashCode();
                if (this._namespace != null)
                    hashCode ^= this._namespace.GetHashCode();
                return hashCode;
            }
        }
    }
}
