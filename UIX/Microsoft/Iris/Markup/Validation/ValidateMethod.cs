// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateMethod
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateMethod : Microsoft.Iris.Markup.Validation.Validate
    {
        private string _methodName;
        private ValidateTypeIdentifier _returnType;
        private Vector<MethodSpecifier> _specifiers;
        private ValidateParameterDefinitionList _paramList;
        private ValidateCode _body;
        private bool _hasVirtualKeyword;
        private bool _hasOverrideKeyword;
        private MarkupMethodSchema _methodExport;
        private MarkupMethodSchema _foundBaseMethod;

        public ValidateMethod(
          SourceMarkupLoader owner,
          int line,
          int column,
          string methodName,
          ValidateTypeIdentifier returnType,
          Vector<MethodSpecifier> specifiers,
          ValidateParameterDefinitionList paramList,
          ValidateCode body)
          : base(owner, line, column)
        {
            this._methodName = methodName;
            this._returnType = returnType;
            this._specifiers = specifiers;
            this._paramList = paramList;
            this._body = body;
            this._body.MarkAsNotEmbedded();
            this._body.DisallowTriggers();
        }

        public void Validate(ValidateClass validateOwner, ValidateContext context)
        {
            if (this.HasErrors)
                return;
            if (context.CurrentPass == LoadPass.PopulatePublicModel)
            {
                this._returnType.Validate();
                if (this._paramList != null)
                    this._paramList.Validate(context);
                context.NotifyMethodFound(this._methodName);
                if (this._paramList != null && this._paramList.HasErrors || (this._returnType.HasErrors || this.HasErrors))
                {
                    this.MarkHasErrors();
                }
                else
                {
                    TypeSchema[] parameterTypes = TypeSchema.EmptyList;
                    string[] parameterNames = MarkupMethodSchema.s_emptyStringArray;
                    if (this._paramList != null)
                    {
                        int count = this._paramList.Parameters.Count;
                        parameterTypes = new TypeSchema[count];
                        parameterNames = new string[count];
                        for (int index = 0; index < count; ++index)
                        {
                            ValidateParameterDefinition parameter = (ValidateParameterDefinition)this._paramList.Parameters[index];
                            parameterNames[index] = parameter.Name;
                            parameterTypes[index] = parameter.FoundType;
                        }
                    }
                    this._methodExport = MarkupMethodSchema.Build(validateOwner.ObjectType, validateOwner.TypeExport, this._methodName, this._returnType.FoundType, parameterTypes, parameterNames);
                    for (int index1 = 0; index1 < this._specifiers.Count; ++index1)
                    {
                        for (int index2 = index1 + 1; index2 < this._specifiers.Count; ++index2)
                        {
                            if (this._specifiers[index1] == this._specifiers[index2])
                                this.ReportError("Duplicate modifier '{0}'", this._specifiers[index1].ToString());
                        }
                    }
                    this._hasVirtualKeyword = this._specifiers.Contains(MethodSpecifier.Virtual);
                    this._hasOverrideKeyword = this._specifiers.Contains(MethodSpecifier.Override);
                    if (!this._hasVirtualKeyword || !this._hasOverrideKeyword)
                        return;
                    this.ReportError("Only one of 'virtual' and 'override' is allowed");
                }
            }
            else
            {
                if (context.CurrentPass != LoadPass.Full)
                    return;
                context.NotifyMethodScopeEnter(this);
                MarkupTypeSchema typeSchema = validateOwner.TypeExport.Base as MarkupTypeSchema;
                if (typeSchema != null)
                {
                    MarkupMethodSchema methodExact = this.FindMethodExact(typeSchema, this._methodExport, true);
                    if (methodExact != null)
                    {
                        if (methodExact.IsVirtual && !this._hasOverrideKeyword)
                            this.ReportError("Method '{0}' is virtual in '{1}', must use 'override' to override", this._methodName, methodExact.Owner.Name);
                        if (!methodExact.IsVirtual)
                            this.ReportError("Method '{0}' was already defined in '{1}' with the same signature and was not declared virtual", this._methodName, methodExact.Owner.Name);
                        if ((this._hasVirtualKeyword || this._hasOverrideKeyword) && methodExact.ReturnType != this._methodExport.ReturnType)
                            this.ReportError("Method '{0}' overrides base method from '{1}', but return types do not match: '{2}' on override, '{3}' on base.", this._methodName, methodExact.Owner.Name, this._methodExport.ReturnType.Name, methodExact.ReturnType.Name);
                        if (methodExact.IsVirtual && this._hasOverrideKeyword)
                        {
                            for (MarkupTypeSchema markupTypeSchema = typeSchema; markupTypeSchema != null && this._foundBaseMethod == null; markupTypeSchema = markupTypeSchema.Base as MarkupTypeSchema)
                            {
                                foreach (MarkupMethodSchema virtualMethod in markupTypeSchema.VirtualMethods)
                                {
                                    if (virtualMethod.VirtualId == methodExact.VirtualId)
                                    {
                                        this._foundBaseMethod = virtualMethod;
                                        break;
                                    }
                                }
                            }
                        }
                        this._methodExport.SetVirtualId(methodExact.VirtualId);
                    }
                    else if (this._hasOverrideKeyword)
                        this.ReportError("Method '{0}' declared override, but no base method found to override", this._methodName);
                }
                else if (this._hasOverrideKeyword)
                    this.ReportError("Method '{0}' declared override, but no base method found to override", this._methodName);
                if (this._hasVirtualKeyword || this._hasOverrideKeyword)
                    validateOwner.AddVirtualMethod(this._methodExport, this._foundBaseMethod);
                if (this._paramList != null)
                    this._paramList.Validate(context);
                this._body.Validate(new TypeRestriction(this._returnType.FoundType), context);
                context.NotifyMethodScopeExit(this);
            }
        }

        private MarkupMethodSchema FindMethodExact(
          MarkupTypeSchema typeSchema,
          MarkupMethodSchema methodCheck,
          bool deep)
        {
            MarkupTypeSchema markupTypeSchema = typeSchema;
            do
            {
                foreach (MarkupMethodSchema method in markupTypeSchema.Methods)
                {
                    if (IsExactMatch(method, methodCheck))
                        return method;
                }
                markupTypeSchema = markupTypeSchema.Base as MarkupTypeSchema;
            }
            while (deep && markupTypeSchema != null);
            return null;
        }

        public static bool IsExactMatch(MarkupMethodSchema method, MarkupMethodSchema methodCheck)
        {
            if (method == null || methodCheck == null || (!(method.Name == methodCheck.Name) || method.ParameterTypes.Length != methodCheck.ParameterTypes.Length))
                return false;
            bool flag = true;
            for (int index = 0; index < method.ParameterTypes.Length; ++index)
            {
                if (method.ParameterTypes[index] != methodCheck.ParameterTypes[index])
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public MarkupMethodSchema MethodExport => this._methodExport;

        public MarkupMethodSchema FoundBaseMethod => this._foundBaseMethod;

        public bool HasVirtualKeyword => this._hasVirtualKeyword;

        public bool HasOverrideKeyword => this._hasOverrideKeyword;

        public ValidateCode Body => this._body;

        public override string ToString()
        {
            string str = "";
            if (this._paramList != null)
            {
                foreach (ValidateParameterDefinition parameter in this._paramList.Parameters)
                {
                    if (str.Length > 0)
                        str += ", ";
                    str += parameter.FoundType.ToString() + " " + parameter.Name;
                }
            }
            return string.Format("{0} {1}({2})", _methodName, _returnType.FoundType, str);
        }
    }
}
