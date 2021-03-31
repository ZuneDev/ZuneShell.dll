// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.EnumSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Data;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using System;
using System.Diagnostics;

namespace Microsoft.Iris.Markup
{
    internal class EnumSchema : TypeSchema
    {
        private string _name;
        private Type _enumType;
        protected Map<string, int> _nameToValueMap;
        private bool _isFlags;
        private string[] _names;
        private int[] _values;

        public EnumSchema(LoadResult owner)
          : base(owner)
        {
        }

        protected void Initialize(
          string enumName,
          Type runtimeType,
          bool isFlags,
          string[] names,
          int[] values)
        {
            this._name = enumName;
            this._enumType = runtimeType;
            this._isFlags = isFlags;
            this._names = names;
            this._values = values;
        }

        protected virtual void InitializeNameToValueMap()
        {
            this._nameToValueMap = new Map<string, int>(this._names.Length);
            for (int index = 0; index < this._names.Length; ++index)
                this._nameToValueMap[this._names[index]] = this._values[index];
            this._names = (string[])null;
            this._values = (int[])null;
        }

        [Conditional("DEBUG")]
        protected void DEBUG_AssertInitialized()
        {
        }

        public Vector<string> Names
        {
            get
            {
                Vector<string> vector = new Vector<string>();
                foreach (string key in this.NameToValueMap.Keys)
                    vector.Add(key);
                return vector;
            }
        }

        protected Map<string, int> NameToValueMap
        {
            get
            {
                if (this._nameToValueMap == null)
                    this.InitializeNameToValueMap();
                return this._nameToValueMap;
            }
        }

        protected bool IsFlags => this._isFlags;

        public override string Name => this._name;

        public override string AlternateName => (string)null;

        public override TypeSchema Base => (TypeSchema)ObjectSchema.Type;

        public override bool Contractual => false;

        public override Type RuntimeType => this._enumType;

        public override bool IsNativeAssignableFrom(object check) => this.RuntimeType.IsAssignableFrom(check.GetType());

        public override bool IsNativeAssignableFrom(TypeSchema check) => false;

        public override bool Disposable => false;

        public override object ConstructDefault() => this.EnumValueToObject(0);

        public override bool HasDefaultConstructor => true;

        public override void InitializeInstance(ref object instance)
        {
        }

        public override bool HasInitializer => false;

        public override ConstructorSchema FindConstructor(TypeSchema[] parameters) => (ConstructorSchema)null;

        public override PropertySchema FindProperty(string name) => (PropertySchema)null;

        public override MethodSchema FindMethod(string name, TypeSchema[] parameters) => (MethodSchema)null;

        public override EventSchema FindEvent(string name) => (EventSchema)null;

        public override object FindCanonicalInstance(string name)
        {
            int num;
            return this.NameToValue(name, out num) ? this.EnumValueToObject(num) : (object)null;
        }

        public bool NameToValue(string name, out int value) => this.NameToValueMap.TryGetValue(name, out value);

        protected virtual object EnumValueToObject(int value) => Enum.ToObject(this._enumType, value);

        protected virtual int ValueFromObject(object obj) => (int)obj;

        public override PropertySchema[] Properties => PropertySchema.EmptyList;

        private Result ConvertFromString(string value, out object instance)
        {
            instance = (object)null;
            int num1 = 0;
            if (this._isFlags && value.IndexOf(',') >= 0)
            {
                foreach (string name in StringUtility.SplitAndTrim(',', value))
                {
                    int num2;
                    if (!this.NameToValue(name, out num2))
                        return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)name, (object)this._name);
                    num1 |= num2;
                }
            }
            else if (!this.NameToValue(value, out num1))
                return Result.Fail("Unable to convert \"{0}\" to type '{1}'", (object)value, (object)this._name);
            instance = this.EnumValueToObject(num1);
            return Result.Success;
        }

        public override Result TypeConverter(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            Result result;
            if (StringSchema.Type.IsAssignableFrom(fromType))
                result = this.ConvertFromString((string)from, out instance);
            else if (Int32Schema.Type.IsAssignableFrom(fromType))
            {
                instance = this.EnumValueToObject((int)from);
                result = Result.Success;
            }
            else
            {
                instance = (object)null;
                result = Result.Fail("Unsupported");
            }
            return result;
        }

        public override bool SupportsTypeConversion(TypeSchema fromType) => StringSchema.Type.IsAssignableFrom(fromType) || Int32Schema.Type.IsAssignableFrom(fromType);

        public override void EncodeBinary(ByteCodeWriter writer, object instance) => writer.WriteInt32(this.ValueFromObject(instance));

        public override object DecodeBinary(ByteCodeReader reader) => this.EnumValueToObject(reader.ReadInt32());

        public override bool SupportsBinaryEncoding => true;

        public override object PerformOperation(object left, object right, OperationType op)
        {
            bool flag = object.Equals(left, right);
            switch (op)
            {
                case OperationType.RelationalEquals:
                    return BooleanBoxes.Box(flag);
                case OperationType.RelationalNotEquals:
                    return BooleanBoxes.Box(!flag);
                default:
                    return (object)null;
            }
        }

        public override bool SupportsOperation(OperationType op)
        {
            switch (op)
            {
                case OperationType.RelationalEquals:
                case OperationType.RelationalNotEquals:
                    return true;
                default:
                    return false;
            }
        }

        public override bool IsNullAssignable => false;

        public override bool IsRuntimeImmutable => true;

        public override bool IsEnum => true;

        public override int FindTypeHint => -1;
    }
}
