// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.UIXTypeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using System;

namespace Microsoft.Iris.Markup
{
    internal class UIXTypeSchema : TypeSchema
    {
        private short _typeID;
        private short _baseTypeID;
        private string _name;
        private string _alternateName;
        private TypeSchema _baseType;
        private DefaultConstructHandler _defaultConstructor;
        private ConstructorSchema[] _constructors;
        private PropertySchema[] _properties;
        private MethodSchema[] _methods;
        private EventSchema[] _events;
        private TypeConverterHandler _typeConverter;
        private SupportsTypeConversionHandler _supportsTypeConversion;
        private EncodeBinaryHandler _encodeBinary;
        private DecodeBinaryHandler _decodeBinary;
        private PerformOperationHandler _performOperation;
        private SupportsOperationHandler _supportsOperation;
        private FindCanonicalInstanceHandler _findCanonicalInstance;
        private Type _instanceType;
        private UIXTypeFlags _flags;
        private static object[] EmptyParameterList = new object[0];

        public UIXTypeSchema(
          short typeID,
          string name,
          string alternateName,
          short baseTypeID,
          Type instanceType,
          UIXTypeFlags flags)
          : base((LoadResult)MarkupSystem.UIXGlobal)
        {
            this._typeID = typeID;
            this._baseTypeID = baseTypeID;
            this._name = name;
            this._alternateName = alternateName;
            this._instanceType = instanceType;
            this._flags = flags;
            UIXTypes.RegisterTypeForID(typeID, (TypeSchema)this);
        }

        public void Initialize(
          DefaultConstructHandler defaultConstructor,
          ConstructorSchema[] constructors,
          PropertySchema[] properties,
          MethodSchema[] methods,
          EventSchema[] events,
          FindCanonicalInstanceHandler findCanonicalInstance,
          TypeConverterHandler typeConverter,
          SupportsTypeConversionHandler supportsTypeConversion,
          EncodeBinaryHandler encodeBinary,
          DecodeBinaryHandler decodeBinary,
          PerformOperationHandler performOperation,
          SupportsOperationHandler supportsOperation)
        {
            if (constructors == null)
                constructors = ConstructorSchema.EmptyList;
            if (properties == null)
                properties = PropertySchema.EmptyList;
            if (methods == null)
                methods = MethodSchema.EmptyList;
            if (events == null)
                events = EventSchema.EmptyList;
            this._defaultConstructor = defaultConstructor;
            this._constructors = constructors;
            this._properties = properties;
            this._methods = methods;
            this._events = events;
            this._findCanonicalInstance = findCanonicalInstance;
            this._typeConverter = typeConverter;
            this._supportsTypeConversion = supportsTypeConversion;
            this._encodeBinary = encodeBinary;
            this._decodeBinary = decodeBinary;
            this._performOperation = performOperation;
            this._supportsOperation = supportsOperation;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            foreach (DisposableObject constructor in this._constructors)
                constructor.Dispose((object)this);
            foreach (DisposableObject property in this._properties)
                property.Dispose((object)this);
            foreach (DisposableObject method in this._methods)
                method.Dispose((object)this);
            foreach (DisposableObject disposableObject in this._events)
                disposableObject.Dispose((object)this);
        }

        public override string Name => this._name;

        public override string AlternateName => this._alternateName;

        public override TypeSchema Base
        {
            get
            {
                if (this._baseTypeID != (short)-1 && this._baseType == null)
                    this._baseType = UIXTypes.MapIDToType(this._baseTypeID);
                return this._baseType;
            }
        }

        public override bool Contractual => false;

        public override Type RuntimeType => this._instanceType;

        public override bool IsNativeAssignableFrom(object check) => this.RuntimeType.IsAssignableFrom(check.GetType());

        public override bool IsNativeAssignableFrom(TypeSchema checkSchema) => false;

        public override bool Disposable => (this._flags & UIXTypeFlags.Disposable) != UIXTypeFlags.None;

        public override bool IsStatic => (this._flags & UIXTypeFlags.Static) != UIXTypeFlags.None;

        public override object ConstructDefault() => this._defaultConstructor();

        public override bool HasDefaultConstructor => this._defaultConstructor != null;

        public override bool HasInitializer => false;

        public override void InitializeInstance(ref object instance)
        {
        }

        public override ConstructorSchema FindConstructor(TypeSchema[] parameters)
        {
            for (int index1 = 0; index1 < this._constructors.Length; ++index1)
            {
                ConstructorSchema constructor = this._constructors[index1];
                TypeSchema[] parameterTypes = constructor.ParameterTypes;
                if (parameters.Length == parameterTypes.Length)
                {
                    bool flag = true;
                    for (int index2 = 0; index2 < parameters.Length; ++index2)
                    {
                        if (!parameterTypes[index2].IsAssignableFrom(parameters[index2]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        return constructor;
                }
            }
            return (ConstructorSchema)null;
        }

        public override PropertySchema FindProperty(string name)
        {
            for (int index = 0; index < this._properties.Length; ++index)
            {
                PropertySchema property = this._properties[index];
                if (name == property.Name)
                    return property;
            }
            return (PropertySchema)null;
        }

        public override ConstructorSchema[] Constructors => this._constructors;

        public override PropertySchema[] Properties => this._properties;

        public override MethodSchema[] Methods => this._methods;

        public override EventSchema[] Events => this._events;

        public override MethodSchema FindMethod(string name, TypeSchema[] parameters)
        {
            for (int index1 = 0; index1 < this._methods.Length; ++index1)
            {
                MethodSchema method = this._methods[index1];
                if (name == method.Name)
                {
                    TypeSchema[] parameterTypes = method.ParameterTypes;
                    if (parameters.Length == parameterTypes.Length)
                    {
                        bool flag = true;
                        for (int index2 = 0; index2 < parameters.Length; ++index2)
                        {
                            if (!parameterTypes[index2].IsAssignableFrom(parameters[index2]))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                            return method;
                    }
                }
            }
            return (MethodSchema)null;
        }

        public override EventSchema FindEvent(string name)
        {
            for (int index = 0; index < this._events.Length; ++index)
            {
                EventSchema eventSchema = this._events[index];
                if (name == eventSchema.Name)
                    return eventSchema;
            }
            return (EventSchema)null;
        }

        public override object FindCanonicalInstance(string name) => this._findCanonicalInstance != null ? this._findCanonicalInstance(name) : (object)null;

        public override Result TypeConverter(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            return this._typeConverter(from, fromType, out instance);
        }

        public override bool SupportsTypeConversion(TypeSchema fromType) => this._supportsTypeConversion != null && this._supportsTypeConversion(fromType);

        public override void EncodeBinary(ByteCodeWriter writer, object instance) => this._encodeBinary(writer, instance);

        public override object DecodeBinary(ByteCodeReader reader) => this._decodeBinary(reader);

        public override bool SupportsBinaryEncoding => this._encodeBinary != null;

        public override int FindTypeHint => (int)this._typeID;

        public override object PerformOperation(object left, object right, OperationType op)
        {
            object obj = (object)null;
            if (this._performOperation != null)
                obj = this._performOperation(left, right, op);
            return obj;
        }

        public override bool SupportsOperation(OperationType op) => this._supportsOperation != null && this._supportsOperation(op);

        public override bool IsNullAssignable => !this._instanceType.IsValueType;

        public override bool IsRuntimeImmutable => (this._flags & UIXTypeFlags.Immutable) != UIXTypeFlags.None;

        public override string ErrorContextDescription => this.Name;
    }
}
