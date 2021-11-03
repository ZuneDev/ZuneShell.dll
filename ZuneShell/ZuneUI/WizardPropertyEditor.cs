// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardPropertyEditor
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    public abstract class WizardPropertyEditor : MetadataEditMedia
    {
        private WizardPropertyEditorPage _page;

        protected WizardPropertyEditor() => this._source = WizardPropertySource.Instance;

        public abstract PropertyDescriptor[] PropertyDescriptors { get; }

        public WizardPropertyEditorPage Page => this._page;

        internal void Initialize(WizardPropertyEditorPage page)
        {
            this._page = page;
            this.Initialize(new object[1]
            {
         page
            }, this.PropertyDescriptors);
        }
    }
}
