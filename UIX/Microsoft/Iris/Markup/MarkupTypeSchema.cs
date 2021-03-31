// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupTypeSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Markup
{
    internal abstract class MarkupTypeSchema : TypeSchema
    {
        private const int c_TypeDepthShift = 27;
        private const uint c_ScriptOffsetMask = 134217727;
        public const uint InvalidScriptId = 4294967295;
        private MarkupLoadResult _owner;
        private string _name;
        private MarkupTypeSchema _baseType;
        private uint _typeDepth;
        private int _totalPropertiesAndLocalsCount;
        private PropertySchema[] _properties = PropertySchema.EmptyList;
        private MethodSchema[] _methods = MethodSchema.EmptyList;
        private Map<MethodSignatureKey, MethodSchema> _methodLookupTable;
        private MethodSchema[] _virtualMethods = MethodSchema.EmptyList;
        private SymbolReference[] _symbolReferenceTable;
        private IntPtr _addressOfInheritableSymbolsTable;
        private SymbolRecord[] _inheritableSymbolsTable;
        private uint _initializePropertiesOffset = uint.MaxValue;
        private uint _initializeLocalsInputOffset = uint.MaxValue;
        private uint _initializeContentOffset = uint.MaxValue;
        private uint[] _refreshGroupOffsets;
        private uint[] _initialEvaluateOffsets;
        private uint[] _finalEvaluateOffsets;
        private uint _listenerCount;
        private bool _sealed;
        private object _loadData;
        private static object[] EmptyParameterList = new object[0];

        public static MarkupTypeSchema Build(
          TypeSchema markupTypeDefinition,
          MarkupLoadResult owner,
          string name)
        {
            if (markupTypeDefinition == ClassSchema.Type)
                return (MarkupTypeSchema)new ClassTypeSchema(owner, name);
            if (markupTypeDefinition == UISchema.Type)
                return (MarkupTypeSchema)new UIClassTypeSchema(owner, name);
            if (markupTypeDefinition == EffectSchema.Type)
                return (MarkupTypeSchema)new EffectClassTypeSchema(owner, name);
            if (markupTypeDefinition == DataTypeSchema.Type)
                return (MarkupTypeSchema)new MarkupDataTypeSchema(owner, name);
            return markupTypeDefinition == DataQuerySchema.Type ? (MarkupTypeSchema)new MarkupDataQuerySchema(owner, name) : (MarkupTypeSchema)null;
        }

        public MarkupTypeSchema(MarkupLoadResult owner, string name)
          : base((LoadResult)owner)
        {
            this._owner = owner;
            this._name = name;
        }

        public abstract MarkupType MarkupType { get; }

        public void SetBaseType(MarkupTypeSchema baseType) => this._baseType = baseType;

        public object LoadData
        {
            get => this._loadData;
            set => this._loadData = value;
        }

        public virtual void BuildProperties()
        {
        }

        public void Seal()
        {
            if (this._sealed)
                return;
            this.SealWorker();
        }

        protected virtual void SealWorker()
        {
            this._sealed = true;
            this._loadData = (object)null;
            if (this._baseType == null)
            {
                this._typeDepth = 1U;
            }
            else
            {
                this._baseType.Seal();
                this._typeDepth = this._baseType._typeDepth + 1U;
            }
        }

        public bool Sealed => this._sealed;

        protected override void OnDispose()
        {
            base.OnDispose();
            foreach (DisposableObject property in this._properties)
                property.Dispose((object)this);
            foreach (DisposableObject method in this._methods)
                method.Dispose((object)this);
            foreach (DisposableObject virtualMethod in this._virtualMethods)
                virtualMethod.Dispose((object)this);
        }

        public override string Name => this._name;

        public override string AlternateName => (string)null;

        public override TypeSchema Base => this._baseType == null ? this.DefaultBase : (TypeSchema)this._baseType;

        public override bool Contractual => false;

        public override bool IsNativeAssignableFrom(object check) => false;

        public override bool IsNativeAssignableFrom(TypeSchema checkSchema) => false;

        public MarkupTypeSchema MarkupTypeBase => this._baseType;

        protected abstract TypeSchema DefaultBase { get; }

        public override bool Disposable => true;

        public override bool HasDefaultConstructor => true;

        public object Run(
          IMarkupTypeBase markupType,
          uint scriptId,
          bool ignoreErrors,
          ParameterContext parameterContext)
        {
            ErrorManager.EnterContext((object)markupType.TypeSchema.Owner.ErrorContextUri, ignoreErrors);
            MarkupTypeSchema markupTypeSchema = this;
            uint num = scriptId >> 27;
            while ((int)num != (int)markupTypeSchema._typeDepth)
                markupTypeSchema = markupTypeSchema._baseType;
            object obj = markupTypeSchema.RunAtOffset(markupType, scriptId & 134217727U, parameterContext);
            ErrorManager.ExitContext();
            return obj;
        }

        protected object RunAtOffset(
          IMarkupTypeBase markupType,
          uint scriptOffset,
          ParameterContext parameterContext)
        {
            object obj = (object)null;
            if (markupType.ScriptEnabled)
            {
                InterpreterContext context = InterpreterContext.Acquire(markupType, this, scriptOffset, parameterContext);
                obj = Interpreter.Run(context);
                if (context != null)
                    InterpreterContext.Release(context);
            }
            if (obj == Interpreter.ScriptError && !ErrorManager.IgnoringErrors)
                markupType.NotifyScriptErrors();
            return obj;
        }

        protected object RunAtOffset(IMarkupTypeBase markupType, uint scriptOffset) => this.RunAtOffset(markupType, scriptOffset, new ParameterContext());

        public override ConstructorSchema FindConstructor(TypeSchema[] parameters) => (ConstructorSchema)null;

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

        public override PropertySchema[] Properties => this._properties;

        public override MethodSchema[] Methods => this._methods;

        public override MethodSchema FindMethod(string name, TypeSchema[] parameters)
        {
            MethodSchema methodSchema = (MethodSchema)null;
            if (this._methodLookupTable != null)
                this._methodLookupTable.TryGetValue(new MethodSignatureKey(name, parameters), out methodSchema);
            return methodSchema;
        }

        public MethodSchema[] VirtualMethods => this._virtualMethods;

        public override bool HasInitializer => true;

        protected void InitializeInstance(IMarkupTypeBase classBase)
        {
            if (!this.InitializePropertiesLocalsInputContent(classBase, true))
                return;
            this.RefreshAllListeners(classBase);
            if (!this.RunInitialEvaluates(classBase))
                return;
            classBase.NotifyInitialized();
        }

        private bool InitializePropertiesLocalsInputContent(
          IMarkupTypeBase classBase,
          bool shouldInitializeContent)
        {
            ErrorManager.EnterContext((object)this);
            try
            {
                if (!this.RunInitializeScript(classBase, this._initializePropertiesOffset))
                    return false;
                if (this._baseType != null)
                {
                    bool shouldInitializeContent1 = shouldInitializeContent && this._initializeContentOffset == uint.MaxValue;
                    if (!this._baseType.InitializePropertiesLocalsInputContent(classBase, shouldInitializeContent1))
                        return false;
                }
                if (!this.RunInitializeScript(classBase, this._initializeLocalsInputOffset))
                    return false;
                if (shouldInitializeContent)
                {
                    if (!this.RunInitializeScript(classBase, this._initializeContentOffset))
                        return false;
                }
            }
            finally
            {
                ErrorManager.ExitContext();
            }
            return true;
        }

        private void RefreshAllListeners(IMarkupTypeBase scriptHost)
        {
            if (this._baseType != null)
                this._baseType.RefreshAllListeners(scriptHost);
            this.RunOffsets(scriptHost, this._refreshGroupOffsets, true);
        }

        protected virtual bool RunInitialEvaluates(IMarkupTypeBase scriptHost) => (this._baseType == null || this._baseType.RunInitialEvaluates(scriptHost)) && this.RunOffsets(scriptHost, this._initialEvaluateOffsets);

        public bool RunFinalEvaluates(IMarkupTypeBase scriptHost) => (this._baseType == null || this._baseType.RunFinalEvaluates(scriptHost)) && this.RunOffsets(scriptHost, this._finalEvaluateOffsets);

        private bool RunOffsets(IMarkupTypeBase scriptHost, uint[] scriptOffsets) => this.RunOffsets(scriptHost, scriptOffsets, false);

        private bool RunOffsets(IMarkupTypeBase scriptHost, uint[] scriptOffsets, bool ignoreErrors)
        {
            bool flag = true;
            if (scriptOffsets != null)
            {
                ErrorManager.EnterContext((object)this, ignoreErrors);
                foreach (uint scriptOffset in scriptOffsets)
                {
                    if (!this.RunInitializeScript(scriptHost, scriptOffset))
                    {
                        flag = false;
                        break;
                    }
                }
                ErrorManager.ExitContext();
            }
            return flag;
        }

        protected bool RunInitializeScript(IMarkupTypeBase scriptHost, uint scriptOffset)
        {
            if (scriptOffset == uint.MaxValue || this.RunAtOffset(scriptHost, scriptOffset) != Interpreter.ScriptError || ErrorManager.IgnoringErrors)
                return true;
            ErrorManager.ReportWarning("Script runtime failure: Scripting errors have prevented '{0}' from properly initializing and will affect its operation", (object)this._name);
            return false;
        }

        public override EventSchema FindEvent(string name) => (EventSchema)null;

        public override object FindCanonicalInstance(string name) => (object)null;

        public override Result TypeConverter(
          object from,
          TypeSchema fromType,
          out object instance)
        {
            instance = (object)null;
            return Result.Fail("Type conversion is not available for '{0}'", (object)this._name);
        }

        public override bool SupportsTypeConversion(TypeSchema fromType) => false;

        public override bool SupportsBinaryEncoding => false;

        public override void EncodeBinary(ByteCodeWriter writer, object instance)
        {
        }

        public override object DecodeBinary(ByteCodeReader reader) => (object)null;

        public override object PerformOperation(object left, object right, OperationType op) => (object)null;

        public override bool SupportsOperation(OperationType op) => false;

        public override bool IsNullAssignable => true;

        public override bool IsRuntimeImmutable => false;

        public override int FindTypeHint => -1;

        public SymbolReference[] SymbolReferenceTable => this._symbolReferenceTable;

        public SymbolRecord[] InheritableSymbolsTable
        {
            get
            {
                if (this._inheritableSymbolsTable == null)
                {
                    if (this._addressOfInheritableSymbolsTable != IntPtr.Zero)
                        CompiledMarkupLoader.DecodeInheritableSymbolTable(this, (ByteCodeReader)null, this._addressOfInheritableSymbolsTable);
                    else
                        this._inheritableSymbolsTable = SymbolRecord.EmptyList;
                }
                return this._inheritableSymbolsTable;
            }
        }

        public uint TypeDepth => this._typeDepth;

        public int TotalPropertiesAndLocalsCount => this._totalPropertiesAndLocalsCount;

        public uint InitializePropertiesOffset => this._initializePropertiesOffset;

        public uint InitializeLocalsInputOffset => this._initializeLocalsInputOffset;

        public uint InitializeContentOffset => this._initializeContentOffset;

        public uint[] RefreshGroupOffsets => this._refreshGroupOffsets;

        public uint[] InitialEvaluateOffsets => this._initialEvaluateOffsets;

        public uint[] FinalEvaluateOffsets => this._finalEvaluateOffsets;

        public uint ListenerCount => this._listenerCount;

        public uint TotalListenerCount => this._listenerCount + (this._baseType == null ? 0U : this._baseType.TotalListenerCount);

        public uint EncodeScriptOffsetAsId(uint scriptOffset) => scriptOffset | this._typeDepth << 27;

        public string LocallyUniqueId => this._typeDepth.ToString();

        public void SetTypeDepth(uint typeDepth) => this._typeDepth = typeDepth;

        public void SetPropertyList(PropertySchema[] properties) => this._properties = properties;

        public void SetMethodList(MethodSchema[] methods)
        {
            this._methods = methods;
            this._methodLookupTable = new Map<MethodSignatureKey, MethodSchema>(methods.Length);
            foreach (MethodSchema method in this._methods)
                this._methodLookupTable[new MethodSignatureKey(method.Name, method.ParameterTypes)] = method;
        }

        public void SetVirtualMethodList(MethodSchema[] virtualMethods) => this._virtualMethods = virtualMethods;

        public void SetSymbolReferenceTable(SymbolReference[] symbolTable) => this._symbolReferenceTable = symbolTable;

        public void SetInheritableSymbolsTable(SymbolRecord[] symbolTable) => this._inheritableSymbolsTable = symbolTable;

        public void SetAddressOfInheritableSymbolTable(IntPtr address) => this._addressOfInheritableSymbolsTable = address;

        public void SetRefreshListenerGroupOffsets(uint[] refreshGroupOffsets) => this._refreshGroupOffsets = refreshGroupOffsets;

        public void SetInitialEvaluateOffsets(uint[] initialEvaluateOffsets) => this._initialEvaluateOffsets = initialEvaluateOffsets;

        public void SetFinalEvaluateOffsets(uint[] finalEvaluateOffsets) => this._finalEvaluateOffsets = finalEvaluateOffsets;

        public void SetListenerCount(uint listenerCount) => this._listenerCount = listenerCount;

        public void SetTotalPropertiesAndLocalsCount(int totalPropertiesAndLocalsCount) => this._totalPropertiesAndLocalsCount = totalPropertiesAndLocalsCount;

        public void SetInitializePropertiesOffset(uint offset) => this._initializePropertiesOffset = offset;

        public void SetInitializeLocalsInputOffset(uint offset) => this._initializeLocalsInputOffset = offset;

        public void SetInitializeContentOffset(uint offset) => this._initializeContentOffset = offset;
    }
}
