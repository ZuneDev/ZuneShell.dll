// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupMethodSchema
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup
{
    internal abstract class MarkupMethodSchema : MethodSchema
    {
        private string _name;
        private TypeSchema _returnType;
        private TypeSchema[] _parameterTypes;
        private string[] _parameterNames;
        protected uint _codeOffset = uint.MaxValue;
        private int _virtualId = -1;
        private bool _isVirtualThunk;
        public static readonly string[] s_emptyStringArray = new string[0];
        public static readonly MarkupMethodSchema[] EmptyMethodList = new MarkupMethodSchema[0];

        public static MarkupMethodSchema Build(
          TypeSchema markupTypeBase,
          MarkupTypeSchema owner,
          string name,
          TypeSchema returnType,
          TypeSchema[] parameterTypes,
          string[] parameterNames,
          bool isVirtualThunk)
        {
            MarkupMethodSchema markupMethodSchema = (MarkupMethodSchema)null;
            if (markupTypeBase == ClassSchema.Type || markupTypeBase == EffectSchema.Type)
                markupMethodSchema = (MarkupMethodSchema)new ClassMethodSchema((ClassTypeSchema)owner, name, returnType, parameterTypes, parameterNames);
            else if (markupTypeBase == UISchema.Type)
                markupMethodSchema = (MarkupMethodSchema)new UIClassMethodSchema((UIClassTypeSchema)owner, name, returnType, parameterTypes, parameterNames);
            markupMethodSchema._isVirtualThunk = isVirtualThunk;
            return markupMethodSchema;
        }

        public static MarkupMethodSchema Build(
          TypeSchema markupTypeBase,
          MarkupTypeSchema owner,
          string name,
          TypeSchema returnType,
          TypeSchema[] parameterTypes,
          string[] parameterNames)
        {
            return MarkupMethodSchema.Build(markupTypeBase, owner, name, returnType, parameterTypes, parameterNames, false);
        }

        public static MarkupMethodSchema BuildVirtualThunk(
          TypeSchema markupTypeBase,
          MarkupMethodSchema virtualMethod)
        {
            return MarkupMethodSchema.Build(markupTypeBase, (MarkupTypeSchema)virtualMethod.Owner, virtualMethod.Name, virtualMethod.ReturnType, virtualMethod.ParameterTypes, virtualMethod.ParameterNames, true);
        }

        protected MarkupMethodSchema(
          MarkupTypeSchema owner,
          string name,
          TypeSchema returnType,
          TypeSchema[] parameterTypes,
          string[] parameterNames)
          : base((TypeSchema)owner)
        {
            this._name = name;
            this._returnType = returnType;
            this._parameterTypes = parameterTypes;
            this._parameterNames = parameterNames;
            for (int index = 0; index < this._parameterNames.Length; ++index)
                this._parameterNames[index] = NotifyService.CanonicalizeString(this._parameterNames[index]);
        }

        public override string Name => this._name;

        public override TypeSchema[] ParameterTypes => this._parameterTypes;

        public string[] ParameterNames => this._parameterNames;

        public override TypeSchema ReturnType => this._returnType;

        public override bool IsStatic => false;

        public bool IsVirtual => this._virtualId >= 0;

        public int VirtualId => this._virtualId;

        public bool IsVirtualThunk => this._isVirtualThunk;

        public uint CodeOffset => this._codeOffset;

        public void SetCodeOffset(uint codeOffset) => this._codeOffset = codeOffset;

        public void SetVirtualId(int virtualId) => this._virtualId = virtualId;

        public override object Invoke(object instance, object[] parameters)
        {
            IMarkupTypeBase markupTypeBase = this.GetMarkupTypeBase(instance);
            if (markupTypeBase == null)
                return (object)null;
            return this._isVirtualThunk ? this.CallVirt(markupTypeBase, parameters) : this.CallDirect(markupTypeBase, parameters);
        }

        private object CallVirt(IMarkupTypeBase markupInstance, object[] parameters)
        {
            MarkupTypeSchema typeSchema = (MarkupTypeSchema)markupInstance.TypeSchema;
            MarkupMethodSchema markupMethodSchema = (MarkupMethodSchema)null;
            while (true)
            {
                foreach (MarkupMethodSchema virtualMethod in typeSchema.VirtualMethods)
                {
                    if (virtualMethod.VirtualId == this._virtualId)
                    {
                        markupMethodSchema = virtualMethod;
                        break;
                    }
                }
                if (markupMethodSchema == null)
                    typeSchema = (MarkupTypeSchema)typeSchema.Base;
                else
                    break;
            }
            return markupMethodSchema.CallDirect(markupInstance, parameters);
        }

        private object CallDirect(IMarkupTypeBase markupInstance, object[] parameters) => markupInstance.RunScript(this._codeOffset, false, new ParameterContext(this._parameterNames, parameters));

        protected abstract IMarkupTypeBase GetMarkupTypeBase(object instance);
    }
}
