// Decompiled with JetBrains decompiler
// Type: ZuneXml.AppCapability
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;

namespace ZuneXml
{
    internal class AppCapability : XmlDataProviderObject
    {
        private DisclosureEnum _disclosureEnum = DisclosureEnum.Invalid;

        public bool NeedDisclosure => this.DisclosureEnum == DisclosureEnum.Disclose || this.DisclosureEnum == DisclosureEnum.DiscloseAndPrompt;

        public bool NeedPrompt => this.DisclosureEnum == DisclosureEnum.Prompt || this.DisclosureEnum == DisclosureEnum.DiscloseAndPrompt;

        internal DisclosureEnum DisclosureEnum
        {
            get => this._disclosureEnum;
            set
            {
                if (this._disclosureEnum == value)
                    return;
                this._disclosureEnum = value;
                this.FirePropertyChanged(nameof(DisclosureEnum));
            }
        }

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "DisclosureType":
                    this.DisclosureEnum = SchemaHelper.ToDisclosureEnum((string)value);
                    break;
            }
            base.SetProperty(propertyName, value);
        }

        internal static XmlDataProviderObject ConstructAppCapabilityObject(
          DataProviderQuery owner,
          object objectTypeCookie)
        {
            return (XmlDataProviderObject)new AppCapability(owner, objectTypeCookie);
        }

        internal AppCapability(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal string Id => (string)base.GetProperty(nameof(Id));

        internal string Description => (string)base.GetProperty(nameof(Description));

        internal string DisclosureType => (string)base.GetProperty(nameof(DisclosureType));

        public override object GetProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "NeedDisclosure":
                    return (object)this.NeedDisclosure;
                case "NeedPrompt":
                    return (object)this.NeedPrompt;
                default:
                    return base.GetProperty(propertyName);
            }
        }
    }
}
