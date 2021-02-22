// Decompiled with JetBrains decompiler
// Type: ZuneUI.ErrorDialogInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.ErrorMapperApi;
using UIXControls;

namespace ZuneUI
{
    public class ErrorDialogInfo : DialogHelper
    {
        private int _hr;
        private int _hrOriginal;
        private string _title;
        private string _description;
        private string _webHelpUrl;
        private eErrorCondition _condition;

        internal static void Show(int hr, string title) => ErrorDialogInfo.Show(hr, eErrorCondition.eEC_None, title, (string)null);

        internal static void Show(int hr, eErrorCondition condition, string title) => ErrorDialogInfo.Show(hr, condition, title, (string)null);

        internal static void Show(int hr, string title, string description) => ErrorDialogInfo.Show(hr, eErrorCondition.eEC_None, title, description);

        internal static void Show(int hr, eErrorCondition condition, string title, string description) => new ErrorDialogInfo(hr, condition, title, description).Show();

        private ErrorDialogInfo(int hr, eErrorCondition condition, string title, string description)
          : base("res://ZuneShellResources!ErrorDialog.uix#ErrorDialogContentUI")
        {
            this._title = title;
            this._hrOriginal = hr;
            this._condition = condition;
            ErrorMapperResult descriptionAndUrl = Microsoft.Zune.ErrorMapperApi.ErrorMapperApi.GetMappedErrorDescriptionAndUrl(this._hrOriginal, this._condition);
            this._hr = descriptionAndUrl.Hr;
            this._description = description ?? descriptionAndUrl.Description;
            this._webHelpUrl = descriptionAndUrl.WebHelpUrl;
        }

        public int HR => this._hr;

        public int OriginalHR => this._hrOriginal;

        public eErrorCondition ErrorCondition => this._condition;

        public string Title => this._title;

        public new string Description => this._description;

        public string WebHelpUrl => this._webHelpUrl;
    }
}
