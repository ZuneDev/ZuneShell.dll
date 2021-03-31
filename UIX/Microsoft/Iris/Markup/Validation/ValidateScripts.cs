// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateScripts
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateScripts
    {
        private ValidateProperty _scriptsProperty;
        private bool _hasErrors;

        public ValidateScripts(ValidateProperty scriptsProperty) => this._scriptsProperty = scriptsProperty;

        public void Validate(ValidateContext context)
        {
            if (this._scriptsProperty.Value != null && !this._scriptsProperty.IsCodeValue)
            {
                this._scriptsProperty.ReportError("Expecting <Script> block");
                this.MarkHasErrors();
            }
            else
            {
                ValidateCode next = (ValidateCode)this._scriptsProperty.Value;
                int num = 0;
                while (next != null)
                {
                    next.Validate(new TypeRestriction((TypeSchema)VoidSchema.Type), context);
                    if (!next.HasErrors)
                    {
                        context.RegisterAction(next);
                        next.MarkAsNotEmbedded();
                        foreach (ValidateExpression triggerExpression in next.DeclaredTriggerExpressions)
                            context.RegisterTrigger(triggerExpression, next);
                    }
                    next = next.Next;
                    ++num;
                }
            }
        }

        public bool HasErrors => this._hasErrors;

        private void MarkHasErrors() => this._hasErrors = true;
    }
}
