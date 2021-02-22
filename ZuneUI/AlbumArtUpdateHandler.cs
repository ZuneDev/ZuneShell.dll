// Decompiled with JetBrains decompiler
// Type: ZuneUI.AlbumArtUpdateHandler
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections.Specialized;
using UIXControls;

namespace ZuneUI
{
    public class AlbumArtUpdateHandler : ModelItem
    {
        private const string _supportedExtensionsFilter = "*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.dib";
        private object _album;
        private string _path;
        private bool _done;
        private Image _image;
        private BooleanChoice _neverShowPasteMessage;
        private static string[] _supportedExtensions = new string[6]
        {
      ".bmp",
      ".jpg",
      ".jpeg",
      ".gif",
      ".png",
      ".dib"
        };

        public AlbumArtUpdateHandler(object album) => this._album = album;

        protected override void OnDispose(bool disposing)
        {
            if (this._neverShowPasteMessage != null)
            {
                this._neverShowPasteMessage.Dispose();
                this._neverShowPasteMessage = (BooleanChoice)null;
            }
            base.OnDispose(disposing);
        }

        public object Album => this._album;

        public string Path
        {
            get => this._path;
            private set
            {
                if (!(value != this._path))
                    return;
                this._path = value;
                this.FirePropertyChanged(nameof(Path));
                this.Image = (Image)null;
            }
        }

        public bool Done
        {
            get => this._done;
            private set
            {
                if (value == this._done)
                    return;
                this._done = value;
                this.FirePropertyChanged(nameof(Done));
            }
        }

        public Image Image
        {
            get
            {
                if (this._image == null && !string.IsNullOrEmpty(this._path))
                    this._image = new Image("file://" + this._path);
                return this._image;
            }
            private set
            {
                if (value == this._image)
                    return;
                this._image = value;
                this.FirePropertyChanged(nameof(Image));
            }
        }

        public void Start() => this.Start(true);

        public void Start(bool allowPaste)
        {
            this.Done = false;
            this.Path = (string)null;
            if (allowPaste && AlbumArtUpdateHandler.CanPaste())
            {
                if (ZuneShell.DefaultInstance.Management.ConfirmPasteAlbumArt)
                {
                    if (this._neverShowPasteMessage == null)
                        this._neverShowPasteMessage = new BooleanChoice((IModelItemOwner)this, Shell.LoadString(StringId.IDS_DONT_SHOW_THIS_MESSAGE_AGAIN));
                    string str = (string)null;
                    if (this.Album is DataProviderObject album)
                        str = album.GetProperty("Title") as string;
                    string message = !string.IsNullOrEmpty(str) ? string.Format(Shell.LoadString(StringId.IDS_CONFIRM_PASTE_ALBUM_ART), (object)str) : Shell.LoadString(StringId.IDS_CONFIRM_PASTE_ALBUM_ART_NO_NAME);
                    this._neverShowPasteMessage.Value = false;
                    MessageBox.Show(Shell.LoadString(StringId.IDS_CONFIRM_PASTE_ALBUM_ART_TITLE), message, (EventHandler)null, new EventHandler(this.ConfirmPaste), (EventHandler)null, (EventHandler)null, this._neverShowPasteMessage);
                }
                else
                    this.ConfirmPaste((object)this, (EventArgs)null);
            }
            else
                this.BrowseForArt();
        }

        public static bool CanPaste()
        {
            bool flag = Clipboard.ContainsData(ClipboardDataType.Image);
            if (!flag && Clipboard.ContainsData(ClipboardDataType.FileDropList))
                flag = AlbumArtUpdateHandler.GetClipboardFile() != null;
            return flag;
        }

        private void BrowseForArt() => FileOpenDialog.Show(Shell.LoadString(StringId.IDS_CHOOSE_ALBUM_ART), FileOpenDialog.MyPicturesPath, new string[2]
        {
      Shell.LoadString(StringId.IDS_ALBUM_ART_DIALOG_ALL_PICTURES),
      "*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.dib"
        }, (DeferredInvokeHandler)(args =>
   {
            this.Path = (string)args;
            this.Done = true;
        }));

        private void ConfirmPaste(object sender, EventArgs args)
        {
            if (this._neverShowPasteMessage != null)
                ZuneShell.DefaultInstance.Management.ConfirmPasteAlbumArt = !this._neverShowPasteMessage.Value;
            if (this.Paste())
                this.Done = true;
            else
                this.BrowseForArt();
        }

        private bool Paste()
        {
            bool flag = false;
            if (this._album is LibraryDataProviderListItem)
            {
                LibraryDataProviderListItem album = (LibraryDataProviderListItem)this._album;
                SafeBitmap image = Clipboard.GetImage();
                if (image != null)
                    flag = album.SetNewThumbnail(image);
                if (!flag)
                {
                    string clipboardFile = AlbumArtUpdateHandler.GetClipboardFile();
                    if (clipboardFile != null)
                    {
                        this.Path = clipboardFile;
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private static string GetClipboardFile()
        {
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList != null)
            {
                foreach (string str in fileDropList)
                {
                    foreach (string supportedExtension in AlbumArtUpdateHandler._supportedExtensions)
                    {
                        if (str.EndsWith(supportedExtension, StringComparison.InvariantCultureIgnoreCase))
                            return str;
                    }
                }
            }
            return (string)null;
        }
    }
}
