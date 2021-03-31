// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.CodeModel.Cpp.DllPropertySchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Microsoft.Iris.CodeModel.Cpp
{
    internal class DllPropertySchema : PropertySchema
    {
        private BitVector32 _bits;
        private string _name;
        private TypeSchema _type;
        private uint _id;

        public DllPropertySchema(DllTypeSchema owner, uint ID)
          : base((TypeSchema)owner)
          => this._id = ID;

        public bool Load(IntPtr property) => this.QueryPropertyName(property) && this.QueryPropertyType(property) && (this.QueryCanRead(property) && this.QueryCanWrite(property)) && this.QueryIsStatic(property) && this.QueryNotifiesOnChange(property);

        public uint ID => this._id;

        [Conditional("DEBUG")]
        public void DEBUG_Dump()
        {
            string str1 = string.Empty;
            if (this.IsStatic)
                str1 = "static ";
            string str2 = "<null>";
            if (this.PropertyType != null)
                str2 = this.PropertyType.Name;
            string str3 = string.Empty;
            if (this.CanRead)
                str3 = "get;";
            string str4 = string.Empty;
            if (this.CanWrite)
                str4 = string.Format("{0}set;", this.CanRead ? (object)" " : (object)string.Empty);
            string str5 = "<notifies>";
            if (!this.NotifiesOnChange)
                str5 = "<doesn't notify>";
            string.Format("0x{0:x8} {1}{2} {3} {{{4}{5}}} {6}", (object)this._id, (object)str1, (object)str2, (object)this.Name, (object)str3, (object)str4, (object)str5);
        }

        public override string Name => this._name;

        public override TypeSchema PropertyType => this._type;

        public override TypeSchema AlternateType => (TypeSchema)null;

        public override bool CanRead => this.GetBit(DllPropertySchema.Bits.CanRead);

        public override bool CanWrite => this.GetBit(DllPropertySchema.Bits.CanWrite);

        public override bool IsStatic => this.GetBit(DllPropertySchema.Bits.IsStatic);

        public override ExpressionRestriction ExpressionRestriction => ExpressionRestriction.None;

        public override bool RequiredForCreation => false;

        public override RangeValidator RangeValidator => (RangeValidator)null;

        public override bool NotifiesOnChange => this.GetBit(DllPropertySchema.Bits.NotifiesOnChange);

        private DllTypeSchema OwnerTypeSchema => (DllTypeSchema)this.Owner;

        public override object GetValue(object instance) => this.OwnerTypeSchema.GetPropertyValue(instance, this);

        public override void SetValue(ref object instance, object value) => this.OwnerTypeSchema.SetPropertyValue(instance, this, value);

        private DllLoadResult OwnerLoadResult => (DllLoadResult)this.Owner.Owner;

        private unsafe bool QueryPropertyName(IntPtr property)
        {
            bool flag = false;
            char* name;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyName(property, out name)))
            {
                this._name = NotifyService.CanonicalizeString(new string(name));
                flag = true;
            }
            return flag;
        }

        private bool QueryPropertyType(IntPtr property)
        {
            bool flag = false;
            uint type;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyType(property, out type)))
            {
                this._type = DllLoadResult.MapType(type);
                flag = true;
            }
            return flag;
        }

        private bool QueryCanRead(IntPtr property)
        {
            bool flag = false;
            bool canRead;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyCanRead(property, out canRead)))
            {
                this.SetBit(DllPropertySchema.Bits.CanRead, canRead);
                flag = true;
            }
            return flag;
        }

        private bool QueryCanWrite(IntPtr property)
        {
            bool flag = false;
            bool canWrite;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyCanWrite(property, out canWrite)))
            {
                this.SetBit(DllPropertySchema.Bits.CanWrite, canWrite);
                flag = true;
            }
            return flag;
        }

        private bool QueryIsStatic(IntPtr property)
        {
            bool flag = false;
            bool isStatic;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyIsStatic(property, out isStatic)))
            {
                this.SetBit(DllPropertySchema.Bits.IsStatic, isStatic);
                flag = true;
            }
            return flag;
        }

        private bool CheckNativeReturn(uint hr) => DllLoadResult.CheckNativeReturn(hr, "IUIXProperty");

        private bool QueryNotifiesOnChange(IntPtr property)
        {
            bool flag = false;
            bool notifiesOnChange;
            if (this.CheckNativeReturn(NativeApi.SpQueryPropertyNotifiesOnChange(property, out notifiesOnChange)))
            {
                this.SetBit(DllPropertySchema.Bits.NotifiesOnChange, notifiesOnChange);
                flag = true;
            }
            return flag;
        }

        private bool GetBit(DllPropertySchema.Bits lookupBit) => this._bits[(int)lookupBit];

        private void SetBit(DllPropertySchema.Bits changeBit, bool value) => this._bits[(int)changeBit] = value;

        private enum Bits : uint
        {
            CanRead = 1,
            CanWrite = 2,
            IsStatic = 4,
            NotifiesOnChange = 8,
        }
    }
}
