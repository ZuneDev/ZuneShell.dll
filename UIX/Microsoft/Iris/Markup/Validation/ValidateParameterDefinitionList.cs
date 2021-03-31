// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.Validation.ValidateParameterDefinitionList
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Markup.Validation
{
    internal class ValidateParameterDefinitionList : Microsoft.Iris.Markup.Validation.Validate
    {
        private ArrayList _paramDefinitionList;

        public ValidateParameterDefinitionList(SourceMarkupLoader owner, int line, int column)
          : base(owner, line, column)
          => this._paramDefinitionList = new ArrayList();

        public void AppendToEnd(ValidateParameterDefinition expression) => this._paramDefinitionList.Add((object)expression);

        public ArrayList Parameters => this._paramDefinitionList;

        public void Validate(ValidateContext context)
        {
            foreach (ValidateParameterDefinition paramDefinition in this._paramDefinitionList)
            {
                paramDefinition.Validate((ValidateCode)null, context);
                if (paramDefinition.HasErrors)
                    this.MarkHasErrors();
            }
        }
    }
}
