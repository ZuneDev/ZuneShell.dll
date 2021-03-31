// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.ViewItems.Host
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.Markup.UIX;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;

namespace Microsoft.Iris.ViewItems
{
    internal class Host : ViewItem, ISchemaInfo
    {
        private const string DefaultUI = "Default";
        private UIClassTypeSchema _typeRestriction;
        private UIClassTypeSchema _typeCurrent;
        private UIClass _childUI;
        private string _lastRequestedSource;
        private HostStatus _status;
        private bool _inputEnabled;
        private bool _dynamicHost;
        private bool _unloadable;
        private bool _newContentOnTop;
        private Vector<UIPropertyRecord> _heldUIProperties;
        private Vector<IDisposableObject> _heldInitialUIDisposables;
        private ChildFaultedInDelegate _loadNotify;
        private string _loadNotifyURI;
        private HostRequestPacket _pendingHostRequest;
        private uint _islandId;
        private static DeferredHandler s_startRequestHandler = new DeferredHandler(StartSourceRequest);

        public Host()
          : this(null, null)
        {
        }

        public Host(UIClassTypeSchema typeSchema)
          : this(typeSchema, typeSchema)
        {
        }

        public Host(UIClassTypeSchema typeRestriction, UIClassTypeSchema typeCurrent)
        {
            this._status = HostStatus.Normal;
            this._inputEnabled = true;
            this._typeRestriction = typeRestriction;
            this._typeCurrent = typeCurrent;
            if (this._typeCurrent == null)
                return;
            this.SetChildUI(this._typeCurrent.ConstructUI());
            this._lastRequestedSource = this.SourceFromType(_typeCurrent);
        }

        private string SourceFromType(TypeSchema type) => type.Owner.Uri + "#" + type.Name;

        public TypeSchema TypeSchema => _typeCurrent;

        protected override void OnDispose()
        {
            base.OnDispose();
            this.Cancel();
            if (this._childUI != null)
                this.SetChildUI(null);
            if (this._heldInitialUIDisposables != null)
            {
                foreach (DisposableObject initialUiDisposable in this._heldInitialUIDisposables)
                    initialUiDisposable.Dispose(this);
            }
            if (!this.Unloadable)
                return;
            this.UnloadAll();
            MarkupSystem.FreeIslandId(this._islandId);
            this._islandId = 0U;
        }

        protected override void OnOwnerDeclared(object owner)
        {
            base.OnOwnerDeclared(owner);
            UIClass uiClass = (UIClass)owner;
            if (this.ChildUI == null)
                return;
            uiClass.Children.Add(ChildUI);
        }

        public void RequestSource(string source, Vector<UIPropertyRecord> properties) => this.RequestSource(source, null, properties);

        public void RequestSource(TypeSchema type, Vector<UIPropertyRecord> properties) => this.RequestSource(null, type, properties);

        public void RequestSource(string source, TypeSchema type, Vector<UIPropertyRecord> properties)
        {
            if (type != null)
            {
                if (!HostSchema.Type.IsAssignableFrom(type))
                {
                    ErrorManager.ReportError("RequestSource failed: Referrenced type '{0}' is not a UI", type.Name);
                    return;
                }
                source = this.SourceFromType(type);
            }
            this.Cancel();
            HostRequestPacket hostRequestPacket = new HostRequestPacket();
            hostRequestPacket.Host = this;
            hostRequestPacket.Source = source;
            hostRequestPacket.Type = (UIClassTypeSchema)type;
            hostRequestPacket.Properties = properties;
            this._pendingHostRequest = hostRequestPacket;
            this._lastRequestedSource = source;
            DeferredCall.Post(DispatchPriority.High, s_startRequestHandler, hostRequestPacket);
        }

        public void Cancel()
        {
            if (this._pendingHostRequest != null)
            {
                this._pendingHostRequest.Clear();
                this._pendingHostRequest = null;
            }
            this.RevokePendingLoadNotification();
        }

        private static void StartSourceRequest(object args)
        {
            HostRequestPacket hostRequestPacket = (HostRequestPacket)args;
            if (hostRequestPacket.Host == null)
                return;
            Host host = hostRequestPacket.Host;
            string source = hostRequestPacket.Source;
            UIClassTypeSchema type = hostRequestPacket.Type;
            Vector<UIPropertyRecord> properties = hostRequestPacket.Properties;
            host.Cancel();
            ErrorManager.EnterContext(source);
            try
            {
                host.SetStatus(HostStatus.LoadingSource);
                LoadResult loadResult = null;
                string uiToCreate = null;
                if (type == null && source != null)
                    loadResult = MarkupSystem.Load(CrackSourceUri(source, out uiToCreate), host.InheritedIslandId);
                host.CompleteSourceRequest(source, type, properties, loadResult, uiToCreate);
            }
            finally
            {
                ErrorManager.ExitContext();
            }
        }

