// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Application
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Animations;
using Microsoft.Iris.Data;
using Microsoft.Iris.Input;
using Microsoft.Iris.Library;
using Microsoft.Iris.Markup;
using Microsoft.Iris.OS;
using Microsoft.Iris.Render;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Threading;

namespace Microsoft.Iris
{
    public static class Application
    {
        private static Application.InitializationState s_initializationState = Application.InitializationState.NotInitialized;
        private static UISession s_session;
        private static Window s_mainWindow;
        private static bool s_isShuttingDown;
        private static RenderingType s_renderType = RenderingType.Default;
        private static SoundType s_soundType = SoundType.None;
        private static RenderingQuality s_renderingQuality = RenderingQuality.MaxQuality;
        private static bool s_EnableAnimations = true;
        private static bool s_IsRTL = false;
        private static Dictionary<int, IExternalAnimationInput> s_idToExternalAnimationInput;
        private static Dictionary<int, IAnimationInputProvider> s_animationProviders;

        public static string Name
        {
            get => UIApplication.ApplicationName;
            set => UIApplication.ApplicationName = value;
        }

        public static Window Window
        {
            get
            {
                if (Application.s_initializationState != Application.InitializationState.FullyInitialized)
                    throw new InvalidOperationException("Application.Initialize must be called prior Window query");
                if (Application.s_mainWindow == null)
                    Application.s_mainWindow = new Window((UIForm)Application.s_session.Form);
                return Application.s_mainWindow;
            }
        }

