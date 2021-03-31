// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementAttribute
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementAttribute : ValidateStatement
    {
        private const string DeclareTriggerAttributeName = "DeclareTrigger";
        private const string InitialEvaluateAttributeName = "InitialEvaluate";
        private const string FinalEvaluateAttributeName = "FinalEvaluate";
        private string _attributeName;
        private ValidateParameter _parameterList;

        public ValidateStatementAttribute(
          SourceMarkupLoader owner,
          string attributeName,
          ValidateParameter parameterList,
          int line,
          int column)
          : base(owner, line, column, StatementType.Attribute)
        {
            if (parameterList == ValidateParameter.EmptyList)
                parameterList = null;
            this._attributeName = attributeName;
            this._parameterList = parameterList;
        }

        public string AttributeName => this._attributeName;

        public ValidateParameter ParameterList => this._parameterList;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            if (this._attributeName == "DeclareTrigger")
                this.ValidateDeclareTrigger(container, context);
            else if (this._attributeName == "InitialEvaluate")
                this.ValidateInitialOrFinalEvaluate(container, context, true);
            else if (this._attributeName == "FinalEvaluate")
                this.ValidateInitialOrFinalEvaluate(container, context, false);
            else
                this.ReportError("Script attribute '{0}' is unknown", this._attributeName);
        }

        private void ValidateDeclareTrigger(ValidateCode container, ValidateContext context)
        {
            ValidateParameter parameterList = this._parameterList;
            if (this._parameterList == null || parameterList.Next != null)
            {
                this.ReportError("Script attribute '{0}' invalid number of parameters (expecting: {1})", this._attributeName, "1");
            }
            else
            {
                ValidateExpressionDeclareTrigger.StartNotifierTracking(context, parameterList.Expression);
                parameterList.Expression.MakeDeclareTriggerUsage();
                parameterList.Validate(context, true);
                ValidateExpressionDeclareTrigger.StopNotifierTracking(this, context, parameterList.Expression);
                if (parameterList.HasErrors)
                    this.MarkHasErrors();
                else if (context.IsTrackingDeclaredTriggers)
                {
                    ValidateExpression expression = this._parameterList.Expression;
                    context.TrackDeclaredTrigger(expression);
                    container.MarkDeclaredTriggerStatements();
                }
                else
                    this.ReportError("Expressions can only be used as triggers if they exist within Script blocks");
            }
        }

        private void ValidateInitialOrFinalEvaluate(
          ValidateCode container,
          ValidateContext context,
          bool isInitialEvaluate)
        {
            if (this._parameterList == null || this._parameterList.Next != null)
            {
                this.ReportError("Script attribute '{0}' invalid number of parameters (expecting: {1})", this._attributeName, "1");
            }
            else
            {
                this._parameterList.Validate(context, false);
                if (!(this._parameterList.Expression is ValidateExpressionConstant expression) || expression.ConstantType != ConstantType.Boolean)
                {
                    this.ReportError("Script attribute parameter must be Boolean 'true' or 'false'");
                }
                else
                {
                    bool foundConstant = (bool)expression.FoundConstant;
                    if (isInitialEvaluate)
                        container.MarkInitialEvaluate(foundConstant);
                    else
                        container.MarkFinalEvaluate(foundConstant);
                }
            }
        }
    }
}