        private void CompleteSourceRequest(
          string requestedSource,
          UIClassTypeSchema requestedType,
          Vector<UIPropertyRecord> properties,
          LoadResult loadResult,
          string uiToCreate)
        {
            ErrorManager.EnterContext(requestedSource);
            ErrorWatermark watermark = ErrorManager.Watermark;
            bool flag = true;
            this.ForceContentChange();
            try
            {
                if (this._childUI != null)
                {
                    if (this._typeRestriction != null)
                        this.HoldChildUIPropertyValues();
                    this.SetChildUI(null);
                    this._typeCurrent = this._typeRestriction;
                    this.FireNotification(NotificationID.SourceType);
                }
                this._dynamicHost = true;
                UIClassTypeSchema uiClassTypeSchema = null;
                Vector<UIPropertyRecord> vector = null;
                if (requestedSource != null)
                {
                    if (requestedType == null)
                    {
                        if (loadResult == null || loadResult.Status == LoadResultStatus.Error)
                        {
                            ErrorManager.ReportError("RequestSource failed: Unable to load '{0}'", requestedSource);
                        }
                        else
                        {
                            TypeSchema type = loadResult.FindType(uiToCreate);
                            if (type == null)
                                ErrorManager.ReportError("RequestSource failed: Unable to find '{0}' within '{1}'", uiToCreate, requestedSource);
                            else if (!HostSchema.Type.IsAssignableFrom(type))
                                ErrorManager.ReportError("RequestSource failed: Referrenced type '{0}' is not a UI", uiToCreate);
                            else
                                requestedType = (UIClassTypeSchema)type;
                        }
                    }
                    if (requestedType != null)
                    {
                        uiClassTypeSchema = requestedType;
                        if (this._typeRestriction != null && !this._typeRestriction.IsAssignableFrom(uiClassTypeSchema))
                            ErrorManager.ReportError("RequestSource failed: Found '{0}' within '{1}', but, it is not a '{2}'", uiToCreate, requestedSource, _typeRestriction.Name);
                        vector = this.NegotiateNewChildUIPropertyValues(uiClassTypeSchema, properties);
                    }
                }
                if (watermark.ErrorsDetected)
                {
                    this.SetStatus(HostStatus.FailureLoadingSource);
                }
                else
                {
                    if (uiClassTypeSchema != null)
                    {
                        Host host = (Host)uiClassTypeSchema.ConstructDefault();
                        UIClass childUi = host.ChildUI;
                        foreach (UIPropertyRecord uiPropertyRecord in vector)
                        {
                            object instance = host;
                            uiPropertyRecord.Schema.SetValue(ref instance, uiPropertyRecord.Value);
                        }
                        this._heldUIProperties = null;
                        this.SetChildUI(childUi);
                        object instance1 = this;
                        uiClassTypeSchema.InitializeInstance(ref instance1);
                        this.UI.Children.Add(childUi);
                        this._typeCurrent = uiClassTypeSchema;
                        this.FireNotification(NotificationID.Source);
                        this.FireNotification(NotificationID.SourceType);
                    }
                    this.SetStatus(HostStatus.Normal);
                    if (!this.NotifyForLatestLoad())
                        return;
                    DeferredCall.Post(DispatchPriority.LayoutSync, new SimpleCallback(this.DeliverLoadCompleteNotification));
                    flag = false;
                }
            }
            finally
            {
                if (flag)
                    this.RevokePendingLoadNotification();
                ErrorManager.ExitContext();
            }
        }

        private bool NotifyForLatestLoad() => this._loadNotify != null && InvariantString.Equals(this._loadNotifyURI, this.Source);

        private void DeliverLoadCompleteNotification()
        {
            if (this.NotifyForLatestLoad() && this.ChildUI != null && this.ChildUI.RootItem != null)
                this._loadNotify(this, this.ChildUI.RootItem);
            this.RevokePendingLoadNotification();
        }

