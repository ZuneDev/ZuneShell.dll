// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementReturn
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementReturn : ValidateStatement
    {
        private ValidateExpression _expression;
        private Vector<int> _scopedLocalsToClear;
        private uint _jumpFixupOffset;
        private bool _isTrailingReturn;

        public ValidateStatementReturn(
          SourceMarkupLoader owner,
          ValidateExpression expression,
          int line,
          int column)
          : base(owner, line, column, StatementType.Return)
        {
            this._expression = expression;
        }

        public ValidateExpression Expression => this._expression;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            if (this._expression != null)
            {
                this._expression.Validate(new TypeRestriction(ObjectSchema.Type), context);
                if (this._expression.HasErrors)
                    this.MarkHasErrors();
            }
            container.TrackReturnStatement(this);
            this._scopedLocalsToClear = context.GetImmediateFrameUnwindList(this.Owner);
        }

        public Vector<int> ScopedLocalsToClear => this._scopedLocalsToClear;

        public uint JumpFixupOffset => this._jumpFixupOffset;

        public void TrackJumpFixupOffset(uint jumpFixupOffset) => this._jumpFixupOffset = jumpFixupOffset;

        public bool IsTrailingReturn => this._isTrailingReturn;

        public void MarkAsTrailingReturn() => this._isTrailingReturn = true;
    }
}
