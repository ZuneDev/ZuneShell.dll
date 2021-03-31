// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpressionNew
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateExpressionNew : ValidateExpression
    {
        private ValidateTypeIdentifier _constructType;
        private ValidateParameter _parameterList;
        private bool _isParameterizedConstruction;
        private TypeSchema _foundConstructType;
        private int _foundConstructTypeIndex = -1;
        private ConstructorSchema _foundParameterizedConstructor;
        private int _foundParameterizedConstructorIndex = -1;

        public ValidateExpressionNew(
          SourceMarkupLoader owner,
          ValidateTypeIdentifier constructType,
          ValidateParameter parameterList,
          int line,
          int column)
          : base(owner, line, column, ExpressionType.New)
        {
            if (parameterList == ValidateParameter.EmptyList)
                parameterList = null;
            this._constructType = constructType;
            this._parameterList = parameterList;
        }

        public ValidateTypeIdentifier ConstructType => this._constructType;

        public ValidateParameter ParameterList => this._parameterList;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            this._constructType.Validate();
            if (this._constructType.HasErrors)
            {
                this.MarkHasErrors();
            }
            else
            {
                this._foundConstructType = this._constructType.FoundType;
                this._foundConstructTypeIndex = this._constructType.FoundTypeIndex;
                this.DeclareEvaluationType(this._foundConstructType, typeRestriction);
                if (this.Usage == ExpressionUsage.LValue)
                    this.ReportError("Expression cannot be used as the target an assignment (related symbol: '{0}')", this._foundConstructType.Name);
                int length = 0;
                for (ValidateParameter validateParameter = this._parameterList; validateParameter != null; validateParameter = validateParameter.Next)
                {
                    validateParameter.Validate(context);
                    if (validateParameter.HasErrors)
                        this.MarkHasErrors();
                    ++length;
                }
                if (this.HasErrors)
                    return;
                if (length == 0)
                {
                    if (this._foundConstructType.HasDefaultConstructor)
                        return;
                    this.ReportError("A matching constructor could not be found on '{0}'", this._foundConstructType.Name);
                }
                else
                {
                    TypeSchema[] parameters = new TypeSchema[length];
                    int index = 0;
                    for (ValidateParameter validateParameter = this._parameterList; validateParameter != null; validateParameter = validateParameter.Next)
                    {
                        parameters[index] = validateParameter.FoundParameterType;
                        ++index;
                    }
                    this._foundParameterizedConstructor = this._foundConstructType.FindConstructor(parameters);
                    if (this._foundParameterizedConstructor != null)
                    {
                        this._isParameterizedConstruction = true;
                        this._foundParameterizedConstructorIndex = this.Owner.TrackImportedConstructor(this._foundParameterizedConstructor);
                    }
                    else
                    {
                        string empty = string.Empty;
                        bool flag = true;
                        foreach (TypeSchema typeSchema in parameters)
                        {
                            if (!flag)
                                empty += ", ";
                            empty += typeSchema.Name;
                            flag = false;
                        }
                        this.ReportError("A matching constructor could not be found on '{0}' that accepts parameters '{1}'", this._foundConstructType.Name, empty);
                    }
                }
            }
        }

        public bool IsParameterizedConstruction => this._isParameterizedConstruction;

        public TypeSchema FoundConstructType => this._foundConstructType;

        public int FoundConstructTypeIndex => this._foundConstructTypeIndex;

        public ConstructorSchema FoundParameterizedConstructor => this._foundParameterizedConstructor;

        public int FoundParameterizedConstructorIndex => this._foundParameterizedConstructorIndex;
    }
}
