// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateCode
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateCode : ValidateObject
    {
        private ValidateStatementCompound _statementCompound;
        private Vector<ValidateStatementReturn> _returnStatements = new Vector<ValidateStatementReturn>();
        private Vector<ValidateExpression> _declaredTriggerExpressions;
        private TypeSchema _returnType;
        private uint _encodeStartOffset;
        private bool _embedded;
        private bool _allowTriggers = true;
        private bool _hasDeclaredTriggerStatements;
        private bool _hasInitialEvaluateStatement;
        private bool _initialEvaluate;
        private bool _finalEvaluate;

        public ValidateCode(
          SourceMarkupLoader owner,
          ValidateStatementCompound statementCompound,
          int line,
          int offset)
          : base(owner, line, offset, ObjectSourceType.Code)
        {
            this._statementCompound = statementCompound;
            this._embedded = true;
        }

        public ValidateStatementCompound StatementCompound => this._statementCompound;

        public bool Embedded => this._embedded;

        public override TypeSchema ObjectType => this._returnType;

        public override void Validate(TypeRestriction typeRestriction, ValidateContext context)
        {
            if (this._allowTriggers)
                context.StartDeclaredTriggerTracking();
            this._statementCompound.Validate(this, context);
            if (this._allowTriggers)
                this._declaredTriggerExpressions = context.StopDeclaredTriggerTracking();
            if (this._statementCompound.HasErrors)
                this.MarkHasErrors();
            if (this._returnStatements.Count > 0)
            {
                for (int index = 0; index < this._returnStatements.Count; ++index)
                {
                    ValidateStatementReturn returnStatement = this._returnStatements[index];
                    if (returnStatement.HasErrors)
                        return;
                    TypeSchema type = (TypeSchema)VoidSchema.Type;
                    ValidateExpression expression = returnStatement.Expression;
                    if (expression != null)
                        type = expression.ObjectType;
                    this.ValidateReturnType(type);
                    if (this.HasErrors)
                        return;
                }
            }
            else
                this._returnType = (TypeSchema)VoidSchema.Type;
            string errorMessage = "'{0}' cannot be used in this context (expecting types compatible with '{1}')";
            if (typeRestriction.Primary == VoidSchema.Type && typeRestriction.Secondary == null)
                errorMessage = "Return values are not supported for this code block (currently returning '{0}')";
            else if (this._returnStatements.Count == 0)
                errorMessage = "Code block must have at least one return statement of type '{0}'";
            if (!typeRestriction.Check((ValidateObject)this, errorMessage, this._returnType))
                return;
            ValidateStatementReturn validateStatementReturn = (ValidateStatementReturn)null;
            ValidateStatement validateStatement = this._statementCompound.StatementList;
            if (validateStatement != null)
            {
                while (validateStatement.Next != null)
                    validateStatement = validateStatement.Next;
                if (validateStatement.StatementType == StatementType.Return)
                {
                    validateStatementReturn = (ValidateStatementReturn)validateStatement;
                    validateStatementReturn.MarkAsTrailingReturn();
                }
            }
            if (this._returnType == VoidSchema.Type || validateStatementReturn != null)
                return;
            this.ReportError("Code block must end with a return of type '{0}'", this._returnType.Name);
        }

        private void ValidateReturnType(TypeSchema type)
        {
            if (this._returnType == null)
            {
                this._returnType = type;
            }
            else
            {
                if (this._returnType == type)
                    return;
                if (type.IsAssignableFrom(this._returnType))
                {
                    this._returnType = type;
                }
                else
                {
                    if (this._returnType.IsAssignableFrom(type))
                        return;
                    this.ReportError("Return type cannot be determined due to inconsistant return type statements (mismatched types are: '{0}' and '{1}')", this._returnType.Name, type.Name);
                }
            }
        }

        public void MarkAsNotEmbedded() => this._embedded = false;

        public void DisallowTriggers() => this._allowTriggers = false;

        public uint EncodeStartOffset => this._encodeStartOffset;

        public Vector<ValidateStatementReturn> ReturnStatements => this._returnStatements;

        public Vector<ValidateExpression> DeclaredTriggerExpressions => this._declaredTriggerExpressions;

        public bool InitialEvaluate
        {
            get
            {
                if (this._hasInitialEvaluateStatement)
                    return this._initialEvaluate;
                return !this._hasDeclaredTriggerStatements && !this._finalEvaluate && !this._embedded;
            }
        }

        public bool FinalEvaluate => this._finalEvaluate;

        public void TrackReturnStatement(ValidateStatementReturn returnStatement) => this._returnStatements.Add(returnStatement);

        public void TrackEncodingOffset(uint startOffset) => this._encodeStartOffset = startOffset;

        public void MarkInitialEvaluate(bool initialEvaluate)
        {
            this._hasInitialEvaluateStatement = true;
            this._initialEvaluate = initialEvaluate;
        }

        public void MarkFinalEvaluate(bool finalEvaluate) => this._finalEvaluate = finalEvaluate;

        public void MarkDeclaredTriggerStatements() => this._hasDeclaredTriggerStatements = true;

        public ValidateCode Next
        {
            get => (ValidateCode)base.Next;
            set => base.Next = (ValidateObject)value;
        }
    }
}
