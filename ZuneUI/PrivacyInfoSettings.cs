// Decompiled with JetBrains decompiler
// Type: ZuneUI.PrivacyInfoSettings
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System;

namespace ZuneUI
{
    [Flags]
    public enum PrivacyInfoSettings
    {
        None = 0,
        AllowExplicitContent = 1,
        AllowFriends = 2,
        AllowMicrosoftCommunications = 4,
        AllowPartnerCommunications = 8,
        AllowPurchase = 16, // 0x00000010
        Communications = 32, // 0x00000020
        FriendsSharing = 64, // 0x00000040
        MusicSharing = 128, // 0x00000080
        ProfileCustomization = 256, // 0x00000100
        AllSocial = 512, // 0x00000200
        UsageCollection = 1024, // 0x00000400
        CreateNewAccount = UsageCollection | AllowMicrosoftCommunications, // 0x00000404
        CreateNewAccountWithSocial = CreateNewAccount | AllSocial, // 0x00000604
        CreateChildAccount = UsageCollection | AllowPurchase | AllowExplicitContent, // 0x00000411
        CreateChildAccountWithSocial = CreateChildAccount | ProfileCustomization | MusicSharing | FriendsSharing | Communications | AllowFriends, // 0x000005F3
        SocialSettings = UsageCollection | ProfileCustomization | MusicSharing | FriendsSharing | Communications, // 0x000005E0
        NewsletterSettings = AllowMicrosoftCommunications, // 0x00000004
        NoNewsletterSettings = SocialSettings | AllSocial | AllowPurchase | AllowFriends | AllowExplicitContent, // 0x000007F3
        All = NoNewsletterSettings | NewsletterSettings | AllowPartnerCommunications, // 0x000007FF
    }
}
