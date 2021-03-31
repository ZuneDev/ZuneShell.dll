// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementBreak
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementBreak : ValidateStatement
    {
        private ValidateStatementLoop _loopStatement;
        private Vector<int> _scopedLocalsToClear;
        private uint _jumpFixupOffset;
        private bool _isContinue;

        public ValidateStatementBreak(SourceMarkupLoader owner, bool isContinue, int line, int column)
          : base(owner, line, column, StatementType.Break)
          => this._isContinue = isContinue;

        protected override void OnDispose()
        {
            this._loopStatement = null;
            base.OnDispose();
        }

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            this._loopStatement = context.EnclosingLoop;
            if (this._loopStatement == null)
            {
                this.ReportError("No enclosing loop out of which to break or continue");
            }
            else
            {
                this._loopStatement.TrackBreakStatement(this);
                this._scopedLocalsToClear = context.GetLoopUnwindList(this.Owner);
            }
        }

        public bool IsContinue => this._isContinue;

        public Vector<int> ScopedLocalsToClear => this._scopedLocalsToClear;

        public uint JumpFixupOffset => this._jumpFixupOffset;

        public void TrackJumpFixupOffset(uint jumpFixupOffset) => this._jumpFixupOffset = jumpFixupOffset;
    }
}
