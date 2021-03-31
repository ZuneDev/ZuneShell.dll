// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateExpression
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

namespace Microsoft.Iris.Markup.Validation
{
    internal abstract class ValidateExpression : ValidateObject
    {
        private TypeSchema _evaluationType;
        private ExpressionType _expressionType;
        private ExpressionUsage _usage;
        private int _notifyIndex = -1;
        private bool _isNotifierRoot;
        private uint _encodeStartOffset;

        public ValidateExpression(
          SourceMarkupLoader owner,
          int line,
          int column,
          ExpressionType expressionType)
          : base(owner, line, column, ObjectSourceType.Expression)
        {
            this._expressionType = expressionType;
            this._usage = ExpressionUsage.RValue;
        }

        public override TypeSchema ObjectType => this._evaluationType;

        public ExpressionType ExpressionType => this._expressionType;

        public void MakeLValueUsage() => this._usage = ExpressionUsage.LValue;

        public void MakeDeclareTriggerUsage() => this._usage = ExpressionUsage.DeclareTrigger;

        public uint EncodeStartOffset => this._encodeStartOffset;

        protected void DeclareEvaluationType(TypeSchema evaluationType, TypeRestriction typeRestriction)
        {
            if (!typeRestriction.Check(this, evaluationType))
                return;
            this._evaluationType = evaluationType;
        }

        public ExpressionUsage Usage => this._usage;

        protected void DeclareNotifies(ValidateContext context) => this._notifyIndex = context.TrackDeclareNotifies(this);

        public int NotifyIndex => this._notifyIndex;

        public bool IsNotifierRoot => this._isNotifierRoot;

        public void MarkNotifierRoot() => this._isNotifierRoot = true;

        public void TrackEncodingOffset(uint startOffset) => this._encodeStartOffset = startOffset;
    }
}
