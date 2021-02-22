// Decompiled with JetBrains decompiler
// Type: ZuneUI.MessageTypeInfo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;

namespace ZuneUI
{
    internal class MessageTypeInfo
    {
        private string _UIText;
        private string _detailsTemplate;
        private static Hashtable s_MessageTypes;

        public MessageTypeInfo(StringId uiText, string detailsTemplate)
        {
            this._UIText = Shell.LoadString(uiText);
            this._detailsTemplate = detailsTemplate;
        }

        public string UIText => this._UIText;

        public string DetailsTemplate => this._detailsTemplate;

        public static MessageTypeInfo GetMessageType(string serviceType)
        {
            if (MessageTypeInfo.s_MessageTypes == null)
            {
                MessageTypeInfo.s_MessageTypes = new Hashtable(17);
                MessageTypeInfo.s_MessageTypes.Add((object)"album", (object)new MessageTypeInfo(StringId.IDS_TYPE_ALBUM, "res://ZuneShellResources!InboxTrackDetails.uix#AlbumDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"card", (object)new MessageTypeInfo(StringId.IDS_TYPE_CARD, "res://ZuneShellResources!InboxProfileDetails.uix#CardDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"forums", (object)new MessageTypeInfo(StringId.IDS_TYPE_FORUM, "res://ZuneShellResources!InboxTextDetails.uix#ForumDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"friendrequest", (object)new MessageTypeInfo(StringId.IDS_TYPE_FRIENDREQUEST, "res://ZuneShellResources!InboxProfileDetails.uix#FriendRequestUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"message", (object)new MessageTypeInfo(StringId.IDS_TYPE_TEXT, "res://ZuneShellResources!InboxTextDetails.uix#TextDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"musicvideo", (object)new MessageTypeInfo(StringId.IDS_TYPE_MUSICVIDEO, "res://ZuneShellResources!InboxTextDetails.uix#UnknownDetails"));
                MessageTypeInfo.s_MessageTypes.Add((object)"notification", (object)new MessageTypeInfo(StringId.IDS_TYPE_NOTIFICATION, "res://ZuneShellResources!InboxTextDetails.uix#NotificationDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"photos", (object)new MessageTypeInfo(StringId.IDS_TYPE_PHOTOS, "res://ZuneShellResources!InboxPhotoDetails.uix#PhotoDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"playlist", (object)new MessageTypeInfo(StringId.IDS_TYPE_PLAYLIST, "res://ZuneShellResources!InboxTrackDetails.uix#PlaylistDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"podcast", (object)new MessageTypeInfo(StringId.IDS_TYPE_PODCASTSERIES, "res://ZuneShellResources!InboxPodcastDetails.uix#PodcastDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"song", (object)new MessageTypeInfo(StringId.IDS_TYPE_SONG, "res://ZuneShellResources!InboxTrackDetails.uix#TrackDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"video", (object)new MessageTypeInfo(StringId.IDS_TYPE_VIDEO, "res://ZuneShellResources!InboxVideoDetails.uix#EpisodeMessageDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"movie", (object)new MessageTypeInfo(StringId.IDS_TYPE_VIDEO, "res://ZuneShellResources!InboxVideoDetails.uix#MovieDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"movietrailer", (object)new MessageTypeInfo(StringId.IDS_TYPE_VIDEO, "res://ZuneShellResources!InboxVideoDetails.uix#TrailerDetailsUI"));
                MessageTypeInfo.s_MessageTypes.Add((object)"", (object)new MessageTypeInfo(StringId.IDS_TYPE_UNKNOWN, "res://ZuneShellResources!InboxTextDetails.uix#UnknownDetails"));
            }
            return (MessageTypeInfo)(MessageTypeInfo.s_MessageTypes[(object)serviceType] ?? MessageTypeInfo.s_MessageTypes[(object)""]);
        }
    }
}