        public static RenderingType RenderingType
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                Application.s_renderType = value;
            }
            get => Application.s_renderType;
        }

        public static SoundType SoundType
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                Application.s_soundType = value;
            }
            get => Application.s_soundType;
        }

        public static RenderingQuality RenderingQuality
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                Application.s_renderingQuality = value;
            }
            get => Application.s_renderingQuality;
        }

        public static bool AnimationsEnabled
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                Application.s_EnableAnimations = value;
            }
            get => Application.s_EnableAnimations;
        }

        public static bool IsRTL
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                Application.s_IsRTL = value;
            }
            get => Application.s_IsRTL;
        }

        public static bool IsDx9AccelerationAvailable
        {
            get
            {
                if (Application.s_initializationState != Application.InitializationState.FullyInitialized)
                    throw new InvalidOperationException("Application.Initialize must be called prior to DX9 check");
                return Application.s_session.IsGraphicsDeviceAvailable(GraphicsDeviceType.Direct3D9);
            }
        }

        private static bool IsInitialized => Application.s_initializationState != Application.InitializationState.NotInitialized;

        public static bool StaticDllResourcesOnly
        {
            set
            {
                if (Application.IsInitialized)
                    throw new InvalidOperationException("Property cannot be modified after application has been initialized");
                DllResources.StaticDllResourcesOnly = value;
            }
            get => DllResources.StaticDllResourcesOnly;
        }

        public static void Initialize()
        {
            if (Application.IsInitialized)
                throw new InvalidOperationException("Application already initialized");
            Application.VerifyTrustedEnvironment();
            Application.s_session = new UISession();
            Application.s_session.IsRtl = Application.s_IsRTL;
            Application.s_session.InputManager.KeyCoalescePolicy = new KeyCoalesceFilter(Application.QueryKeyCoalesce);
            GraphicsDeviceType graphicsType = Application.ChooseRenderingGraphicsDevice(Application.s_renderType);
            switch (graphicsType)
            {
                case GraphicsDeviceType.Gdi:
                    Application.s_renderType = RenderingType.GDI;
                    break;
                case GraphicsDeviceType.Direct3D9:
                    Application.s_renderType = RenderingType.DX9;
                    break;
                default:
                    throw new ArgumentException(InvariantString.Format("Unknown graphics type {0}", graphicsType));
            }
            if (graphicsType == GraphicsDeviceType.Gdi)
                Application.s_EnableAnimations = false;
            SoundDeviceType soundType = Application.ChooseRendererSoundDevice(Application.s_soundType);
            switch (soundType)
            {
                case SoundDeviceType.None:
                    Application.s_soundType = SoundType.None;
                    break;
                case SoundDeviceType.DirectSound8:
                    Application.s_soundType = SoundType.DirectSound;
                    break;
                default:
                    throw new ArgumentException(InvariantString.Format("Unknown sound type {0}", soundType));
            }
            Application.s_session.InitializeRenderingDevices(graphicsType, (GraphicsRenderingQuality)Application.s_renderingQuality, soundType);
            Application.s_renderingQuality = (RenderingQuality)Application.s_session.RenderSession.GraphicsDevice.RenderingQuality;
            Application.InitializeCommon(true);
            if (!Application.s_EnableAnimations)
                AnimationSystem.OverrideAnimationState(true);
            UIForm uiForm = new UIForm(Application.s_session);
            Application.s_initializationState = Application.InitializationState.FullyInitialized;
        }

        private static void InitializeCommon(bool fullInitialization)
        {
            ErrorManager.OnErrors += new NotifyErrorBatch(Application.NotifyErrorBatchHandler);
            Microsoft.Iris.Debug.Trace.Initialize();
            MarkupSystem.Startup(!fullInitialization);
            StaticServices.Initialize();
        }

        public static void InitializeForToolOnly()
        {
            if (Application.IsInitialized)
                throw new InvalidOperationException("Application has already been initialized");
            RenderApi.InitializeForToolOnly();
            UIDispatcher uiDispatcher = new UIDispatcher(true);
            Application.InitializeCommon(false);
            Application.s_initializationState = Application.InitializationState.InitializedWithoutUI;
        }

        public static bool IsApplicationThread => UIDispatcher.IsUIThread;

        public static bool LoadMarkup(string uri)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            return MarkupSystem.Load(uri, MarkupSystem.RootIslandId) != null;
        }

        public static bool CompileMarkup(CompilerInput[] compilands, CompilerInput dataTableCompiland)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (compilands == null)
                throw new ArgumentNullException(nameof(compilands));
            foreach (CompilerInput compiland in compilands)
            {
                if (compiland.SourceFileName == null)
                    throw new ArgumentNullException("SourceFileName");
                if (compiland.OutputFileName == null)
                    throw new ArgumentNullException("OutputFileName");
            }
            if (dataTableCompiland.SourceFileName == null && dataTableCompiland.OutputFileName != null || dataTableCompiland.SourceFileName != null && dataTableCompiland.OutputFileName == null)
                throw new ArgumentException(nameof(dataTableCompiland));
            return MarkupCompiler.Compile(compilands, dataTableCompiland);
        }

        public static void UnloadAllMarkup()
        {
            UIDispatcher.VerifyOnApplicationThread();
            MarkupSystem.UnloadAll();
        }

        public static void AddMarkupRedirect(string fromPrefix, string toPrefix)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (fromPrefix == null)
                throw new ArgumentNullException(nameof(fromPrefix));
            if (toPrefix == null)
                throw new ArgumentNullException(nameof(toPrefix));
            ResourceManager.Instance.AddUriRedirect(fromPrefix, toPrefix);
        }

        public static void AddImportRedirect(string fromPrefix, string toPrefix)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (fromPrefix == null)
                throw new ArgumentNullException(nameof(fromPrefix));
            if (toPrefix == null)
                throw new ArgumentNullException(nameof(toPrefix));
            MarkupSystem.AddImportRedirect(fromPrefix, toPrefix);
        }

        public static void RegisterDataProvider(string name, DataProviderQueryFactory factory)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (MarkupDataProvider.GetDataProvider(name) != null)
                throw new ArgumentException("Provider is already registered");
            MarkupDataProvider.RegisterDataProvider(new AssemblyDataProviderWrapper(name, factory));
        }

        public static void Run(DeferredInvokeHandler initialLoadComplete)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (Application.s_initializationState != Application.InitializationState.FullyInitialized)
                throw new InvalidOperationException("Application not initialized for displaying UI");
            if (initialLoadComplete != null)
                ((UIForm)Application.s_session.Form).SetInitialLoadCompleteCallback(DeferredInvokeProxy.Thunk(initialLoadComplete));
            UIApplication.Run();
        }

        public static void Run() => Application.Run(null);

        public static event EventHandler ShuttingDown;

        public static bool IsShuttingDown => Application.s_isShuttingDown;

        public static void Shutdown()
        {
            UIDispatcher.VerifyOnApplicationThread();
            Application.s_isShuttingDown = true;
            if (Application.ShuttingDown != null)
                Application.ShuttingDown(null, EventArgs.Empty);
            MarkupSystem.Shutdown();
            if (Application.s_initializationState == Application.InitializationState.FullyInitialized)
            {
                Application.s_session.Dispose();
                Application.s_session = null;
            }
            if (Application.s_initializationState == Application.InitializationState.InitializedWithoutUI)
                RenderApi.ShutdownForToolOnly();
            StaticServices.Uninitialize();
            Microsoft.Iris.Debug.Trace.Shutdown();
            ErrorManager.OnErrors -= new NotifyErrorBatch(Application.NotifyErrorBatchHandler);
            Application.s_initializationState = Application.InitializationState.NotInitialized;
        }

        public static void DeferredInvoke(DeferredInvokeHandler method, DeferredInvokePriority priority) => Application.DeferredInvoke(method, null, priority);

        public static void DeferredInvoke(DeferredInvokeHandler method, object args) => Application.DeferredInvoke(method, args, DeferredInvokePriority.Normal);

        public static void DeferredInvoke(
          DeferredInvokeHandler method,
          object args,
          DeferredInvokePriority priority)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            DispatchPriority priority1;
            switch (priority)
            {
                case DeferredInvokePriority.Normal:
                    priority1 = DispatchPriority.AppEvent;
                    break;
                case DeferredInvokePriority.Low:
                    priority1 = DispatchPriority.Idle;
                    break;
                default:
                    throw new ArgumentException(InvariantString.Format("Unknown DeferredInvokePriority {0}", priority));
            }
            DeferredCall.Post(priority1, DeferredInvokeProxy.Thunk(method), args);
        }

        public static void DeferredInvoke(DeferredInvokeHandler method, TimeSpan delay) => Application.DeferredInvoke(method, null, delay);

        public static void DeferredInvoke(DeferredInvokeHandler method, object args, TimeSpan delay)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            DeferredCall.Post(delay, DeferredInvokeProxy.Thunk(method), args);
        }

        public static void DeferredInvoke(Thread thread, DeferredInvokeHandler method) => Application.DeferredInvoke(thread, method, null);

        public static void DeferredInvoke(Thread thread, DeferredInvokeHandler method, object args)
        {
            if (thread == null)
                throw new ArgumentNullException(nameof(thread));
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            DeferredCall.Post(thread, DispatchPriority.AppEvent, DeferredInvokeProxy.Thunk(method), args);
        }

        public static void DeferredInvokeOnWorkerThread(
          DeferredInvokeHandler workerMethod,
          DeferredInvokeHandler notifyMethod,
          object args)
        {
            if (workerMethod == null)
                throw new ArgumentNullException(nameof(workerMethod));
            if (notifyMethod == null)
                throw new ArgumentNullException(nameof(notifyMethod));
            UIApplication.DeferredInvokeOnWorkerThread(DeferredInvokeProxy.Thunk(workerMethod), DeferredInvokeProxy.Thunk(notifyMethod), args);
        }

        public static void RunWorkerMessagePump(
          DeferredInvokeHandler initialWork,
          object initialWorkArgs)
        {
            if (UIDispatcher.CurrentDispatcher != null)
                throw new InvalidOperationException("Thread already has a dispatcher running");
            if (initialWork == null)
                throw new ArgumentNullException(nameof(initialWork));
            UIApplication.StartArgs startArgs = null;
            if (initialWork != null)
                startArgs = new UIApplication.StartArgs(DeferredInvokeProxy.Thunk(initialWork), initialWorkArgs);
            UIApplication.StartDispatcher(startArgs);
        }

        public static Thread StartWorkerThreadWithMessagePump(string threadName) => UIApplication.StartThreadWithDispatcher(threadName);

        public static void ShutdownWorkerMessagePump(Thread thread)
        {
            if (thread == UIDispatcher.MainUIThread)
                throw new InvalidOperationException("Use Application.Shutdown to shut down the application dispatcher");
            UIDispatcher.StopCurrentMessageLoop(thread);
        }

        public static int CreateExternalAnimationInput(IDictionary<string, int> propertyNameToId)
        {
            UIDispatcher.VerifyOnApplicationThread();
            if (Application.s_idToExternalAnimationInput == null)
                Application.s_idToExternalAnimationInput = new Dictionary<int, IExternalAnimationInput>();
            SimpleAnimationPropertyMap animationPropertyMap = new SimpleAnimationPropertyMap(propertyNameToId);
            IExternalAnimationInput externalAnimationInput = Application.s_session.RenderSession.AnimationSystem.CreateExternalAnimationInput(s_idToExternalAnimationInput, animationPropertyMap);
            Application.s_idToExternalAnimationInput.Add((int)externalAnimationInput.UniqueId, externalAnimationInput);
            return (int)externalAnimationInput.UniqueId;
        }

        public static void DisposeExternalAnimationInput(int animationId)
        {
            UIDispatcher.VerifyOnApplicationThread();
            IExternalAnimationInput externalAnimationInput;
            if (Application.s_idToExternalAnimationInput == null || !Application.s_idToExternalAnimationInput.TryGetValue(animationId, out externalAnimationInput))
                return;
            Application.s_idToExternalAnimationInput.Remove(animationId);
            externalAnimationInput.UnregisterUsage(s_idToExternalAnimationInput);
            IAnimationInputProvider animationInputProvider;
            if (Application.s_animationProviders == null || !Application.s_animationProviders.TryGetValue(animationId, out animationInputProvider))
                return;
            Application.s_animationProviders.Remove(animationId);
            animationInputProvider.UnregisterUsage(s_idToExternalAnimationInput);
        }

        internal static IExternalAnimationInput MapExternalAnimationInput(
          int animationId)
        {
            UIDispatcher.VerifyOnApplicationThread();
            IExternalAnimationInput externalAnimationInput = null;
            if (Application.s_idToExternalAnimationInput != null)
                Application.s_idToExternalAnimationInput.TryGetValue(animationId, out externalAnimationInput);
            return externalAnimationInput;
        }

        public static void SetExternalAnimationInputProperty(
          int animationId,
          string property,
          float value)
        {
            UIDispatcher.VerifyOnApplicationThread();
            IAnimationInputProvider provider;
            if (Application.s_animationProviders == null || !Application.s_animationProviders.TryGetValue(animationId, out provider))
            {
                provider = Application.MapExternalAnimationInput(animationId).CreateProvider(s_idToExternalAnimationInput);
                if (Application.s_animationProviders == null)
                    Application.s_animationProviders = new Dictionary<int, IAnimationInputProvider>();
                Application.s_animationProviders.Add(animationId, provider);
            }
            provider.PublishFloat(property, value);
        }

        public static event ErrorReportHandler ErrorReport;

        private static void NotifyErrorBatchHandler(IList records)
        {
            if (Application.ErrorReport == null)
                return;
            Error[] errors = new Error[records.Count];
            for (int index = 0; index < records.Count; ++index)
            {
                ErrorRecord record = (ErrorRecord)records[index];
                errors[index] = new Error()
                {
                    Context = record.Context,
                    Message = record.Message,
                    Warning = record.Warning,
                    Line = record.Line,
                    Column = record.Column
                };
            }
            Application.ErrorReport(errors);
        }

        private static GraphicsDeviceType ChooseRenderingGraphicsDevice(
          RenderingType type)
        {
            GraphicsDeviceType graphicsType;
            switch (type)
            {
                case RenderingType.GDI:
                    graphicsType = GraphicsDeviceType.Gdi;
                    break;
                case RenderingType.DX9:
                case RenderingType.Default:
                    graphicsType = GraphicsDeviceType.Direct3D9;
                    break;
                default:
                    throw new ArgumentException(InvariantString.Format("Unknown rendering type {0}", type));
            }
            if (type == RenderingType.Default && !Application.s_session.IsGraphicsDeviceRecommended(graphicsType) || !Application.s_session.IsGraphicsDeviceAvailable(graphicsType))
                graphicsType = GraphicsDeviceType.Gdi;
            return graphicsType;
        }

        private static SoundDeviceType ChooseRendererSoundDevice(SoundType typeRequested)
        {
            if (typeRequested == SoundType.None)
                return SoundDeviceType.None;
            if (typeRequested != SoundType.DirectSound)
                throw new ArgumentException(InvariantString.Format("Unknown sound type {0}", typeRequested));
            return Application.s_session.IsSoundDeviceAvailable(SoundDeviceType.DirectSound8) ? SoundDeviceType.DirectSound8 : SoundDeviceType.None;
        }

        private static bool QueryKeyCoalesce(Keys key) => true;

        private static void VerifyTrustedEnvironment()
        {
            Assembly assembly1 = new StackTrace().GetFrame(2).GetMethod().DeclaringType.Assembly;
            Assembly assembly2 = typeof(Application).Assembly;
            byte[] publicKey1 = assembly1.GetName().GetPublicKey();
            byte[] publicKey2 = assembly2.GetName().GetPublicKey();
            bool flag = true;
            if (publicKey1.Length == publicKey2.Length)
            {
                for (int index = 0; index < publicKey2.Length; ++index)
                {
                    if (publicKey1[index] != publicKey2[index])
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
                flag = false;
            //if (!flag)
            //    throw new SecurityException("Attempt to activate system within an untrusted environment");
        }

        private enum InitializationState
        {
            NotInitialized,
            FullyInitialized,
            InitializedWithoutUI,
        }
    }
}
