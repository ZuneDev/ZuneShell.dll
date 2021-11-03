// Decompiled with JetBrains decompiler
// Type: ZuneUI.IZunePage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public interface IZunePage : IPage, INotifyPropertyChanged
    {
        string UI { get; set; }

        string BackgroundUI { get; set; }

        string BottomBarUI { get; }

        string OverlayUI { get; set; }

        IDictionary NavigationArguments { get; set; }

        ICommandHandler CommandHandler { get; }

        bool ShowBackArrow { get; }

        bool ShowDeviceIcon { get; }

        bool ShowPlaylistIcon { get; }

        bool ShowCDIcon { get; }

        bool ShowNowPlayingX { get; }

        bool ShowNowPlayingBackgroundOnIdle { get; }

        bool NotificationAreaVisible { get; }

        bool AutoHideToolbars { get; }

        bool ShowAppBackground { get; }

        ComputerIconState ShowComputerIcon { get; }

        bool ShowLogo { get; }

        bool ShowPivots { get; }

        bool ShowSearch { get; }

        bool ShowSettings { get; }

        TransportControlStyle TransportControlStyle { get; }

        PlaybackContext PlaybackContext { get; }

        bool HandleBack();

        bool HandleEscape();

        bool CanNavigateForwardTo(IZunePage destination);
    }
}