        private void RevokePendingLoadNotification()
        {
            this._loadNotify = null;
            this._loadNotifyURI = null;
        }

        private void HoldChildUIPropertyValues()
        {
            if (this._typeRestriction == null)
                return;
            this._heldUIProperties = new Vector<UIPropertyRecord>();
            for (TypeSchema typeRestriction = _typeRestriction; typeRestriction != HostSchema.Type; typeRestriction = typeRestriction.Base)
            {
                foreach (PropertySchema property1 in typeRestriction.Properties)
                {
                    string name = property1.Name;
                    if (this._childUI.Storage.ContainsKey(name) && !UIPropertyRecord.IsInList(this._heldUIProperties, name))
                    {
                        object property2 = this._childUI.GetProperty(name);
                        UIPropertyRecord.AddToList(this._heldUIProperties, name, property2);
                        if (!this._dynamicHost && property2 != null && property1.PropertyType.Disposable)
                        {
                            IDisposableObject disposable = (IDisposableObject)property2;
                            if (this._childUI.UnregisterDisposable(ref disposable))
                            {
                                if (this._heldInitialUIDisposables == null)
                                    this._heldInitialUIDisposables = new Vector<IDisposableObject>();
                                this._heldInitialUIDisposables.Add(disposable);
                                disposable.TransferOwnership(this);
                            }
                        }
                    }
                }
            }
        }

        private Vector<UIPropertyRecord> NegotiateNewChildUIPropertyValues(
          TypeSchema replacementType,
          Vector<UIPropertyRecord> specifiedProperties)
        {
            Vector<UIPropertyRecord> list = specifiedProperties ?? new Vector<UIPropertyRecord>();
            if (this._typeRestriction != null)
            {
                foreach (UIPropertyRecord heldUiProperty in this._heldUIProperties)
                {
                    if (!UIPropertyRecord.IsInList(list, heldUiProperty.Name))
                        list.Add(heldUiProperty);
                }
            }
            foreach (UIPropertyRecord uiPropertyRecord in list)
            {
                uiPropertyRecord.Schema = replacementType.FindPropertyDeep(uiPropertyRecord.Name);
                if (uiPropertyRecord.Schema == null)
                    ErrorManager.ReportError("Runtime UI replacement to '{0}' failed since a property named '{1}' was specified but doesn't exist on '{0}'", replacementType.Name, uiPropertyRecord.Name);
                else if (!uiPropertyRecord.Schema.PropertyType.IsAssignableFrom(uiPropertyRecord.Value))
                    ErrorManager.ReportError("Runtime UI replacement to '{0}' failed since the value specified ({1}) for the '{2}' property is incompatible (type expected is '{3}')", replacementType.Name, uiPropertyRecord.Value, uiPropertyRecord.Name, uiPropertyRecord.Schema.PropertyType.Name);
            }
            foreach (string name in replacementType.FindRequiredPropertyNamesDeep())
            {
                if (!UIPropertyRecord.IsInList(list, name))
                    ErrorManager.ReportError("Runtime UI replacement to '{0}' failed since required property '{1}' was never provided a value", replacementType.Name, name);
            }
            return list;
        }

        protected override ViewItemID IDForChild(ViewItem childItem) => new ViewItemID(this.Source);

        protected override FindChildResult ChildForID(
          ViewItemID part,
          out ViewItem resultItem)
        {
            resultItem = null;
            FindChildResult findChildResult = FindChildResult.Failure;
            if (part.StringPartValid && !part.IDValid)
            {
                if (InvariantString.Equals(this.Source, part.StringPart) && this.ChildUI != null && this.ChildUI.RootItem != null)
                {
                    resultItem = this.ChildUI.RootItem;
                    findChildResult = FindChildResult.Success;
                }
                else if (this._status == HostStatus.LoadingSource)
                    findChildResult = FindChildResult.PotentiallyFaultIn;
            }
            return findChildResult;
        }

        internal override void FaultInChild(ViewItemID childID, ChildFaultedInDelegate handler)
        {
            this._loadNotify = handler;
            this._loadNotifyURI = childID.StringPart;
        }

        public string Source => this._lastRequestedSource;

        public TypeSchema SourceType => _typeCurrent;

        public bool Unloadable
        {
            get => this._unloadable;
            set
            {
                if (this._unloadable == value)
                    return;
                this._unloadable = value;
                if (!this._unloadable)
                    return;
                this._islandId = MarkupSystem.AllocateIslandId();
                if (this._islandId != 0U)
                    return;
                ErrorManager.ReportError("Maximum number of Unloadable Hosts reached");
            }
        }

