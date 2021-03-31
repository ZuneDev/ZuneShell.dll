// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllMethodSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using System;
using System.Diagnostics;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllMethodSchema : MethodSchema
    {
        private bool _isStatic;
        private TypeSchema _returnType;
        private TypeSchema[] _parameterTypes;
        private string _name;
        private uint _id;

        public DllMethodSchema(DllTypeSchema owner, uint ID)
          : base((TypeSchema)owner)
          => this._id = ID;

        public bool Load(IntPtr method) => this.QueryMethodName(method) && this.QueryForParameterTypes(method) && this.QueryReturnType(method) && this.QueryIsStatic(method);

        public uint ID => this._id;

        [Conditional("DEBUG")]
        public void DEBUG_Dump()
        {
            string str1 = string.Empty;
            if (this.IsStatic)
                str1 = "static ";
            string str2 = "void";
            if (this.ReturnType != null)
                str2 = this.ReturnType.Name;
            string str3 = string.Empty;
            if (this._parameterTypes != null)
            {
                for (int index = 0; index < this._parameterTypes.Length; ++index)
                {
                    string str4 = "<null>";
                    if (this._parameterTypes[index] != null)
                        str4 = this._parameterTypes[index].Name;
                    str3 = string.Format("{0}{1}{2}", (object)str3, index > 0 ? (object)", " : (object)string.Empty, (object)str4);
                }
            }
            string.Format("0x{0:x8} {1}{2} {3}({4})", (object)this._id, (object)str1, (object)str2, (object)this.Name, (object)str3);
        }

        public override string Name => this._name;

        public override TypeSchema[] ParameterTypes => this._parameterTypes;

        public override TypeSchema ReturnType => this._returnType;

        public override bool IsStatic => this._isStatic;

        private DllTypeSchema OwnerTypeSchema => (DllTypeSchema)this.Owner;

        public override object Invoke(object instance, object[] parameters) => this.OwnerTypeSchema.InvokeMethod(instance, this, parameters);

        private DllLoadResult OwnerLoadResult => (DllLoadResult)this.Owner.Owner;

        private unsafe bool QueryMethodName(IntPtr method)
        {
            bool flag = false;
            char* name;
            if (this.CheckNativeReturn(NativeApi.SpQueryMethodName(method, out name)))
            {
                this._name = new string(name);
                flag = true;
            }
            return flag;
        }

        private bool QueryForParameterTypes(IntPtr method)
        {
            bool flag1 = false;
            uint count;
            if (this.CheckNativeReturn(NativeApi.SpQueryMethodParameterCount(method, out count)))
            {
                if (count > 0U)
                {
                    uint[] IDs = new uint[(int)count];
                    if (this.CheckNativeReturn(NativeApi.SpGetMethodParameterTypes(method, IDs, count)))
                    {
                        this._parameterTypes = new TypeSchema[(int)count];
                        bool flag2 = false;
                        for (uint index = 0; index < count; ++index)
                        {
                            TypeSchema typeSchema = DllLoadResult.MapType(IDs[index]);
                            if (typeSchema != null)
                                this._parameterTypes[index] = typeSchema;
                            else
                                flag2 = true;
                        }
                        flag1 = !flag2;
                    }
                }
                else
                {
                    this._parameterTypes = TypeSchema.EmptyList;
                    flag1 = true;
                }
            }
            return flag1;
        }

        private bool QueryReturnType(IntPtr method)
        {
            bool flag = false;
            uint type;
            if (this.CheckNativeReturn(NativeApi.SpQueryMethodReturnType(method, out type)))
            {
                this._returnType = DllLoadResult.MapType(type);
                flag = this._returnType != null;
            }
            return flag;
        }

        private bool QueryIsStatic(IntPtr method) => this.CheckNativeReturn(NativeApi.SpQueryMethodIsStatic(method, out this._isStatic));

        private bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXMethod");
    }
}
