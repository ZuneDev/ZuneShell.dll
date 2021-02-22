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
        private static List<DeviceIconSetBuilder> _builders = new List<DeviceIconSetBuilder>();
        private static DeviceIconSizeConstraint _backgroundConstraint = new DeviceIconSizeConstraint(2, 1023, 2, 1023);
        private static DeviceIconSizeConstraint _largeConstraint = new DeviceIconSizeConstraint(100, 225, 265, 400);
        private static DeviceIconSizeConstraint _mediumConstraint = new DeviceIconSizeConstraint(50, 110, 50, 200);
        private static DeviceIconSizeConstraint _smallConstraint = new DeviceIconSizeConstraint(15, 42, 25, 49);
        private static DeviceIconSet _defaultIconSet = new DeviceIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Large.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Large.Disconnected.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Medium.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Medium.Unsupported.png")), new InteractiveIconSet(new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Default.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Default.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Default.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Default.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Hover.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Hover.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Hover.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Hover.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Drag.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Drag.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Drag.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Drag.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Click.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Click.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Click.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Disconnected.Click.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.png"), new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.png")), new SimpleIconSet(new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.Dark.png"), new Image("res://ZuneShellResources!DefaultDevice.Connected.Syncing.Dark.png")))), new Image("res://ZuneShellResources!DefaultDevice.Background.png"), new ColorSet(new DeviceColor(87, 87, 87), new DeviceColor(0, 0, 2), new DeviceColor(0, 0, 2), new DeviceColor(0, 0, 2)));
        private static DeviceIconSet _unloadedIconSet = new DeviceIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Large.Default.png"), new Image("res://ZuneShellResources!Unloaded.Large.Disconnected.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Medium.Default.png"), new Image("res://ZuneShellResources!Unloaded.Medium.Unsupported.png")), new InteractiveIconSet(new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Default.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Default.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Default.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Default.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Hover.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Hover.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Hover.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Hover.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Drag.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Drag.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Drag.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Drag.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Click.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Click.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Click.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Disconnected.Click.Dark.png"))), new BackgroundAwareIconSet(new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.png"), new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.png")), new SimpleIconSet(new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.Dark.png"), new Image("res://ZuneShellResources!Unloaded.Connected.Syncing.Dark.png")))), new Image("res://ZuneShellResources!Unloaded.Background.png"), new ColorSet(new DeviceColor(87, 87, 87), new DeviceColor(0, 0, 2), new DeviceColor(0, 0, 2), new DeviceColor(0, 0, 2)));

        public static IDeviceIconSet BuildDeviceIconSet(
          DeviceAssetSet assetSet,
          DeviceIconSetConstructionCompletedCallback callback)
        {
            DeviceIconSetBuilder builder = new DeviceIconSetBuilder();
            _builders.Add(builder);
            builder.BuildIconSet(assetSet, result =>
           {
               callback(result);
               _builders.Remove(builder);
           });
            return UnloadedIconSet;
        }

        public static IDeviceIconSet DefaultIconSet => _defaultIconSet;

        private static IDeviceIconSet UnloadedIconSet => _unloadedIconSet;

        internal delegate void DeviceIconSetConstructionCompletedCallback(IDeviceIconSet iconSet);

        private class DeviceIconSetBuilder
        {
            private DeviceAssetSet _assetSet;
            private DeviceIconSetConstructionCompletedCallback _callback;
            private ImageListLoaderAndVerifier _loader;
            private IInteractiveDeviceIconSet _smallImageSubset;
            private DetailedImages _detailedImageSubset;
            private BackgroundImageAndColors _backgroundImageAndColorSubset;
            private static readonly ImageIdAndConstraint[] _smallImagesToLoad = new ImageIdAndConstraint[18]
            {
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDefault, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDefault, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDefaultDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDefaultDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedHover, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedHover, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedHoverDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedHoverDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDrag, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDrag, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedDragDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedDragDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedClick, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedClick, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedClickDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageDisconnectedClickDark, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedSyncing, _smallConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark, _smallConstraint)
            };
            private static readonly ImageIdAndConstraint[] _detailedImagesToLoad = new ImageIdAndConstraint[4]
            {
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageMediumDefault, _mediumConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageMediumUnsupported, _mediumConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageLargeDefault, _largeConstraint),
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageLargeDisconnected, _largeConstraint)
            };
            private static readonly ImageIdAndConstraint[] _backgroundImagesToLoad = new ImageIdAndConstraint[1]
            {
        new ImageIdAndConstraint(DeviceAssetImages.eDeviceAssetImageBackground, _backgroundConstraint)
            };

            public DeviceIconSetBuilder() => this._loader = new ImageListLoaderAndVerifier();

            public void BuildIconSet(
              DeviceAssetSet assetSet,
              DeviceIconSetConstructionCompletedCallback callback)
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
                Application.DeferredInvoke(delegate
               {
                   this.LoadSmallImages();
               }, null);
            }

            private void End()
            {
                DeviceIconSetConstructionCompletedCallback callback = this._callback;
                IDeviceIconSet result = null;
                result = this._smallImageSubset == null || this._detailedImageSubset == null || this._backgroundImageAndColorSubset == null ? DefaultIconSet : new DeviceIconSet(this._detailedImageSubset.LargeImages, this._detailedImageSubset.MediumImages, this._smallImageSubset, this._backgroundImageAndColorSubset.BackgroundImage, this._backgroundImageAndColorSubset.Colors);
                Application.DeferredInvoke(delegate
               {
                   callback(result);
               }, null);
                this._assetSet = null;
                this._callback = null;
            }

            private void LoadSmallImages() => this._loader.LoadAndVerify(_assetSet.ImageUris, _smallImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.SmallCustomImageLoadComplete));

            private void SmallCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._smallImageSubset = this.ConstructSmallImageSubset(images);
                    this.LoadDetailedImages();
                }
                else
                    this._loader.LoadAndVerify(_assetSet.DefaultImageUris, _smallImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.SmallDefaultImageLoadComplete));
            }

            private void SmallDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._smallImageSubset = images == null ? DefaultIconSet.Small : this.ConstructSmallImageSubset(images);
                this.LoadDetailedImages();
            }

            private IInteractiveDeviceIconSet ConstructSmallImageSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return new InteractiveIconSet(new BackgroundAwareIconSet(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDefault], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDefault]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDefaultDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDefaultDark])), new BackgroundAwareIconSet(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedHover], images[DeviceAssetImages.eDeviceAssetImageDisconnectedHover]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedHoverDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedHoverDark])), new BackgroundAwareIconSet(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDrag], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDrag]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedDragDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedDragDark])), new BackgroundAwareIconSet(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedClick], images[DeviceAssetImages.eDeviceAssetImageDisconnectedClick]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedClickDark], images[DeviceAssetImages.eDeviceAssetImageDisconnectedClickDark])), new BackgroundAwareIconSet(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedSyncing], images[DeviceAssetImages.eDeviceAssetImageConnectedSyncing]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark], images[DeviceAssetImages.eDeviceAssetImageConnectedSyncingDark])));
            }

            private void LoadDetailedImages() => this._loader.LoadAndVerify(_assetSet.ImageUris, _detailedImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.DetailedCustomImageLoadComplete));

            private void DetailedCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._detailedImageSubset = this.ConstructDetailedImageSubset(images);
                    this.LoadBackgroundImageAndColors();
                }
                else
                    this._loader.LoadAndVerify(_assetSet.DefaultImageUris, _detailedImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.DetailedDefaultImageLoadComplete));
            }

            private void DetailedDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._detailedImageSubset = images == null ? new DetailedImages(DefaultIconSet.Medium, DefaultIconSet.Large) : this.ConstructDetailedImageSubset(images);
                this.LoadBackgroundImageAndColors();
            }

            private DetailedImages ConstructDetailedImageSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return new DetailedImages(new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageMediumDefault], images[DeviceAssetImages.eDeviceAssetImageMediumUnsupported]), new SimpleIconSet(images[DeviceAssetImages.eDeviceAssetImageLargeDefault], images[DeviceAssetImages.eDeviceAssetImageLargeDisconnected]));
            }

            private void LoadBackgroundImageAndColors() => this._loader.LoadAndVerify(_assetSet.ImageUris, _backgroundImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.BackgroundCustomImageLoadComplete));

            private void BackgroundCustomImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                if (images != null)
                {
                    this._backgroundImageAndColorSubset = this.ConstructBackgroundImageAndColorsSubset(images);
                    this.End();
                }
                else
                    this._loader.LoadAndVerify(_assetSet.DefaultImageUris, _backgroundImagesToLoad, new ImageListLoaderAndVerifier.LoadAndVerificationCompletedHandler(this.BackgroundDefaultImageLoadComplete));
            }

            private void BackgroundDefaultImageLoadComplete(IDictionary<DeviceAssetImages, Image> images)
            {
                this._backgroundImageAndColorSubset = images == null ? new BackgroundImageAndColors(DefaultIconSet.Background, DefaultIconSet.Colors) : this.ConstructBackgroundImageAndColorsSubset(images);
                this.End();
            }

            private BackgroundImageAndColors ConstructBackgroundImageAndColorsSubset(
              IDictionary<DeviceAssetImages, Image> images)
            {
                return new BackgroundImageAndColors(images[DeviceAssetImages.eDeviceAssetImageBackground], new ColorSet(new DeviceColor(this._assetSet.Colors[0]), new DeviceColor(this._assetSet.Colors[1]), new DeviceColor(this._assetSet.Colors[2]), new DeviceColor(this._assetSet.Colors[3])));
            }

            private class ImageIdAndConstraint
            {
                private DeviceAssetImages _id;
                private DeviceIconSizeConstraint _constraint;

                public ImageIdAndConstraint(
                  DeviceAssetImages id,
                  DeviceIconSizeConstraint constraint)
                {
                    this._id = id;
                    this._constraint = constraint;
                }

                public DeviceAssetImages ID => this._id;

                public DeviceIconSizeConstraint Constraint => this._constraint;
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
                private IList<ImageIdAndConstraint> _imagesToLoad;
                private LoadAndVerificationCompletedHandler _callback;
                private int _asynchronousLoadsRemaining;
                private bool _imagesAreValid;
                private Dictionary<DeviceAssetImages, Image> _images;

                public void LoadAndVerify(
                  IList<string> paths,
                  IList<ImageIdAndConstraint> imagesToLoad,
                  LoadAndVerificationCompletedHandler callback)
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
                    this._images = new Dictionary<DeviceAssetImages, Image>(23, DeviceAssetImagesEqualityComparer.Instance);
                    Application.DeferredInvoke(delegate
                   {
                       this.LoadAndVerifyWorker();
                   }, null);
                }

                private void LoadAndVerifyWorker()
                {
                    int num = 0;
                    foreach (ImageIdAndConstraint imageIdAndConstraint in _imagesToLoad)
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
                  DeviceIconSizeConstraint constraint)
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
                            Image tempImage = null;
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
                                    DeviceIconSizeConstraint tempConstraint = constraint;
                                    tempImage.ImageLoadComplete += delegate
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
                  DeviceIconSizeConstraint constraint)
                {
                    if (ImageFitsConstraint(image, constraint))
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
                    Application.DeferredInvoke(delegate
                   {
                       this.LoadingComplete();
                   }, null);
                }

                private void LoadingComplete()
                {
                    LoadAndVerificationCompletedHandler callback = this._callback;
                    IDictionary<DeviceAssetImages, Image> result = this._imagesAreValid ? _images : null;
                    Application.DeferredInvoke(delegate
                   {
                       callback(result);
                   }, null);
                    this._paths = null;
                    this._imagesToLoad = null;
                    this._callback = null;
                }

                private static bool ImageFitsConstraint(
                  Image image,
                  DeviceIconSizeConstraint constraint)
                {
                    return image != null && constraint != null && (image.Width >= constraint.MinWidth && image.Width <= constraint.MaxWidth) && image.Height >= constraint.MinHeight && image.Height <= constraint.MaxHeight;
                }

                public delegate void LoadAndVerificationCompletedHandler(
                  IDictionary<DeviceAssetImages, Image> images);
            }

            private class DeviceAssetImagesEqualityComparer : IEqualityComparer<DeviceAssetImages>
            {
                public static DeviceAssetImagesEqualityComparer Instance = new DeviceAssetImagesEqualityComparer();

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
                this._r = (int)((packedColor & 16711680U) >> 16) / (float)byte.MaxValue;
                this._g = (int)((packedColor & 65280U) >> 8) / (float)byte.MaxValue;
                this._b = ((int)packedColor & byte.MaxValue) / (float)byte.MaxValue;
            }

            public DeviceColor(byte r, byte g, byte b)
            {
                this._r = r / 250f;
                this._g = g / 250f;
                this._b = b / 250f;
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
