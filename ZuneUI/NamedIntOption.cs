// Decompiled with JetBrains decompiler
// Type: ZuneUI.NamedIntOption
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System;

namespace ZuneUI
{
    internal class NamedIntOption : Command
    {
        private static NamedIntOption[] _keepOptions;
        private static NamedIntOption[] _playbackOptions;
        private static NamedIntOption[] _syncOptions;
        private static NamedIntOption[] _screenGraphicsOptions;
        private int _value;

        public NamedIntOption(IModelItemOwner owner, string description, int value)
          : base(owner, description, (EventHandler)null)
          => this._value = value;

        public int Value => this._value;

        public static void SelectOptionByValue(Choice choice, int value)
        {
            for (int index = 0; index < choice.Options.Count; ++index)
            {
                if (((NamedIntOption)choice.Options[index]).Value == value)
                {
                    choice.ChosenIndex = index;
                    break;
                }
            }
        }

        public static NamedIntOption[] PodcastKeepOptions
        {
            get
            {
                if (NamedIntOption._keepOptions == null)
                {
                    string format = Shell.LoadString(StringId.IDS_KEEP_N_OPTION);
                    NamedIntOption._keepOptions = new NamedIntOption[12]
                    {
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_KEEP_0_OPTION), 0),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_KEEP_1_OPTION), 1),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 2), 2),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 3), 3),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 4), 4),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 5), 5),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 6), 6),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 7), 7),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 8), 8),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 9), 9),
            new NamedIntOption((IModelItemOwner) null, string.Format(format, (object) 10), 10),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_KEEP_ALL_OPTION), -1)
                    };
                }
                return NamedIntOption._keepOptions;
            }
        }

        public static NamedIntOption[] PodcastPlaybackOptions
        {
            get
            {
                if (NamedIntOption._playbackOptions == null)
                    NamedIntOption._playbackOptions = new NamedIntOption[2]
                    {
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_NEWEST_FIRST_OPTION), 0),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_OLDEST_FIRST_OPTION), 1)
                    };
                return NamedIntOption._playbackOptions;
            }
        }

        public static NamedIntOption[] PodcastSyncOptions
        {
            get
            {
                if (NamedIntOption._syncOptions == null)
                    NamedIntOption._syncOptions = new NamedIntOption[4]
                    {
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SYNC_UNPLAYED_OPTION), 2),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SYNC_ALL_DOWNLOADED_OPTION), 0),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SYNC_FIRST_UNPLAYED_OPTION), 3),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SYNC_NONE_OPTION), 4)
                    };
                return NamedIntOption._syncOptions;
            }
        }

        public static NamedIntOption[] ScreenGraphicsOptions
        {
            get
            {
                if (NamedIntOption._screenGraphicsOptions == null)
                    NamedIntOption._screenGraphicsOptions = new NamedIntOption[4]
                    {
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_BASIC), 0),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_ADVANCED), 1),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_ADVANCED_ANIMATION), 2),
            new NamedIntOption((IModelItemOwner) null, Shell.LoadString(StringId.IDS_SCREEN_GRAPHICS_PREMIUM), 3)
                    };
                return NamedIntOption._screenGraphicsOptions;
            }
        }
    }
}
