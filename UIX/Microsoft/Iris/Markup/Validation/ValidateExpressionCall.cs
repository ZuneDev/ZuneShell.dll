// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionCall
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionCall : ValidateExpression
    {
        private ValidateExpression _target;
        private ValidateTypeIdentifier _typeIdentifier;
        private string _memberName;
        private ValidateParameter _parameterList;
        private TypeSchema[] _foundParameterTypes;
        private SchemaType _foundMemberType;
        private int _foundMemberIndex = -1;
        private bool _foundTargetIsStatic;
        private object _foundCanonicalInstance;
        private bool _isIndexAssignment;

        public ValidateExpressionCall(
          SourceMarkupLoader owner,
          ValidateExpression target,
          string memberName,
          ValidateParameter parameterList,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Call)
        {
            this._target = target;
            this._memberName = memberName;
            this._parameterList = parameterList;
        }

        public ValidateExpressionCall(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier typeIdentifier,
          string memberName,
          ValidateParameter parameterList,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.Call)
        {
            this._typeIdentifier = typeIdentifier;
            this._memberName = memberName;
            this._parameterList = parameterList;
        }

        public ValidateExpression Target => !this._foundTargetIsStatic ? this._target : (ValidateExpression)null;

        public string MemberName => this._memberName;

        public ValidateParameter ParameterList => this._parameterList;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            int foundTargetTypeIndex = -1;
            TypeSchema targetType;
            if (this._target != null)
            {
                if (this._target.ExpressionType == ExpressionType.Symbol)
                {
                    ValidateExpressionSymbol target = (ValidateExpressionSymbol)this._target;
                    target.Validate(TypeRestriction.NotVoid, context, true);
                    foundTargetTypeIndex = target.FoundSymbolIndex;
                    this._foundTargetIsStatic = target.FoundSymbolIsType;
                }
                else
                    this._target.Validate(TypeRestriction.NotVoid, context);
                if (this._target.HasErrors)
                {
                    this.MarkHasErrors();
                    return;
                }
                targetType = this._target.ObjectType;
            }
            else
            {
                this._typeIdentifier.Validate();
                if (this._typeIdentifier.HasErrors)
                {
                    this.MarkHasErrors();
                    return;
                }
                targetType = this._typeIdentifier.FoundType;
                foundTargetTypeIndex = this._typeIdentifier.FoundTypeIndex;
                this._foundTargetIsStatic = true;
            }
            ExpressionRestriction expressionRestriction = ExpressionRestriction.None;
            bool canRead = true;
            bool canWrite = false;
            this._foundParameterTypes = this.BuildParameterTypeList(context);
            if (this._foundParameterTypes == null)
                return;
            if (this._target != null && this._target.ExpressionType == ExpressionType.BaseClass)
                this.ValidateBaseCall(typeRestriction, context);
            else if (this._parameterList == null)
            {
                if (!this.ValidatePropertyOrEvent(targetType, typeRestriction, context, foundTargetTypeIndex, ref expressionRestriction, ref canRead, ref canWrite))
                    return;
            }
            else if (!this.ValidateMethodCall(targetType, typeRestriction, context, ref expressionRestriction))
                return;
            if (expressionRestriction == ExpressionRestriction.NoAccess)
            {
                this.ReportError("Expression access to '{0}' is not available", this._memberName);
            }
            else
            {
                if (this.Usage == ExpressionUsage.LValue && (!canWrite || expressionRestriction == ExpressionRestriction.ReadOnly))
                    this.ReportError("Expression access to '{0}' only supports read operations", this._memberName);
                if (this.Usage == ExpressionUsage.RValue && !canRead)
                    this.ReportError("Expression access to '{0}' only supports write operations", this._memberName);
                if (this.HasErrors)
                    return;
                if (this._foundMemberType == SchemaType.Property && this.Usage == ExpressionUsage.LValue)
                    context.Owner.NotifyFoundPropertyAssignment(this);
                if (this._foundMemberType != SchemaType.Method)
                    return;
                context.Owner.NotifyFoundMethodCall(this);
            }
        }

        private bool ValidatePropertyOrEvent(
          TypeSchema targetType,
          TypeRestriction typeRestriction,
          ValidateContext context,
          int foundTargetTypeIndex,
          ref ExpressionRestriction expressionRestriction,
          ref bool canRead,
          ref bool canWrite)
        {
            PropertySchema propertyDeep = targetType.FindPropertyDeep(this._memberName);
            if (propertyDeep != null && (propertyDeep.IsStatic || !this._foundTargetIsStatic))
            {
                this._foundMemberType = SchemaType.Property;
                this._foundMemberIndex = this.Owner.TrackImportedProperty(propertyDeep);
                expressionRestriction = propertyDeep.ExpressionRestriction;
                canRead = propertyDeep.CanRead;
                canWrite = propertyDeep.CanWrite && !propertyDeep.Owner.IsRuntimeImmutable;
                this.DeclareEvaluationType(propertyDeep.PropertyType, typeRestriction);
                if (!this._foundTargetIsStatic && propertyDeep.NotifiesOnChange)
                    this.DeclareNotifies(context);
            }
            if (this._foundMemberType == SchemaType.None && !this._foundTargetIsStatic)
            {
                EventSchema eventDeep = targetType.FindEventDeep(this._memberName);
                if (eventDeep != null)
                {
                    this._foundMemberType = SchemaType.Event;
                    this._foundMemberIndex = this.Owner.TrackImportedEvent(eventDeep);
                    expressionRestriction = ExpressionRestriction.ReadOnly;
                    this.DeclareEvaluationType((TypeSchema)VoidSchema.Type, typeRestriction);
                    this.DeclareNotifies(context);
                }
            }
            if (this._foundMemberType == SchemaType.None && this._foundTargetIsStatic)
            {
                object canonicalInstance = targetType.FindCanonicalInstance(this._memberName);
                if (canonicalInstance != null)
                {
                    this._foundMemberType = SchemaType.CanonicalInstance;
                    this._foundMemberIndex = foundTargetTypeIndex;
                    this._foundCanonicalInstance = canonicalInstance;
                    expressionRestriction = ExpressionRestriction.ReadOnly;
                    this.DeclareEvaluationType(targetType, typeRestriction);
                }
            }
            if (this._foundMemberType != SchemaType.None)
                return true;
            this.ReportError("Unable to find a Property, Event, or Method called \"{0}\" on '{1}'", this._memberName, targetType.Name);
            return false;
        }

        private bool ValidateMethodCall(
          TypeSchema targetType,
          TypeRestriction typeRestriction,
          ValidateContext context,
          ref ExpressionRestriction expressionRestriction)
        {
            expressionRestriction = ExpressionRestriction.ReadOnly;
            TypeSchema[] foundParameterTypes = this._foundParameterTypes;
            MethodSchema methodDeep = targetType.FindMethodDeep(this._memberName, foundParameterTypes);
            if (methodDeep != null && (methodDeep.IsStatic || !this._foundTargetIsStatic))
            {
                this._foundMemberType = SchemaType.Method;
                this._foundMemberIndex = this.Owner.TrackImportedMethod(methodDeep);
                this.DeclareEvaluationType(methodDeep.ReturnType, typeRestriction);
            }
            if (this._foundMemberType != SchemaType.None)
                return true;
            if (foundParameterTypes.Length == 0)
            {
                this.ReportError("Unable to find a Property, Event, or Method called \"{0}\" on '{1}'", this._memberName, targetType.Name);
                return false;
            }
            string empty = string.Empty;
            bool flag = true;
            foreach (TypeSchema typeSchema in foundParameterTypes)
            {
                if (!flag)
                    empty += ", ";
                empty += typeSchema.Name;
                flag = false;
            }
            this.ReportError("Unable to find a Method \"{0}\" that accepts parameters '{1}' on '{2}'", this._memberName, empty, targetType.Name);
            return false;
        }

        private void ValidateBaseCall(TypeRestriction typeRestriction, ValidateContext context)
        {
            MarkupMethodSchema markupMethodSchema = context.CurrentMethod != null ? context.CurrentMethod.FoundBaseMethod : (MarkupMethodSchema)null;
            if (markupMethodSchema != null)
            {
                if (this._parameterList == null || this._memberName != markupMethodSchema.Name)
                    this.ReportError("'base' keyword can only be used to call the base virtual method inside an override");
                else if (new MethodSignatureKey(markupMethodSchema.Name, markupMethodSchema.ParameterTypes).Equals((object)new MethodSignatureKey(this._memberName, this._foundParameterTypes)))
                {
                    this._foundMemberType = SchemaType.Method;
                    this._foundMemberIndex = this.Owner.TrackImportedMethod((MethodSchema)markupMethodSchema);
                    this.DeclareEvaluationType(markupMethodSchema.ReturnType, typeRestriction);
                }
                else
                    this.ReportError("'base' keyword can only be used to call the base virtual method inside an override");
            }
            else
                this.MarkHasErrors();
        }

        private TypeSchema[] BuildParameterTypeList(ValidateContext context)
        {
            TypeSchema[] typeSchemaArray = TypeSchema.EmptyList;
            if (this._parameterList != ValidateParameter.EmptyList)
            {
                int length = 0;
                for (ValidateParameter validateParameter = this._parameterList; validateParameter != null; validateParameter = validateParameter.Next)
                {
                    validateParameter.Validate(context);
                    if (validateParameter.HasErrors)
                        this.MarkHasErrors();
                    ++length;
                }
                if (this.HasErrors)
                    return (TypeSchema[])null;
                typeSchemaArray = new TypeSchema[length];
                int index = 0;
                for (ValidateParameter validateParameter = this._parameterList; validateParameter != null; validateParameter = validateParameter.Next)
                {
                    typeSchemaArray[index] = validateParameter.FoundParameterType;
                    ++index;
                }
            }
            return typeSchemaArray;
        }

        public void SetAsIndexAssignment() => this._isIndexAssignment = true;

        public SchemaType FoundMemberType => this._foundMemberType;

        public int FoundMemberIndex => this._foundMemberIndex;

        public object FoundCanonicalInstance => this._foundCanonicalInstance;

        public bool IsIndexAssignment => this._isIndexAssignment;
    }
}
