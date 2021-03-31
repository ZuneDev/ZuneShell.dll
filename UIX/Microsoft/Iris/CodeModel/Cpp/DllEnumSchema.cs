// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllEnumSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using System;
using System.Diagnostics;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllEnumSchema : EnumSchema
    {
        private uint _id;
        private IntPtr _nativeSchema;

        public DllEnumSchema(LoadResult owner, uint id, IntPtr nativeSchema)
          : base(owner)
        {
            this._id = id;
            this._nativeSchema = nativeSchema;
        }

        [Conditional("DEBUG")]
        public void DEBUG_DumpEnum()
        {
            int val1 = 0;
            foreach (KeyValueEntry<string, int> nameToValue in this.NameToValueMap)
            {
                if (nameToValue.Key != null)
                    val1 = Math.Max(val1, nameToValue.Key.Length);
            }
            string.Format("{{0,{0}}} = 0x{{1:x8}}", (object)val1);
            foreach (KeyValueEntry<string, int> nameToValue in this.NameToValueMap)
                ;
        }

        public uint ID => this._id;

        public bool Load()
        {
            string name;
            bool flag = this.QueryName(out name);
            if (flag)
            {
                bool isFlags;
                flag = this.QueryIsFlags(out isFlags);
                if (flag)
                {
                    string[] names;
                    int[] values;
                    flag = this.QueryNamesAndValues(out names, out values);
                    if (flag)
                    {
                        this.Initialize(name, typeof(DllEnumProxy), isFlags, names, values);
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private unsafe bool QueryName(out string name)
        {
            char* name1;
            bool flag = this.CheckNativeReturn(NativeApi.SpQueryEnumName(this._nativeSchema, out name1));
            name = !flag ? (string)null : new string(name1);
            return flag;
        }

        private bool QueryIsFlags(out bool isFlags) => this.CheckNativeReturn(NativeApi.SpQueryEnumIsFlags(this._nativeSchema, out isFlags));

        private unsafe bool QueryNamesAndValues(out string[] names, out int[] values)
        {
            bool flag1 = false;
            names = (string[])null;
            values = (int[])null;
            uint valueCount;
            if (this.CheckNativeReturn(NativeApi.SpQueryEnumValueCount(this._nativeSchema, out valueCount)))
            {
                bool flag2 = false;
                if (valueCount > 0U)
                {
                    names = new string[valueCount];
                    values = new int[valueCount];
                    for (uint index = 0; index < valueCount; ++index)
                    {
                        char* name;
                        int num;
                        if (this.CheckNativeReturn(NativeApi.SpGetEnumNameValue(this._nativeSchema, index, out name, out num)))
                        {
                            if ((IntPtr)name != IntPtr.Zero)
                            {
                                names[index] = new string(name);
                                values[index] = num;
                            }
                            else
                                ErrorManager.ReportError("Script runtime failure: Invalid 'null' value for '{0}'", (object)"Name");
                        }
                        else
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                flag1 = !flag2;
            }
            return flag1;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            NativeApi.SpReleaseExternalObject(this._nativeSchema);
            this._nativeSchema = IntPtr.Zero;
        }

        public object GetBoxedValue(int value) => this.EnumValueToObject(value);

        protected override object EnumValueToObject(int value) => (object)new DllEnumProxy(this, value);

        protected override int ValueFromObject(object obj) => ((DllEnumProxy)obj).Value;

        public string InvokeToString(DllEnumProxy proxy)
        {
            string str = (string)null;
            IntPtr result;
            if (this.CheckNativeReturn(NativeApi.SpInvokeEnumToString(this._nativeSchema, proxy.Value, out result)))
                str = DllProxyServices.GetString(result);
            return str;
        }

        private DllLoadResult OwnerLoadResult => (DllLoadResult)this.Owner;

        private bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXEnum");
    }
}