        public bool UnloadAll() => this.UnloadAll(true);

        public bool UnloadAll(bool requestSourceToNull)
        {
            if (this.Unloadable)
            {
                MarkupSystem.UnloadIsland(this._islandId);
                if (requestSourceToNull)
                    this.RequestSource(null, null, null);
                return true;
            }
            ErrorManager.ReportError("UnloadAll may only be called on Unloadable hosts");
            return false;
        }

        protected uint InheritedIslandId
        {
            get
            {
                if (this._islandId == 0U)
                    this._islandId = this.UI == null || this.UI.Host == null ? MarkupSystem.RootIslandId : this.UI.Host.InheritedIslandId;
                return this._islandId;
            }
        }

        public void ForceRefresh() => this.ForceRefresh(false);

        public void ForceRefresh(bool unloadMarkup)
        {
            if (unloadMarkup)
            {
                string lastRequestedSource = this._lastRequestedSource;
                this.RequestSource(null, null, null);
                DeferredCall.Post(DispatchPriority.High, new SimpleCallback(this.DeferredUnloadAll));
                DeferredCall.Post(DispatchPriority.High, new DeferredHandler(this.DeferredRequestSource), lastRequestedSource);
            }
            else
                this.RequestSource(this._lastRequestedSource, null);
        }

        private void DeferredUnloadAll() => this.UnloadAll(false);

        private void DeferredRequestSource(object objLastRequestedSource) => this.RequestSource((string)objLastRequestedSource, null);

        public HostStatus Status => this._status;

        private void SetStatus(HostStatus status)
        {
            if (this._status == status)
                return;
            this._status = status;
            this.FireNotification(NotificationID.Status);
        }

        public bool InputEnabled
        {
            get => this._inputEnabled;
            set
            {
                if (this._inputEnabled == value)
                    return;
                this._inputEnabled = value;
                if (this._childUI != null)
                    this._childUI.OnInputEnabledChanged();
                this.FireNotification(NotificationID.InputEnabled);
            }
        }

        public UIClass ChildUI => this._childUI;

        private void SetChildUI(UIClass childUI)
        {
            LoadResult loadResult = null;
            if (this._childUI != null)
            {
                loadResult = this._childUI.TypeSchema.Owner;
                this._childUI.Dispose(this);
            }
            if (this._dynamicHost)
            {
                loadResult?.UnregisterUsage(this);
                childUI?.TypeSchema.Owner.RegisterUsage(this);
            }
            this._childUI = childUI;
        }

        protected override void CreateVisualContainer(IRenderSession renderSession)
        {
            base.CreateVisualContainer(renderSession);
            if (this._childUI == null)
                return;
            this._childUI.OnHostVisibilityChanged();
        }

        protected static string CrackSourceUri(string source, out string uiToCreate)
        {
            string str = source;
            uiToCreate = "Default";
            int length = source.LastIndexOf('#');
            if (length != -1)
            {
                str = source.Substring(0, length);
                uiToCreate = source.Substring(length + 1, source.Length - length - 1);
            }
            return str;
        }

        public void NotifyChildUIScriptErrors() => this.SetStatus(HostStatus.FailureRunningScript);

        public object GetChildUIProperty(string name) => this._childUI == null ? UIPropertyRecord.FindInList(this._heldUIProperties, name).Value : this._childUI.GetProperty(name);

        public void SetChildUIProperty(string name, object value)
        {
            if (this._childUI != null)
            {
                this._childUI.SetProperty(name, value);
            }
            else
            {
                UIPropertyRecord inList = UIPropertyRecord.FindInList(this._heldUIProperties, name);
                if (Utility.IsEqual(inList.Value, value))
                    return;
                inList.Value = value;
                this.FireNotification(name);
            }
        }

        public void FireChildUINotification(string id) => this.FireNotification(id);

        public bool NewContentOnTop
        {
            get => this._newContentOnTop;
            set
            {
                if (this._newContentOnTop == value)
                    return;
                this._newContentOnTop = value;
                this.FireNotification(NotificationID.NewContentOnTop);
            }
        }

        protected override VisualOrder GetVisualOrder() => !this.NewContentOnTop ? VisualOrder.Last : VisualOrder.First;

        public override string ToString() => base.ToString() + " ('" + this._lastRequestedSource + "')";
    }
}
