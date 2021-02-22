// Decompiled with JetBrains decompiler
// Type: ZuneUI.ThumbnailCommand
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class ThumbnailCommand : Command, IThumbnailCommand, ICommand, INotifyPropertyChanged
    {
        private Image _image;

        public ThumbnailCommand()
          : this((IModelItemOwner)null)
        {
        }

        public ThumbnailCommand(IModelItemOwner owner)
          : base(owner)
        {
        }

        public virtual Image Image
        {
            get => this._image;
            set
            {
                if (this._image == value)
                    return;
                this._image = value;
                this.FirePropertyChanged(nameof(Image));
            }
        }

        public string ImagePath
        {
            set => this.Image = new Image(value);
        }

        IDictionary IThumbnailCommand.Data => this.Data;
    }
}
