// Decompiled with JetBrains decompiler
// Type: ZuneUI.DeviceIconSetFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace ZuneUI
{
    internal static class DeviceIconSetFactory
    {
        private const int _reasonableMaximumSurfaceSize = 1024;
        private static List<DeviceIconSetFactory.DeviceIconSetBuilder> _builders = new List<DeviceIconSetFactory.DeviceIconSetBuilder>();
        private static DeviceIconSetFactory.DeviceIconSizeConstraint _backgroundConstraint = new DeviceIconSetFactory.DeviceIconSizeConstraint(2, 1023, 2, 1023);
        private static DeviceIconSetFactory.DeviceIconSizeConstraint _largeConstraint = new DeviceIconSetFactory.DeviceIconSizeConstraint(100, 225, 265, 400);
        private static DeviceIconSetFactory.DeviceIconSizeConstraint _mediumConstraint = new DeviceIconSetFactory.DeviceIconSizeConstraint(50, 110, 50, 200);
        private static DeviceIconSetFactory.DeviceIconSizeConstraint _smallConstraint = new DeviceIconSetFactory.DeviceIconSizeConstraint(15, 42, 25, 49);
        private static DeviceIconSetFactory.DeviceIconSet _defaultIconSet = new DeviceIconSetFactory.DeviceIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Large.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Large.Disconnected.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Medium.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Medium.Unsupported.png")), (IInteractiveDeviceIconSet)new DeviceIconSetFactory.InteractiveIconSet((IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Default.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Default.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Default.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Hover.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Hover.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Hover.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Hover.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Drag.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Drag.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Drag.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Drag.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Click.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Click.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Click.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Click.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.png"), new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.Dark.png")))), new Image("res://ZuneShellResources!DefaultDevice.Background.png"), (IColorSet)new DeviceIconSetFactory.ColorSet((IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)87, (byte)87, (byte)87), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2)));
        private static DeviceIconSetFactory.DeviceIconSet _unloadedIconSet = new DeviceIconSetFactory.DeviceIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Large.Default.png"), new Image("res://ZuneShellResources!Unloaded.Large.Disconnected.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Medium.Default.png"), new Image("res://ZuneShellResources!Unloaded.Medium.Unsupported.png")), (IInteractiveDeviceIconSet)new DeviceIconSetFactory.InteractiveIconSet((IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Default.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Default.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Default.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Default.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Hover.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Hover.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Hover.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Hover.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Drag.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Drag.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Drag.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Drag.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Click.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Click.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Click.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Click.Dark.png"))), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.png"), new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.png")), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.Dark.png")))), new Image("res://ZuneShellResources!Unloaded.Background.png"), (IColorSet)new DeviceIconSetFactory.ColorSet((IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)87, (byte)87, (byte)87), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2), (IDeviceColor)new DeviceIconSetFactory.DeviceColor((byte)0, (byte)0, (byte)2)));

        public static IDeviceIconSet BuildDeviceIconSet(
          DeviceAssetSet assetSet,
          DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback callback)
        {
            DeviceIconSetFactory.DeviceIconSetBuilder builder = new DeviceIconSetFactory.DeviceIconSetBuilder();
            DeviceIconSetFactory._builders.Add(builder);
            builder.BuildIconSet(assetSet, (DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback)(result =>
           {
               callback(result);
               DeviceIconSetFactory._builders.Remove(builder);
           }));
            return DeviceIconSetFactory.UnloadedIconSet;
        }

        public static IDeviceIconSet DefaultIconSet => (IDeviceIconSet)DeviceIconSetFactory._defaultIconSet;

        private static IDeviceIconSet UnloadedIconSet => (IDeviceIconSet)DeviceIconSetFactory._unloadedIconSet;

        internal delegate void DeviceIconSetConstructionCompletedCallback(IDeviceIconSet iconSet);

        private class DeviceIconSetBuilder
        {
            private DeviceAssetSet _assetSet;
            private DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback _callback;
            private DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier _loader;
            private IInteractiveDeviceIconSet _smallImageSubset;
            private DeviceIconSetFactory.DeviceIconSetBuilder.DetailedImages _detailedImageSubset;
            private DeviceIconSetFactory.DeviceIconSetBuilder.BackgroundImageAndColors _backgroundImageAndColorSubset;
            private static readonly DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[] _smallImagesToLoad = new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[18]
            {
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDefault, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDefault, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDefaultDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDefaultDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedHover, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedHover, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedHoverDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedHoverDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDrag, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDrag, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDragDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDragDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedClick, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedClick, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedClickDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedClickDark, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedSyncing, DeviceIconSetFactory._smallConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark, DeviceIconSetFactory._smallConstraint)
            };
            private static readonly DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[] _detailedImagesToLoad = new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[4]
            {
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageMediumDefault, DeviceIconSetFactory._mediumConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageMediumUnsupported, DeviceIconSetFactory._mediumConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageLargeDefault, DeviceIconSetFactory._largeConstraint),
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageLargeDisconnected, DeviceIconSetFactory._largeConstraint)
            };
            private static readonly DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[] _backgroundImagesToLoad = new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint[1]
            {
        new DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageBackground, DeviceIconSetFactory._backgroundConstraint)
            };

            public DeviceIconSetBuilder() => this._loader = new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier();

            public void BuildIconSet(
              DeviceAssetSet assetSet,
              DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback callback)
            {
                if (!Application.IsApplicationThread)
                    throw new Exception("DeviceIconSet loading is only supported on the application thread.  This feature is not thread-safe.");
                if (this._assetSet != null)
                    throw new Exception("BuildIconSet was called while a set was already being built.  Calls to BuildIconSet cannot be reentrant.");
                if (assetSet == null)
                    throw new ArgumentNullException(nameof(assetSet));
                if (callback == null)
                    throw new ArgumentNullException(nameof(callback));
                this._assetSet = assetSet;
                this._callback = callback;
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   this.LoadSmallImages();
               }, (object)null);
            }

            private void End()
            {
                DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback callback = this._callback;
                IDeviceIconSet result = (IDeviceIconSet)null;
                result = this._smallImageSubset == null || this._detailedImageSubset == null || this._backgroundImageAndColorSubset == null ? DeviceIconSetFactory.DefaultIconSet : (IDeviceIconSet)new DeviceIconSetFactory.DeviceIconSet(this._detailedImageSubset.LargeImages, this._detailedImageSubset.MediumImages, this._smallImageSubset, this._backgroundImageAndColorSubset.BackgroundImage, this._backgroundImageAndColorSubset.Colors);
                Application.DeferredInvoke((DeferredInvokeHandler)delegate
               {
                   callback(result);
               }, (object)null);
                this._assetSet = (DeviceAssetSet)null;
                this._callback = (DeviceIconSetFactory.DeviceIconSetConstructionCompletedCallback)null;
            }

            private void LoadSmallImages() => this._loader.LoadAndVerify((IList<string>)this._assetSet.ImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._smallImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.SmallCustomImageLoadComplete));

            private void SmallCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._smallImageSubset = this.ConstructSmallImageSubset(images);
                    this.LoadDetailedImages();
                }
                else
                    this._loader.LoadAndVerify((IList<string>)this._assetSet.DefaultImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._smallImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.SmallDefaultImageLoadComplete));
            }

            private void SmallDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._smallImageSubset = images == null ? DeviceIconSetFactory.DefaultIconSet.Small : this.ConstructSmallImageSubset(images);
                this.LoadDetailedImages();
            }

            private IInteractiveDeviceIconSet ConstructSmallImageSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return (IInteractiveDeviceIconSet)new DeviceIconSetFactory.InteractiveIconSet((IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDefault], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDefault]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDefaultDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDefaultDark])), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedHover], images[DeviceAssetImages.eDeviceAssetImageDisconnectedHover]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedHoverDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedHoverDark])), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDrag], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDrag]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDragDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDragDark])), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedClick], images[DeviceAssetImages.eDeviceAssetImageDisconnectedClick]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedClickDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedClickDark])), (IBackgroundAwareDeviceIconSet)new DeviceIconSetFactory.BackgroundAwareIconSet((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedSyncing], images[DeviceAssetImages.eDeviceAssetImageConnectedSyncing]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark], images[DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark])));
            }

            private void LoadDetailedImages() => this._loader.LoadAndVerify((IList<string>)this._assetSet.ImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._detailedImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.DetailedCustomImageLoadComplete));

            private void DetailedCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._detailedImageSubset = this.ConstructDetailedImageSubset(images);
                    this.LoadBackgroundImageAndColors();
                }
                else
                    this._loader.LoadAndVerify((IList<string>)this._assetSet.DefaultImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._detailedImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.DetailedDefaultImageLoadComplete));
            }

            private void DetailedDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._detailedImageSubset = images == null ? new DeviceIconSetFactory.DeviceIconSetBuilder.DetailedImages(DeviceIconSetFactory.DefaultIconSet.Medium, DeviceIconSetFactory.DefaultIconSet.Large) : this.ConstructDetailedImageSubset(images);
                this.LoadBackgroundImageAndColors();
            }

            private DeviceIconSetFactory.DeviceIconSetBuilder.DetailedImages ConstructDetailedImageSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return new DeviceIconSetFactory.DeviceIconSetBuilder.DetailedImages((ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageMediumDefault], images[DeviceAssetImages.eDeviceAssetImageMediumUnsupported]), (ISimpleDeviceIconSet)new DeviceIconSetFactory.SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageLargeDefault], images[DeviceAssetImages.eDeviceAssetImageLargeDisconnected]));
            }

            private void LoadBackgroundImageAndColors() => this._loader.LoadAndVerify((IList<string>)this._assetSet.ImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._backgroundImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.BackgroundCustomImageLoadComplete));

            private void BackgroundCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._backgroundImageAndColorSubset = this.ConstructBackgroundImageAndColorsSubset(images);
                    this.End();
                }
                else
                    this._loader.LoadAndVerify((IList<string>)this._assetSet.DefaultImageUris, (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)DeviceIconSetFactory.DeviceIconSetBuilder._backgroundImagesToLoad, new DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.BackgroundDefaultImageLoadComplete));
            }

            private void BackgroundDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._backgroundImageAndColorSubset = images == null ? new DeviceIconSetFactory.DeviceIconSetBuilder.BackgroundImageAndColors(DeviceIconSetFactory.DefaultIconSet.Background, DeviceIconSetFactory.DefaultIconSet.Colors) : this.ConstructBackgroundImageAndColorsSubset(images);
                this.End();
            }

            private DeviceIconSetFactory.DeviceIconSetBuilder.BackgroundImageAndColors ConstructBackgroundImageAndColorsSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return new DeviceIconSetFactory.DeviceIconSetBuilder.BackgroundImageAndColors(images[DeviceAssetImages.eDeviceAssetImageBackground], (IColorSet)new DeviceIconSetFactory.ColorSet((IDeviceColor)new DeviceIconSetFactory.DeviceColor(this._assetSet.Colors[0]), (IDeviceColor)new DeviceIconSetFactory.DeviceColor(this._assetSet.Colors[1]), (IDeviceColor)new DeviceIconSetFactory.DeviceColor(this._assetSet.Colors[2]), (IDeviceColor)new DeviceIconSetFactory.DeviceColor(this._assetSet.Colors[3])));
            }

            private class ImageIdAndConstraint
            {
                private DeviceAssetImages _id;
                private DeviceIconSetFactory.DeviceIconSizeConstraint _constraint;

                public ImageIdAndConstraint(
                  DeviceAssetImages id,
                  DeviceIconSetFactory.DeviceIconSizeConstraint constraint)
                {
                    this._id = id;
                    this._constraint = constraint;
                }

                public DeviceAssetImages ID => this._id;

                public DeviceIconSetFactory.DeviceIconSizeConstraint Constraint => this._constraint;
            }

            private class DetailedImages
            {
                private ISimpleDeviceIconSet _mediumImages;
                private ISimpleDeviceIconSet _largeImages;

                public DetailedImages(ISimpleDeviceIconSet mediumImages, ISimpleDeviceIconSet largeImages)
                {
                    this._mediumImages = mediumImages;
                    this._largeImages = largeImages;
                }

                public ISimpleDeviceIconSet MediumImages => this._mediumImages;

                public ISimpleDeviceIconSet LargeImages => this._largeImages;
            }

            private class BackgroundImageAndColors
            {
                private Image _backgroundImage;
                private IColorSet _colors;

                public BackgroundImageAndColors(Image backgroundImage, IColorSet colors)
                {
                    this._backgroundImage = backgroundImage;
                    this._colors = colors;
                }

                public Image BackgroundImage => this._backgroundImage;

                public IColorSet Colors => this._colors;
            }

            private class ImageListLoaderAndVerifier
            {
                private IList<string> _paths;
                private IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint> _imagesToLoad;
                private DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler _callback;
                private int _asynchronousLoadsRemaining;
                private bool _imagesAreValid;
                private Dictionary<DeviceAssetImages, Image> _images;

                public void LoadAndVerify(
                  IList<string> paths,
                  IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint> imagesToLoad,
                  DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler callback)
                {
                    if (!Application.IsApplicationThread)
                        throw new Exception("DeviceIconSet loading is only supported on the application thread.  This feature is not thread-safe.");
                    if (this._paths != null)
                        throw new Exception("LoadAndVerify was called while a set was already being built.  Calls to LoadAndVerify cannot be reentrant.");
                    if (paths == null)
                        throw new ArgumentNullException("path");
                    if (imagesToLoad == null)
                        throw new ArgumentNullException(nameof(imagesToLoad));
                    if (callback == null)
                        throw new ArgumentNullException(nameof(callback));
                    this._paths = paths;
                    this._imagesToLoad = imagesToLoad;
                    this._callback = callback;
                    this._asynchronousLoadsRemaining = 0;
                    this._imagesAreValid = true;
                    this._images = new Dictionary<DeviceAssetImages, Image>(23, (IEqualityComparer<DeviceAssetImages>)DeviceIconSetFactory.DeviceIconSetBuilder.DeviceAssetImagesEqualityComparer.Instance);
                    Application.DeferredInvoke((DeferredInvokeHandler)delegate
                   {
                       this.LoadAndVerifyWorker();
                   }, (object)null);
                }

                private void LoadAndVerifyWorker()
                {
                    int num = 0;
                    foreach (DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint imageIdAndConstraint in (IEnumerable<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)this._imagesToLoad)
                    {
                        if (this.LoadAndVerifyImage(imageIdAndConstraint.ID, imageIdAndConstraint.Constraint))
                            ++num;
                        if (!this._imagesAreValid)
                            break;
                    }
                    this.SetAsynchronousLoadsRemaining(num);
                }

                private bool LoadAndVerifyImage(
                  DeviceAssetImages imageId,
                  DeviceIconSetFactory.DeviceIconSizeConstraint constraint)
                {
                    int index = (int)imageId;
                    bool flag1 = false;
                    if (index >= this._paths.Count)
                    {
                        this._imagesAreValid = false;
                    }
                    else
                    {
                        string path = this._paths[index];
                        if (path == null)
                            this._imagesAreValid = false;
                        else if (!this.IsPathValid(path))
                        {
                            this._imagesAreValid = false;
                        }
                        else
                        {
                            Image tempImage = (Image)null;
                            bool flag2 = false;
                            try
                            {
                                Image.RemoveCache(path, 1024, 1024);
                                tempImage = new Image(path, 1024, 1024);
                                flag2 = tempImage.Load();
                            }
                            catch (Exception ex)
                            {
                                this._imagesAreValid = false;
                            }
                            if (tempImage == null)
                                this._imagesAreValid = false;
                            else if (this._imagesAreValid)
                            {
                                if (flag2)
                                {
                                    DeviceAssetImages tempImageId = imageId;
                                    DeviceIconSetFactory.DeviceIconSizeConstraint tempConstraint = constraint;
                                    tempImage.ImageLoadComplete += (ImageLoadCompleteHandler)delegate
                                   {
                                       this.VerifyImage(tempImage, tempImageId, tempConstraint);
                                       this.DecrementAsynchronousLoadsRemaining();
                                   };
                                    flag1 = true;
                                }
                                else
                                    this.VerifyImage(tempImage, imageId, constraint);
                            }
                        }
                    }
                    return flag1;
                }

                private bool IsPathValid(string path)
                {
                    if (string.IsNullOrEmpty(path))
                        return false;
                    return !path.StartsWith("file://") || File.Exists(path.Substring("file://".Length));
                }

                private void VerifyImage(
                  Image image,
                  DeviceAssetImages imageId,
                  DeviceIconSetFactory.DeviceIconSizeConstraint constraint)
                {
                    if (DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.ImageFitsConstraint(image, constraint))
                        this._images[imageId] = image;
                    else
                        this._imagesAreValid = false;
                }

                private void SetAsynchronousLoadsRemaining(int value)
                {
                    this._asynchronousLoadsRemaining = value;
                    this.CheckForAsynchronousPortionCompleted();
                }

                private void DecrementAsynchronousLoadsRemaining()
                {
                    --this._asynchronousLoadsRemaining;
                    this.CheckForAsynchronousPortionCompleted();
                }

                private void CheckForAsynchronousPortionCompleted()
                {
                    if (this._asynchronousLoadsRemaining != 0)
                        return;
                    Application.DeferredInvoke((DeferredInvokeHandler)delegate
                   {
                       this.LoadingComplete();
                   }, (object)null);
                }

                private void LoadingComplete()
                {
                    DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler callback = this._callback;
                    IDictionary<DeviceAssetImages, Image> result = this._imagesAreValid ? (IDictionary<DeviceAssetImages, Image>)this._images : (IDictionary<DeviceAssetImages, Image>)null;
                    Application.DeferredInvoke((DeferredInvokeHandler)delegate
                   {
                       callback(result);
                   }, (object)null);
                    this._paths = (IList<string>)null;
                    this._imagesToLoad = (IList<DeviceIconSetFactory.DeviceIconSetBuilder.ImageIdAndConstraint>)null;
                    this._callback = (DeviceIconSetFactory.DeviceIconSetBuilder.ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler)null;
                }

                private static bool ImageFitsConstraint(
                  Image image,
                  DeviceIconSetFactory.DeviceIconSizeConstraint constraint)
                {
                    return image != null && constraint != null && (image.Width >= constraint.MinWidth && image.Width <= constraint.MaxWidth) && image.Height >= constraint.MinHeight && image.Height <= constraint.MaxHeight;
                }

                public delegate void LoadAndVerificationCompletedHandler(
                  IDictionary<DeviceAssetImages, Image> images);
            }

            private class DeviceAssetImagesEqualityComparer : IEqualityComparer<DeviceAssetImages>
            {
                public static DeviceIconSetFactory.DeviceIconSetBuilder.DeviceAssetImagesEqualityComparer Instance = new DeviceIconSetFactory.DeviceIconSetBuilder.DeviceAssetImagesEqualityComparer();

                private DeviceAssetImagesEqualityComparer()
                {
                }

                public bool Equals(DeviceAssetImages x, DeviceAssetImages y) => x == y;

                public int GetHashCode(DeviceAssetImages obj) => obj.GetHashCode();
            }
        }

        private class DeviceIconSizeConstraint
        {
            private int _minWidth;
            private int _maxWidth;
            private int _minHeight;
            private int _maxHeight;

            public DeviceIconSizeConstraint(int minWidth, int maxWidth, int minHeight, int maxHeight)
            {
                this._minWidth = minWidth;
                this._maxWidth = maxWidth;
                this._minHeight = minHeight;
                this._maxHeight = maxHeight;
            }

            public int MinWidth => this._minWidth;

            public int MaxWidth => this._maxWidth;

            public int MinHeight => this._minHeight;

            public int MaxHeight => this._maxHeight;
        }

        private class DeviceIconSet : IDeviceIconSet
        {
            private ISimpleDeviceIconSet _large;
            private ISimpleDeviceIconSet _medium;
            private IInteractiveDeviceIconSet _small;
            private Image _background;
            private IColorSet _colors;

            public DeviceIconSet(
              ISimpleDeviceIconSet large,
              ISimpleDeviceIconSet medium,
              IInteractiveDeviceIconSet small,
              Image background,
              IColorSet colors)
            {
                this._large = large;
                this._medium = medium;
                this._small = small;
                this._background = background;
                this._colors = colors;
            }

            public ISimpleDeviceIconSet Large => this._large;

            public ISimpleDeviceIconSet Medium => this._medium;

            public IInteractiveDeviceIconSet Small => this._small;

            public Image Background => this._background;

            public IColorSet Colors => this._colors;
        }

        private class ColorSet : IColorSet
        {
            private IDeviceColor _light;
            private IDeviceColor _dark;
            private IDeviceColor _text;
            private IDeviceColor _hoverText;

            public ColorSet(
              IDeviceColor light,
              IDeviceColor dark,
              IDeviceColor text,
              IDeviceColor hoverText)
            {
                this._light = light;
                this._dark = dark;
                this._text = text;
                this._hoverText = hoverText;
            }

            public IDeviceColor Light => this._light;

            public IDeviceColor Dark => this._dark;

            public IDeviceColor Text => this._text;

            public IDeviceColor HoverText => this._hoverText;
        }

        private class DeviceColor : IDeviceColor
        {
            private float _r;
            private float _g;
            private float _b;

            public DeviceColor(uint packedColor)
            {
                this._r = (float)(int)((packedColor & 16711680U) >> 16) / (float)byte.MaxValue;
                this._g = (float)(int)((packedColor & 65280U) >> 8) / (float)byte.MaxValue;
                this._b = (float)((int)packedColor & (int)byte.MaxValue) / (float)byte.MaxValue;
            }

            public DeviceColor(byte r, byte g, byte b)
            {
                this._r = (float)r / 250f;
                this._g = (float)g / 250f;
                this._b = (float)b / 250f;
            }

            public DeviceColor(float r, float g, float b)
            {
                this._r = r;
                this._g = g;
                this._b = b;
            }

            public float R => this._r;

            public float G => this._g;

            public float B => this._b;
        }

        private class SimpleIconSet : ISimpleDeviceIconSet
        {
            private Image _connected;
            private Image _disconnected;

            public SimpleIconSet(Image connected, Image disconnected)
            {
                this._connected = connected;
                this._disconnected = disconnected;
            }

            public Image Connected => this._connected;

            public Image Disconnected => this._disconnected;

            public Image GetImageForConnectedness(bool isConnected) => !isConnected ? this.Disconnected : this.Connected;
        }

        private class BackgroundAwareIconSet : IBackgroundAwareDeviceIconSet
        {
            private ISimpleDeviceIconSet _forLightBackground;
            private ISimpleDeviceIconSet _forDarkBackground;

            public BackgroundAwareIconSet(
              ISimpleDeviceIconSet forLightBackground,
              ISimpleDeviceIconSet forDarkBackground)
            {
                this._forLightBackground = forLightBackground;
                this._forDarkBackground = forDarkBackground;
            }

            public ISimpleDeviceIconSet ForLightBackground => this._forLightBackground;

            public ISimpleDeviceIconSet ForDarkBackground => this._forDarkBackground;

            public ISimpleDeviceIconSet GetSetForBackground(bool backgroundIsDark) => !backgroundIsDark ? this.ForLightBackground : this.ForDarkBackground;
        }

        private class InteractiveIconSet : IInteractiveDeviceIconSet
        {
            private IBackgroundAwareDeviceIconSet _default;
            private IBackgroundAwareDeviceIconSet _hover;
            private IBackgroundAwareDeviceIconSet _drag;
            private IBackgroundAwareDeviceIconSet _click;
            private IBackgroundAwareDeviceIconSet _syncing;

            public InteractiveIconSet(
              IBackgroundAwareDeviceIconSet defaultSet,
              IBackgroundAwareDeviceIconSet hover,
              IBackgroundAwareDeviceIconSet drag,
              IBackgroundAwareDeviceIconSet click,
              IBackgroundAwareDeviceIconSet syncing)
            {
                this._default = defaultSet;
                this._hover = hover;
                this._drag = drag;
                this._click = click;
                this._syncing = syncing;
            }

            public IBackgroundAwareDeviceIconSet Default => this._default;

            public IBackgroundAwareDeviceIconSet Hover => this._hover;

            public IBackgroundAwareDeviceIconSet Drag => this._drag;

            public IBackgroundAwareDeviceIconSet Click => this._click;

            public IBackgroundAwareDeviceIconSet Syncing => this._syncing;
        }
    }
}
