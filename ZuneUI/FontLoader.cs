// Decompiled with JetBrains decompiler
// Type: ZuneUI.FontLoader
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune;
using System;
using System.Collections.Generic;

namespace ZuneUI
{
    public class FontLoader
    {
        private List<string> _fonts;
        private string _resourceDll;
        private bool _loaded;
        private bool _loadQueued;

        public List<string> Fonts
        {
            get => this._fonts;
            set
            {
                this._fonts = value;
                this.QueueFontLoading();
            }
        }

        public string Resource
        {
            set
            {
                this._resourceDll = value;
                this.QueueFontLoading();
            }
        }

        private void QueueFontLoading()
        {
            if (this._loaded)
                throw new InvalidOperationException("This is a one trick pony, sorry.");
            if (this._loadQueued)
                return;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.LoadFonts), (object)null);
            this._loadQueued = true;
        }

        private void LoadFonts(object args)
        {
            this._loadQueued = false;
            if (this._fonts == null || this._fonts.Count == 0)
                return;
            if (this._resourceDll == null)
                throw new InvalidOperationException("Must specify a Resource to retrieve the fonts from.");
            foreach (string font in this._fonts)
                MemoryFonts.TryLoadFromResource(this._resourceDll, font);
            this._loaded = true;
        }
    }
}
