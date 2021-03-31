// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateStatementForEach
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup.UIX;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateStatementForEach : ValidateStatementLoop
    {
        private ValidateStatementScopedLocal _scopedLocal;
        private ValidateExpression _expression;
        private ValidateStatementCompound _statementCompound;
        private Vector<int> _scopedLocalsToClear;
        private int _foundGetEnumeratorIndex = -1;
        private int _foundCurrentIndex = -1;
        private int _foundMoveNextIndex = -1;
        private static PropertySchema s_currentProperty;
        private static MethodSchema s_moveNextMethod;

        public ValidateStatementForEach(
          SourceMarkupLoader owner,
          ValidateStatementScopedLocal scopedLocal,
          ValidateExpression expression,
          ValidateStatementCompound statementCompound,
          int line,
          int column)
          : base(owner, line, column, StatementType.ForEach)
        {
            this._scopedLocal = scopedLocal;
            this._expression = expression;
            this._statementCompound = statementCompound;
        }

        public static void InitializeStatics()
        {
            ValidateStatementForEach.s_currentProperty = EnumeratorSchema.Type.FindProperty("Current");
            ValidateStatementForEach.s_moveNextMethod = EnumeratorSchema.Type.FindMethod("MoveNext", TypeSchema.EmptyList);
        }

        public ValidateStatementScopedLocal ScopedLocal => this._scopedLocal;

        public ValidateExpression Expression => this._expression;

        public ValidateStatementCompound StatementCompound => this._statementCompound;

        public override void Validate(ValidateCode container, ValidateContext context)
        {
            context.NotifyScopedLocalFrameEnter((ValidateStatementLoop)this);
            try
            {
                this._scopedLocal.Validate(container, context);
                if (this._scopedLocal.HasErrors)
                    this.MarkHasErrors();
                this._scopedLocal.HasInitialAssignment = true;
                this._expression.Validate(new TypeRestriction((TypeSchema)ListSchema.Type), context);
                if (this._expression.HasErrors)
                {
                    this.MarkHasErrors();
                }
                else
                {
                    MethodSchema methodDeep = ListSchema.Type.FindMethodDeep("GetEnumerator", TypeSchema.EmptyList);
                    if (methodDeep == null)
                        this.ReportError("Type '{0}' is not enumerable", this._expression.ObjectType.Name);
                    if (!EnumeratorSchema.Type.IsAssignableFrom(methodDeep.ReturnType))
                        this.ReportError("While searching for an enumerator, a 'GetEnumerator' method was found on '{0}', but, it is not of type 'Enumerator' (it is '{1}')", this._expression.ObjectType.Name, methodDeep.ReturnType.Name);
                    this._foundGetEnumeratorIndex = this.Owner.TrackImportedMethod(methodDeep);
                    this._statementCompound.Validate(container, context);
                    if (this._statementCompound.HasErrors)
                        this.MarkHasErrors();
                    this._foundCurrentIndex = this.Owner.TrackImportedProperty(ValidateStatementForEach.s_currentProperty);
                    this._foundMoveNextIndex = this.Owner.TrackImportedMethod(ValidateStatementForEach.s_moveNextMethod);
                }
            }
            finally
            {
                this._scopedLocalsToClear = context.NotifyScopedLocalFrameExit();
            }
        }

        public int FoundGetEnumeratorIndex => this._foundGetEnumeratorIndex;

        public int FoundCurrentIndex => this._foundCurrentIndex;

        public int FoundMoveNextIndex => this._foundMoveNextIndex;

        public Vector<int> ScopedLocalsToClear => this._scopedLocalsToClear;
    }
}
