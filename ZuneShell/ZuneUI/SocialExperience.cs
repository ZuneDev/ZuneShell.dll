// Decompiled with JetBrains decompiler
// Type: ZuneUI.SocialExperience
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Zune.Util;
using System;
using System.Collections;

namespace ZuneUI
{
    public class SocialExperience : Experience
    {
        private int _messageCount;
        private Node[] _nodes;
        private Node _friends;
        private Node _me;
        private Node _inbox;

        public SocialExperience(Frame frameOwner)
          : base(frameOwner, StringId.IDS_SOCIAL_PIVOT, SQMDataId.SocialClicks)
        {
        }

        public override IList NodesList
        {
            get
            {
                if (this._nodes == null)
                    this._nodes = new Node[3]
                    {
            this.Friends,
            this.Me,
            this.Inbox
                    };
                return _nodes;
            }
        }

        public Node Friends
        {
            get
            {
                if (this._friends == null)
                    this._friends = new Node(this, StringId.IDS_FRIENDS_PIVOT, "Social\\Friends", SQMDataId.SocialFriendsClicks);
                return this._friends;
            }
        }

        public Node Me
        {
            get
            {
                if (this._me == null)
                    this._me = new Node(this, StringId.IDS_PROFILE_PIVOT, "Social\\Profile", SQMDataId.SocialProfileClicks);
                return this._me;
            }
        }

        public Node Inbox
        {
            get
            {
                if (this._inbox == null)
                    this._inbox = new Node(this, StringId.IDS_INBOX_PIVOT, "Social\\Inbox", SQMDataId.SocialInboxClicks);
                return this._inbox;
            }
        }

        public int MessageCount
        {
            get => this._messageCount;
            set
            {
                if (this._messageCount == value)
                    return;
                if (value > this._messageCount)
                {
                    this.FirePropertyChanged("MessagesArrived");
                    if (this.MessagesArrived != null)
                        this.MessagesArrived(this, null);
                }
                this._messageCount = value;
                this.FirePropertyChanged(nameof(MessageCount));
            }
        }

        public int PlayCount
        {
            get => SignIn.Instance.SignedIn ? ProfileDataHelper.ProfilePlayCount : -1;
            set
            {
                if (ProfileDataHelper.ProfilePlayCount == value || !SignIn.Instance.SignedIn)
                    return;
                ProfileDataHelper.ProfilePlayCount = value;
                this.FirePropertyChanged(nameof(PlayCount));
            }
        }

        public DateTime CommentsLastRead
        {
            get => SignIn.Instance.SignedIn ? ProfileDataHelper.CommentsLastRead : DateTime.MinValue;
            set
            {
                if (!(ProfileDataHelper.CommentsLastRead != value) || !SignIn.Instance.SignedIn)
                    return;
                ProfileDataHelper.CommentsLastRead = value;
                this.FirePropertyChanged(nameof(CommentsLastRead));
            }
        }

        public override string DefaultUIPath => "Social\\Default";

        public event EventHandler MessagesArrived;
    }
}
