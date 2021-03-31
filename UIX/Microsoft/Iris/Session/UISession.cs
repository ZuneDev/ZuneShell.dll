// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.UISession
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Audio;
using Microsoft.Iris.Drawing;
using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.OS;
using Microsoft.Iris.Queues;
using Microsoft.Iris.Render;
using Microsoft.Iris.RenderAPI.Audio;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Session
{
    internal class UISession
    {
        private bool _rtl;
        private static UISession s_theOnlySession;
        private UIDispatcher _dispatcher;
        private InputManager _inputManager;
        private IRenderEngine _engine;
        private IRenderSession _session;
        private EffectManager _effectManager;
        private bool _syncWindowPending;
        private readonly SimpleCallback _syncWindowHandler;
        private bool _initRequestedFlag;
        private readonly SimpleCallback _processInitialization;
        private bool _layoutRequestedFlag;
        private readonly SimpleCallback _processLayout;
        private bool _applyLayoutRequestedFlag;
        private readonly SimpleCallback _applyLayout;
        private bool _paintRequestedFlag;
        private readonly SimpleCallback _processPaint;
        private bool _layingOut;
        private SimpleQueue _queueSyncLayoutComplete;
        private AnimationManager _animationManager;
        private SoundManager _soundManager;
        private Form _form;
        private static readonly DeferredHandler s_deferredPlaySound = new DeferredHandler(UISession.DeferredPlaySound);
        private static readonly DeferredHandler s_deferredPlaySystemSound = new DeferredHandler(UISession.DeferredPlaySystemSound);

        public UISession()
          : this((EventHandler)null, (TimeoutHandler)null, 0U)
        {
        }

        public UISession(
          EventHandler rendererConnectedCallback,
          TimeoutHandler handlerTimeout,
          uint timeoutSecValue)
        {
            this._syncWindowHandler = new SimpleCallback(this.SyncWindowHandler);
            this._queueSyncLayoutComplete = new SimpleQueue();
            this._processInitialization = new SimpleCallback(this.ProcessInitialization);
            this._processLayout = new SimpleCallback(this.ProcessLayout);
            this._applyLayout = new SimpleCallback(this.ApplyLayout);
            this._processPaint = new SimpleCallback(this.ProcessPaint);
            this._inputManager = new InputManager(this);
            UISession.s_theOnlySession = this;
            this._dispatcher = new UIDispatcher(this, handlerTimeout, timeoutSecValue, true);
            int pdwDefaultLayout;
            Win32Api.IFWIN32(Win32Api.GetProcessDefaultLayout(out pdwDefaultLayout));
            this._rtl = pdwDefaultLayout == 1;
            this._engine = RenderApi.CreateEngine(IrisEngineInfo.CreateLocal(), (IRenderHost)this.Dispatcher);
            this._session = this._engine.Session;
            TextImageCache.Initialize(this);
            ScavengeImageCache.Initialize(this);
        }

        internal void InitializeRenderingDevices(
          GraphicsDeviceType graphicsType,
          GraphicsRenderingQuality renderingQuality,
          SoundDeviceType soundType)
        {
            this._engine.Initialize(graphicsType, renderingQuality, soundType);
            this._effectManager = new EffectManager(this._session);
            this._soundManager = new SoundManager(this, this._session);
            this._animationManager = new AnimationManager(this._session);
            this._inputManager.ConnectToRenderer();
        }

        internal bool IsGraphicsDeviceRecommended(GraphicsDeviceType graphicsType) => this._engine.IsGraphicsDeviceAvailable(graphicsType, true);

        internal bool IsGraphicsDeviceAvailable(GraphicsDeviceType graphicsType) => this._engine.IsGraphicsDeviceAvailable(graphicsType, false);

        internal bool IsSoundDeviceAvailable(SoundDeviceType soundType) => this._engine.IsSoundDeviceAvailable(soundType);

        internal bool ProcessNativeEvents() => this._engine.ProcessNativeEvents();

        internal void WaitForWork(uint nTimeoutInMsecs) => this._engine.WaitForWork(nTimeoutInMsecs);

        internal void InterThreadWake() => this._engine.InterThreadWake();

        internal void FlushBatch() => this._engine.FlushBatch();

        internal IRenderSession RenderSession => this._session;

        internal AnimationManager AnimationManager => this._animationManager;

        public void Dispose()
        {
            if (TextImageCache.Instance != null)
                TextImageCache.Instance.PrepareToShutdown();
            if (ScavengeImageCache.Instance != null)
                ScavengeImageCache.Instance.PrepareToShutdown();
            if (this._soundManager != null)
            {
                this._soundManager.Dispose();
                this._soundManager = (SoundManager)null;
            }
            if (this._animationManager != null)
            {
                this._animationManager.Dispose();
                this._animationManager = (AnimationManager)null;
            }
            if (this._form != null)
                this._form.Visible = false;
            this._inputManager.PrepareToShutDown();
            this._queueSyncLayoutComplete = (SimpleQueue)null;
            this._form = (Form)null;
            this._dispatcher.ShutDown(true);
            UISession.s_theOnlySession = (UISession)null;
            this._effectManager.Dispose();
            this._effectManager = (EffectManager)null;
            if (this._engine != null)
            {
                this._engine.Dispose();
                this._engine = (IRenderEngine)null;
                this._session = (IRenderSession)null;
            }
            TextImageCache.Uninitialize(this);
            ScavengeImageCache.Uninitialize(this);
            this._dispatcher.Dispose();
            if (RenderApi.DebugModule == null)
                return;
            RenderApi.DebugModule.Dispose();
            RenderApi.DebugModule = (IDebug)null;
        }

        internal bool IsValid => UISession.s_theOnlySession == this;

        public static void Validate(UISession session)
        {
        }

        public static UISession Default => UISession.s_theOnlySession;

        public bool IsRtl
        {
            get => this._rtl;
            set => this._rtl = value;
        }

        public UIDispatcher Dispatcher => this._dispatcher;

        public InputManager InputManager => this._inputManager;

        public EffectManager EffectManager => this._effectManager;

        internal UIZone RootZone
        {
            get
            {
                UIZone uiZone = (UIZone)null;
                if (this._form != null)
                    uiZone = this._form.Zone;
                return uiZone;
            }
        }

        public bool InLayout => this._layingOut;

        internal void ScheduleUiTask(UiTask task)
        {
            switch (task)
            {
                case UiTask.Initialization:
                    this.ScheduleInitialization();
                    break;
                case UiTask.LayoutComputation:
                    this.ScheduleLayout();
                    break;
                case UiTask.LayoutApplication:
                    this.ScheduleApplyLayout();
                    break;
                case UiTask.Painting:
                    this.SchedulePaint();
                    break;
            }
        }

        private void ScheduleLayout()
        {
            if (this._layoutRequestedFlag)
                return;
            DeferredCall.Post(DispatchPriority.Layout, this._processLayout);
            this._layoutRequestedFlag = true;
        }

        private void ScheduleApplyLayout()
        {
            if (this._applyLayoutRequestedFlag)
                return;
            DeferredCall.Post(DispatchPriority.LayoutApply, this._applyLayout);
            this._applyLayoutRequestedFlag = true;
        }

        private void SchedulePaint()
        {
            if (this._paintRequestedFlag)
                return;
            DeferredCall.Post(DispatchPriority.Render, this._processPaint);
            this._paintRequestedFlag = true;
        }

        private void ScheduleInitialization()
        {
            if (this._initRequestedFlag)
                return;
            DeferredCall.Post(DispatchPriority.High, this._processInitialization);
            this._initRequestedFlag = true;
        }

        public void RequestUpdateView(bool syncWindow)
        {
            this.InputManager.SuspendInputUntil(DispatchPriority.Idle);
            this.Dispatcher.TemporarilyBlockRPCs();
            if (!syncWindow || this._syncWindowPending)
                return;
            this._syncWindowPending = true;
            DeferredCall.Post(DispatchPriority.RenderSync, this._syncWindowHandler);
        }

        private void SyncWindowHandler()
        {
            this._syncWindowPending = false;
            this._session.GraphicsDevice.RenderNowIfPossible();
        }

        internal void EnqueueSyncLayoutCompleteHandler(object snd, EventHandler eh) => this._queueSyncLayoutComplete.PostItem((QueueItem)DeferredCall.Create(eh, snd, EventArgs.Empty));

        private void ProcessInitialization()
        {
            using (UISession.TaskReentrancyDetection.Enter("Initialization"))
            {
                if (!this.IsValid || !this._initRequestedFlag)
                    return;
                this._initRequestedFlag = false;
                this.RootZone?.ProcessUiTask(UiTask.Initialization, (object)null);
            }
        }

        private void ProcessLayout()
        {
            using (UISession.TaskReentrancyDetection.Enter("Layout"))
            {
                if (!this.IsValid || !this._layoutRequestedFlag)
                    return;
                this._layoutRequestedFlag = false;
                UIZone rootZone = this.RootZone;
                if (rootZone == null)
                    return;
                this._layingOut = true;
                rootZone.ProcessUiTask(UiTask.LayoutComputation, (object)null);
                QueueItem nextItem;
                while ((nextItem = this._queueSyncLayoutComplete.GetNextItem()) != null)
                    nextItem.Dispatch();
                this._layingOut = false;
            }
        }

        private void ApplyLayout()
        {
            using (UISession.TaskReentrancyDetection.Enter(nameof(ApplyLayout)))
            {
                if (!this.IsValid || !this._applyLayoutRequestedFlag)
                    return;
                this._applyLayoutRequestedFlag = false;
                this.RootZone?.ProcessUiTask(UiTask.LayoutApplication, (object)null);
            }
        }

        private void ProcessPaint()
        {
            using (UISession.TaskReentrancyDetection.Enter("Paint"))
            {
                if (!this.IsValid || !this._paintRequestedFlag)
                    return;
                this._paintRequestedFlag = false;
                this.RootZone?.ProcessUiTask(UiTask.Painting, (object)null);
            }
        }

        internal void FireOnUnhandledException(object sender, Exception e)
        {
            UISession.UnhandledExceptionHandler unhandledException = this.OnUnhandledException;
            if (unhandledException == null)
                return;
            UISession.UnhandledExceptionArgs args = new UISession.UnhandledExceptionArgs(e);
            unhandledException(sender, args);
        }

        public event UISession.UnhandledExceptionHandler OnUnhandledException;

        public Form Form => this._form;

        internal IRenderWindow GetRenderWindow() => this._engine.Window;

        public SoundManager SoundManager => this._soundManager;

        public void PlaySound(string stSoundSource)
        {
            UISession.PlaySoundArgs playSoundArgs = new UISession.PlaySoundArgs(this, stSoundSource);
            if (!UIDispatcher.IsUIThread)
                DeferredCall.Post(DispatchPriority.High, UISession.s_deferredPlaySound, (object)playSoundArgs);
            else
                UISession.DeferredPlaySound((object)playSoundArgs);
        }

        private static void DeferredPlaySound(object argsObject)
        {
            UISession.PlaySoundArgs playSoundArgs = argsObject as UISession.PlaySoundArgs;
            if (!playSoundArgs.uiSession.IsValid || playSoundArgs.stSoundSource == null)
                return;
            new Sound() { Source = playSoundArgs.stSoundSource }.Play();
        }

        public void PlaySystemSound(SystemSoundEvent systemSoundEvent)
        {
            UISession.PlaySystemSoundArgs playSystemSoundArgs = new UISession.PlaySystemSoundArgs(this, systemSoundEvent);
            if (!UIDispatcher.IsUIThread)
                DeferredCall.Post(DispatchPriority.High, UISession.s_deferredPlaySystemSound, (object)playSystemSoundArgs);
            else
                UISession.DeferredPlaySystemSound((object)playSystemSoundArgs);
        }

        private static void DeferredPlaySystemSound(object argsObject)
        {
            UISession.PlaySystemSoundArgs playSystemSoundArgs = argsObject as UISession.PlaySystemSoundArgs;
            if (!playSystemSoundArgs.uiSession.IsValid || playSystemSoundArgs.systemSoundEvent == SystemSoundEvent.None)
                return;
            new Sound()
            {
                SystemSoundEvent = playSystemSoundArgs.systemSoundEvent
            }.Play();
        }

        internal void RegisterHost(Form form) => this._form = form;

        public class UnhandledExceptionArgs : EventArgs
        {
            private Exception _e;

            public UnhandledExceptionArgs(Exception e) => this._e = e;

            public Exception Error => this._e;
        }

        public delegate void UnhandledExceptionHandler(
          object sender,
          UISession.UnhandledExceptionArgs args);

        private class TaskReentrancyDetection : IDisposable
        {
            private static string s_currentTask;
            private static UISession.TaskReentrancyDetection s_currentTaskClearer = new UISession.TaskReentrancyDetection();

            public static IDisposable Enter(string task)
            {
                if (UISession.TaskReentrancyDetection.s_currentTask != null)
                    InvariantString.Format("REENTRANCY DETECTED! Attempt to process task '{0}' while already processing '{1}'.", (object)UISession.TaskReentrancyDetection.s_currentTask, (object)task);
                UISession.TaskReentrancyDetection.s_currentTask = task;
                return (IDisposable)UISession.TaskReentrancyDetection.s_currentTaskClearer;
            }

            void IDisposable.Dispose() => UISession.TaskReentrancyDetection.s_currentTask = (string)null;
        }

        private class PlaySoundArgs
        {
            public UISession uiSession;
            public string stSoundSource;

            public PlaySoundArgs(UISession session, string source)
            {
                this.uiSession = session;
                this.stSoundSource = source;
            }
        }

        private class PlaySystemSoundArgs
        {
            public UISession uiSession;
            public SystemSoundEvent systemSoundEvent;

            public PlaySystemSoundArgs(UISession session, SystemSoundEvent soundEventId)
            {
                this.uiSession = session;
                this.systemSoundEvent = soundEventId;
            }
        }
    }
}
