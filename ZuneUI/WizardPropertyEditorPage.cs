// Decompiled with JetBrains decompiler
// Type: ZuneUI.WizardPropertyEditorPage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections.Generic;

namespace ZuneUI
{
    public abstract class WizardPropertyEditorPage : WizardPage
    {
        private Dictionary<PropertyDescriptor, object> _committedValues;
        private WizardPropertyEditor _wizardPropertyEditor;

        protected WizardPropertyEditorPage(Wizard owner)
          : base(owner)
        {
        }

        public WizardPropertyEditor WizardPropertyEditor => this._wizardPropertyEditor;

        public override bool IsValid => this._wizardPropertyEditor == null || this._wizardPropertyEditor.IsValid();

        protected void Initialize(WizardPropertyEditor wizardPropertyEditor)
        {
            this._wizardPropertyEditor = wizardPropertyEditor;
            this._committedValues = new Dictionary<PropertyDescriptor, object>();
            if (this._wizardPropertyEditor == null)
                return;
            foreach (PropertyDescriptor propertyDescriptor in this._wizardPropertyEditor.PropertyDescriptors)
                this.AddProperty(propertyDescriptor);
            this._wizardPropertyEditor.Initialize(this);
        }

        private void AddProperty(PropertyDescriptor property)
        {
            if (this._committedValues.ContainsKey(property))
                return;
            this._committedValues.Add(property, property.DefaultValue);
        }

        public object GetCommittedValue(PropertyDescriptor property) => this._committedValues.ContainsKey(property) ? this._committedValues[property] : (object)null;

        public void SetCommittedValue(PropertyDescriptor property, object value)
        {
            if (this._committedValues.ContainsKey(property))
                this._committedValues[property] = value;
            else
                this._committedValues.Add(property, value);
            this.SetUncommittedValue(property, value);
        }

        internal void SetPropertyState(PropertyDescriptor property, object state)
        {
            if (this._wizardPropertyEditor == null)
                return;
            this._wizardPropertyEditor.SetPropertyState(property, state);
        }

        public override void RefreshValidationState()
        {
            if (this._wizardPropertyEditor != null)
                this._wizardPropertyEditor.ResetExternalErrors();
            base.RefreshValidationState();
        }

        protected object GetUncommittedValue(PropertyDescriptor property)
        {
            object obj = (object)null;
            if (this._wizardPropertyEditor != null)
                obj = this._wizardPropertyEditor.GetPropertyData(property);
            return obj;
        }

        protected void SetUncommittedValue(PropertyDescriptor property, object value)
        {
            if (this._wizardPropertyEditor == null)
                return;
            this._wizardPropertyEditor.SetPropertyData(property, value);
        }

        protected bool SetExternalError(PropertyDescriptor descriptor, HRESULT hr)
        {
            bool flag = false;
            MetadataEditProperty metadataEditProperty = (MetadataEditProperty)null;
            if (this._wizardPropertyEditor != null)
                metadataEditProperty = this._wizardPropertyEditor.GetProperty(descriptor);
            if (metadataEditProperty != null)
            {
                flag = true;
                metadataEditProperty.ExternalError = hr;
            }
            return flag;
        }

        protected bool SetExternalError(string propertyName, HRESULT hr)
        {
            bool flag = false;
            MetadataEditProperty metadataEditProperty = (MetadataEditProperty)null;
            if (this._wizardPropertyEditor != null)
                metadataEditProperty = this._wizardPropertyEditor.GetProperty(propertyName);
            if (metadataEditProperty != null)
            {
                flag = true;
                metadataEditProperty.ExternalError = hr;
            }
            return flag;
        }

        internal override bool OnMovingNext()
        {
            if (this._wizardPropertyEditor != null && this._wizardPropertyEditor.IsValid())
                this._wizardPropertyEditor.Commit();
            return base.OnMovingNext();
        }
    }
}
