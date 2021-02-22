// Decompiled with JetBrains decompiler
// Type: ZuneUI.MappedError
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;

namespace ZuneUI
{
    public struct MappedError
    {
        private string _text;
        private string _url;

        public MappedError(int hr)
        {
            ErrorMapperResult descriptionAndUrl = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(hr);
            this._text = descriptionAndUrl.Description;
            this._url = descriptionAndUrl.WebHelpUrl;
        }

        public string Text => this._text;

        public string URL => this._url;
    }
}
