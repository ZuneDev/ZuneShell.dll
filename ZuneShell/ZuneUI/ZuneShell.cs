// Decompiled with JetBrains decompiler
// Type: ZuneUI.ZuneShell
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Util;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.ComponentModel;

namespace ZuneUI
{
    public class ZuneShell : ModelItem
    {
        public Category DeferredNavigateCategory;
        public Node DeferredNavigateNode;
        private static ZuneShell s_defaultInstance;
        private PageStack _pageStack;
        private Command _navigateBackCommand;
        private ICommandHandler _commandHandler;
        private Management _management;
        private int _navigationsToPagePending;
        private bool _navigationLocked;
        private object thisLock = new object();

        public ZuneShell()
        {
            this._pageStack = new PageStack(this);
            this._pageStack.PropertyChanged += new PropertyChangedEventHandler(this.OnPageStackPropertyChanged);
            this._navigateBackCommand = new Command(this, Shell.LoadString(StringId.IDS_NAVIGATE_BACK), new EventHandler(this.OnClickNavigateBack));
            this._pageStack.MaximumStackSize = 1024U;
            this._pageStack.NavigateToPage(new StartupPage());
            DefaultInstance = this;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
                this.DisposeManagement();
            base.OnDispose(disposing);
            if (!disposing || DefaultInstance != this)
                return;
            DefaultInstance = null;
        }

        public Command NavigateBackCommand => this._navigateBackCommand;

        private void OnPageStackPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            string propertyName = args.PropertyName;
            if (!(propertyName == "CurrentPage") && !(propertyName == "CanNavigateBack") && (!(propertyName == "LastNavigationDirection") && !(propertyName == "MaximumStackSize")))
                return;
            this.FirePropertyChanged(propertyName);
            if (!(propertyName == "CurrentPage") && !(propertyName == "CanNavigateBack"))
                return;
            this._navigateBackCommand.Available = this.CanNavigateBack && (this.CurrentPage.ShowBackArrow || this.CurrentPage.ShowComputerIcon != ComputerIconState.Hide || this.CurrentPage.ShowNowPlayingX);
        }

        private void OnClickNavigateBack(object sender, EventArgs args)
        {
            SQMLog.Log(SQMDataId.BackClicks, 1);
            this.NavigateBack();
        }

        public int MaximumStackSize
        {
            get => (int)this._pageStack.MaximumStackSize;
            set => this._pageStack.MaximumStackSize = value >= 0 ? (uint)value : throw new ArgumentOutOfRangeException(nameof(value));
        }

        public ZunePage CurrentPage => (ZunePage)this._pageStack.CurrentPage;

        public bool CanNavigateBack => this._pageStack.CanNavigateBack;
            
        public bool NavigationLocked
        {
            get => this._navigationLocked;
            set
            {
                if (this._navigationLocked == value)
                    return;
                this._navigationLocked = value;
                this.FirePropertyChanged(nameof(NavigationLocked));
            }
        }

        public bool BlockedByNavigationLock
        {
            get => true;
            set => this.FirePropertyChanged(nameof(BlockedByNavigationLock));
        }

        public NavigationDirection LastNavigationDirection => this._pageStack.LastNavigationDirection;

        public ICommandHandler CommandHandler
        {
            get => this._commandHandler;
            set
            {
                if (this._commandHandler == value)
                    return;
                this._commandHandler = value;
                this.FirePropertyChanged(nameof(CommandHandler));
            }
        }

        public static ZuneShell DefaultInstance
        {
            get => s_defaultInstance;
            private set => s_defaultInstance = s_defaultInstance == null || value == null ? value : throw new InvalidOperationException("Should only have one static shell instance.");
        }

        public bool NavigationsPending => this._navigationsToPagePending > 0;

        public void NavigateToPage(ZunePage page)
        {
            lock (this.thisLock)
                ++this._navigationsToPagePending;
            Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredNavigateToPage), page);
        }

        private void DeferredNavigateToPage(object args)
        {
            ZunePage zunePage = (ZunePage)args;
            if (this.CurrentPage == null || this.CurrentPage.CanNavigateForwardTo(zunePage))
                this._pageStack.NavigateToPage(zunePage);
            else
                zunePage.Release();
            lock (this.thisLock)
                --this._navigationsToPagePending;
        }

        public void NavigateBack() => this.NavigateBack(false);

        public void NavigateBack(bool bypassPage) => Application.DeferredInvoke(new DeferredInvokeHandler(this.DeferredNavigateBack), bypassPage);

        private void DeferredNavigateBack(object args)
        {
            if (!(bool)args && this.CurrentPage != null && this.CurrentPage.HandleBack())
                return;
            this._pageStack.NavigateBack();
        }

        public void Execute(string command, IDictionary commandArguments)
        {
            if (this._commandHandler == null)
                throw new InvalidOperationException("No CommandHandler has been registered.  Unable to resolve shell command: " + command);
            this._commandHandler.Execute(command, commandArguments);
        }

        public void LaunchHelp() => this.Execute(InternetConnection.Instance.IsConnected ? "Web\\" + CultureHelper.GetHelpUrl() : "Help\\" + Shell.LoadString(StringId.IDS_ZUNECLIENT_LOCALE) + "\\help.htm", null);

        public Management Management
        {
            get
            {
                if (this._management == null && DefaultInstance != null)
                    this._management = new Management();
                return this._management;
            }
        }

        public void DisposeManagement()
        {
            if (this._management == null)
                return;
            this._management.Dispose();
            this._management = null;
        }

        public static MediaType MapStringToMediaType(string typeName) => (MediaType)LibraryDataProvider.NameToMediaType(typeName);

        public static MediaType MapIntToMediaType(int mediaType) => (MediaType)mediaType;

        public static int MapMediaTypeToInt(MediaType mediaType) => (int)mediaType;

        public static EMediaTypes MapMediaTypeToEMediaTypes(MediaType mediaType) => (EMediaTypes)mediaType;

        public static MediaType GetMediaTypeFromMedia(object media)
        {
            EMediaTypes mediaType = EMediaTypes.eMediaTypeInvalid;
            if (media is IDatabaseMedia)
                ((IDatabaseMedia)media).GetMediaIdAndType(out int _, out mediaType);
            return (MediaType)mediaType;
        }
    }
}
