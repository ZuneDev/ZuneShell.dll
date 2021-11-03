// Decompiled with JetBrains decompiler
// Type: ZuneXml.InboxImageDataProviderObject
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneXml
{
    internal class InboxImageDataProviderObject : DataProviderObject
    {
        public static readonly string PropertyName_InLibrary = nameof(InLibrary);
        public static readonly string PropertyName_ImagePath = nameof(ImagePath);
        public static readonly string PropertyName_Photo = nameof(Photo);
        private bool _inLibrary;
        private string _imagePath;
        private Image _photo;

        public InboxImageDataProviderObject(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        public override object GetProperty(string propertyName)
        {
            if (propertyName == PropertyName_InLibrary)
                return InLibrary;
            if (propertyName == PropertyName_ImagePath)
                return ImagePath;
            return propertyName == PropertyName_Photo ? Photo : null;
        }

        public bool InLibrary
        {
            get => this._inLibrary;
            set
            {
                if (this._inLibrary == value)
                    return;
                this._inLibrary = value;
                this.FirePropertyChanged(PropertyName_InLibrary);
            }
        }

        public string ImagePath
        {
            get => this._imagePath;
            set
            {
                if (!(this._imagePath != value))
                    return;
                this._imagePath = value;
                this.FirePropertyChanged(PropertyName_ImagePath);
            }
        }

        public Image Photo
        {
            get
            {
                if (this._photo == null && !string.IsNullOrEmpty(this._imagePath))
                    this._photo = new Image("file://" + this._imagePath);
                return this._photo;
            }
        }

        public override void SetProperty(string propertyName, object value)
        {
            if (propertyName == PropertyName_InLibrary)
            {
                this.InLibrary = (bool)value;
            }
            else
            {
                if (!(propertyName == PropertyName_ImagePath))
                    throw new ApplicationException("unexpected property name");
                this.ImagePath = (string)value;
            }
        }

        internal void TransferToAppThread()
        {
        }
    }
}
