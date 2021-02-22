// Decompiled with JetBrains decompiler
// Type: ZuneXml.RatableVideo
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using System.Collections;

namespace ZuneXml
{
    internal abstract class RatableVideo : Video
    {
        internal override bool CanSync => false;

        internal override int PointsPrice => this.GetPointsPrice();

        internal override bool HasPreview => this.GetHasPreview();

        internal override bool CanPreview => this.GetCanPreview();

        internal override bool CanSubscriptionPlay => this.GetCanSubscriptionPlay();

        internal override bool CanPurchase => this.GetCanPurchase();

        internal override bool CanPurchaseHD => this.GetCanPurchaseHD();

        internal override bool CanPurchaseSD => this.GetCanPurchaseSD();

        internal override bool CanPurchaseSeason => this.GetCanPurchaseSeason();

        internal override bool CanPurchaseSeasonHD => this.GetCanPurchaseSeasonHD();

        internal override bool CanPurchaseSeasonSD => this.GetCanPurchaseSeasonSD();

        internal override bool CanPurchaseAlbumOnly => false;

        internal override bool CanRent => this.GetCanRent();

        internal override bool CanRentHD => this.GetCanRentHD();

        internal override bool CanRentSD => this.GetCanRentSD();

        internal override bool InCollection => this.GetInCollection();

        internal override bool InCollectionShortcut => this.GetInCollectionShortcut();

        internal override bool IsDownloading => this.GetIsDownloading();

        internal override bool IsParentallyBlocked => this.GetIsParentallyBlocked(this.Rating);

        internal override MiniArtist PrimaryArtist => (MiniArtist)null;

        internal override IList Artists => (IList)null;

        protected RatableVideo(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
        }

        internal abstract string Description { get; }

        internal abstract string Rating { get; }
    }
}
