// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllConstructorSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using System;
using System.Diagnostics;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllConstructorSchema : ConstructorSchema
    {
        private TypeSchema[] _parameterTypes;
        private uint _id;

        public DllConstructorSchema(DllTypeSchema owner, uint ID)
          : base((TypeSchema)owner)
          => this._id = ID;

        public bool Load(IntPtr constructor) => this.QueryForParameterTypes(constructor);

        [Conditional("DEBUG")]
        public void DEBUG_Dump()
        {
            string str1 = string.Empty;
            if (this._parameterTypes != null)
            {
                for (int index = 0; index < this._parameterTypes.Length; ++index)
                {
                    string str2 = "<null>";
                    if (this._parameterTypes[index] != null)
                        str2 = this._parameterTypes[index].Name;
                    str1 = string.Format("{0}{1}{2}", (object)str1, index > 0 ? (object)", " : (object)string.Empty, (object)str2);
                }
            }
            string.Format("0x{0:x8} {1}({2})", (object)this._id, (object)this.Owner.Name, (object)str1);
        }

        public override TypeSchema[] ParameterTypes => this._parameterTypes;

        private DllLoadResult OwnerLoadResult => (DllLoadResult)this.Owner.Owner;

        private bool QueryForParameterTypes(IntPtr constructor)
        {
            bool flag1 = false;
            uint count;
            if (this.CheckNativeReturn(NativeApi.SpQueryConstructorParameterCount(constructor, out count)))
            {
                uint[] IDs = new uint[count];
                if (this.CheckNativeReturn(NativeApi.SpGetConstructorParameterTypes(constructor, IDs, count)))
                {
                    if (count > 0U)
                    {
                        this._parameterTypes = new TypeSchema[count];
                        bool flag2 = false;
                        for (int index = 0; (long)index < (long)count; ++index)
                        {
                            TypeSchema typeSchema = DllLoadResult.MapType(IDs[index]);
                            if (typeSchema != null)
                                this._parameterTypes[index] = typeSchema;
                            else
                                flag2 = true;
                        }
                        flag1 = !flag2;
                    }
                    else
                    {
                        this._parameterTypes = TypeSchema.EmptyList;
                        flag1 = true;
                    }
                }
            }
            return flag1;
        }

        public override object Construct(object[] parameters) => ((DllTypeSchema)this.Owner).Construct(this._id, parameters);

        private bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXConstructor");
    }
}
