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
            get => _fonts;
            set
            {
                _fonts = value;
                QueueFontLoading();
            }
        }

        public string Resource
        {
            set
            {
                _resourceDll = value;
                QueueFontLoading();
            }
        }

        private void QueueFontLoading()
        {
            if (_loaded)
                throw new InvalidOperationException("This is a one trick pony, sorry.");
            if (_loadQueued)
                return;
            
            Application.DeferredInvoke(LoadFonts, null);
            
            _loadQueued = true;
        }

        private void LoadFonts(object args)
        {
            _loadQueued = false;
            if (_fonts == null || _fonts.Count == 0)
                return;
            if (_resourceDll == null)
                throw new InvalidOperationException("Must specify a Resource to retrieve the fonts from.");
            
#if WINDOWS
            foreach (var font in _fonts)
                MemoryFonts.TryLoadFromResource(_resourceDll, font);
#endif
            
            _loaded = true;
        }
    }
}
