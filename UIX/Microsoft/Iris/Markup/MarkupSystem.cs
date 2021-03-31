// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupSystem
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.CodeModel.Cpp;
using Microsoft.Iris.Data;
using Microsoft.Iris.Debug;
using Microsoft.Iris.Markup.Validation;
using Microsoft.Iris.OS;
using Microsoft.Iris.Session;
using Microsoft.Iris.UI;
using System;

namespace Microsoft.Iris.Markup
{
    internal static class MarkupSystem
    {
        public const string RootTag = "UIX";
        public const string SchemaVersion = "http://schemas.microsoft.com/2007/uix";
        public const string DataSchemaRootTag = "DataSchema";
        public const string DataSchemaVersion = "http://schemas.microsoft.com/2007/uixdata";
        public static UIXLoadResult UIXGlobal;
        public static RootLoadResult RootGlobal;
        public static bool CompileMode;
        public static bool TrackAdditionalMetadata;
        public static bool MarkupSystemActive;
        private static Vector s_factoriesByProtocol;
        private static Vector s_factoriesByExtension;
        private static Vector<string> s_importRedirectsFrom;
        private static Vector<string> s_importRedirectsTo;
        private static uint s_rootIslandId;
        private static uint s_activeIslands;

        public static void Startup(bool compileMode)
        {
            MarkupSystem.MarkupSystemActive = true;
            MarkupSystem.CompileMode = compileMode;
            MarkupSystem.s_factoriesByProtocol = new Vector();
            MarkupSystem.s_factoriesByExtension = new Vector();
            MarkupSystem.s_rootIslandId = MarkupSystem.AllocateIslandId();
            MarkupSystem.UIXGlobal = new UIXLoadResult("http://schemas.microsoft.com/2007/uix");
            MarkupSystem.UIXGlobal.RegisterUsage((object)typeof(MarkupSystem));
            UIXLoadResult.InitializeStatics();
            ValidateContext.InitializeStatics();
            ValidateUI.InitializeStatics();
            ValidateClass.InitializeStatics();
            ValidateStatementForEach.InitializeStatics();
            ValidateParameter.InitializeStatics();
            TypeRestriction.InitializeStatics();
            NativeMarkupDataQuery.InitializeStatics();
            NativeMarkupDataType.InitializeStatics();
            MarkupSystem.RootGlobal = new RootLoadResult("Root");
            MarkupSystem.RootGlobal.RegisterUsage((object)typeof(MarkupSystem));
            AssemblyLoadResult.Startup();
            DllLoadResult.Startup();
            ResourceManager.Instance.RegisterSource("res", (IResourceProvider)DllResources.Instance);
            HttpResources.Startup();
            ResourceManager.Instance.RegisterSource("file", (IResourceProvider)FileResources.Instance);
        }

        public static void Shutdown()
        {
            MarkupSystem.UnloadAll();
            AssemblyLoadResult.Shutdown();
            DllLoadResult.Shutdown();
            MarkupSystem.RootGlobal.UnregisterUsage((object)typeof(MarkupSystem));
            MarkupSystem.RootGlobal = (RootLoadResult)null;
            MarkupSystem.UIXGlobal.UnregisterUsage((object)typeof(MarkupSystem));
            MarkupSystem.UIXGlobal = (UIXLoadResult)null;
            HttpResources.Shutdown();
            if (MarkupSystem.s_factoriesByProtocol != null)
                MarkupSystem.s_factoriesByProtocol.Clear();
            if (MarkupSystem.s_factoriesByExtension != null)
                MarkupSystem.s_factoriesByExtension.Clear();
            MarkupSystem.MarkupSystemActive = false;
        }

        public static void EnableMetadataTracking() => MarkupSystem.TrackAdditionalMetadata = true;

        public static LoadResult Load(string uri, uint islandId)
        {
            ErrorManager.EnterContext((object)uri);
            LoadResult loadResult = MarkupSystem.ResolveLoadResult(uri, islandId);
            if (loadResult != null)
            {
                loadResult.Load(LoadPass.DeclareTypes);
                loadResult.Load(LoadPass.PopulatePublicModel);
                loadResult.Load(LoadPass.Full);
                loadResult.Load(LoadPass.Done);
            }
            ErrorManager.ExitContext();
            return loadResult;
        }

        public static LoadResult ResolveLoadResult(string uri, uint islandId)
        {
            ErrorManager.EnterContext((object)uri);
            uri = MarkupSystem.ApplyImportRedirects(uri);
            LoadResult loadResult = LoadResultCache.Read(uri);
            if (loadResult == null)
            {
                bool flag = false;
                bool cacheResult = true;
                foreach (MarkupSystem.Factory factory in MarkupSystem.s_factoriesByProtocol)
                {
                    if (uri.StartsWith(factory.key, StringComparison.Ordinal))
                    {
                        loadResult = factory.handler(uri);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    foreach (MarkupSystem.Factory factory in MarkupSystem.s_factoriesByExtension)
                    {
                        if (uri.EndsWith(factory.key, StringComparison.Ordinal))
                        {
                            loadResult = factory.handler(uri);
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                    loadResult = MarkupSystem.CreateMarkupLoadResult(uri, ref cacheResult);
                if (loadResult == null)
                    loadResult = (LoadResult)new ErrorLoadResult(uri);
                if (cacheResult && loadResult.Cachable)
                {
                    LoadResultCache.Write(uri, loadResult);
                    if (loadResult.UnderlyingUri != null)
                        LoadResultCache.Write(loadResult.UnderlyingUri, loadResult);
                }
            }
            loadResult?.AddReference(islandId);
            ErrorManager.ExitContext();
            return loadResult;
        }

        private static LoadResult CreateMarkupLoadResult(string uri, ref bool cacheResult)
        {
            Resource resource = ResourceManager.AcquireResource(uri);
            if (resource == null)
                return (LoadResult)null;
            ErrorManager.EnterContext((object)resource.Uri);
            LoadResult loadResult = LoadResultCache.Read(resource.Uri);
            if (loadResult != null)
            {
                cacheResult = false;
                LoadResultCache.Write(uri, loadResult);
            }
            else
            {
                loadResult = MarkupLoadResult.Create(uri, resource);
                if (loadResult != null)
                    resource = (Resource)null;
            }
            resource?.Free();
            ErrorManager.ExitContext();
            return loadResult;
        }

        public static uint RootIslandId => MarkupSystem.s_rootIslandId;

        public static uint AllocateIslandId()
        {
            int num1 = ~(int)MarkupSystem.s_activeIslands;
            uint num2 = (uint)(num1 & -num1);
            MarkupSystem.s_activeIslands |= num2;
            return num2;
        }

        public static void FreeIslandId(uint islandId) => MarkupSystem.s_activeIslands &= ~islandId;

        public static void UnloadIsland(uint islandId) => LoadResultCache.Remove(islandId);

        public static void UnloadAll() => LoadResultCache.Clear();

        public static bool RegisterFactoryByProtocol(string protocol, CreateLoadResultHandler handler)
        {
            if (protocol.EndsWith("://", StringComparison.Ordinal))
                protocol = protocol.Substring(0, protocol.Length - 3);
            bool flag = !ResourceManager.Instance.IsRegisteredSource(protocol);
            protocol += "://";
            if (flag)
            {
                foreach (MarkupSystem.Factory factory in MarkupSystem.s_factoriesByProtocol)
                {
                    if (factory.key == protocol)
                        flag = false;
                }
            }
            if (flag)
                MarkupSystem.s_factoriesByProtocol.Add((object)new MarkupSystem.Factory()
                {
                    key = protocol,
                    handler = handler
                });
            return flag;
        }

        public static bool RegisterFactoryByExtension(string extension, CreateLoadResultHandler handler)
        {
            bool flag = true;
            if (!extension.StartsWith(".", StringComparison.Ordinal))
                extension = "." + extension;
            foreach (MarkupSystem.Factory factory in MarkupSystem.s_factoriesByExtension)
            {
                if (factory.key == extension)
                    flag = false;
            }
            if (flag)
                MarkupSystem.s_factoriesByExtension.Add((object)new MarkupSystem.Factory()
                {
                    handler = handler,
                    key = extension
                });
            return flag;
        }

        public static void AddImportRedirect(string fromPrefix, string toPrefix)
        {
            if (MarkupSystem.s_importRedirectsFrom == null)
            {
                MarkupSystem.s_importRedirectsFrom = new Vector<string>();
                MarkupSystem.s_importRedirectsTo = new Vector<string>();
            }
            MarkupSystem.s_importRedirectsFrom.Add(fromPrefix);
            MarkupSystem.s_importRedirectsTo.Add(toPrefix);
        }

        private static string ApplyImportRedirects(string uri)
        {
            if (MarkupSystem.s_importRedirectsFrom != null)
            {
                for (int index = 0; index < MarkupSystem.s_importRedirectsFrom.Count; ++index)
                {
                    string str = MarkupSystem.s_importRedirectsFrom[index];
                    if (uri.StartsWith(str, StringComparison.Ordinal))
                    {
                        uri = uri.Substring(str.Length);
                        uri = MarkupSystem.s_importRedirectsTo[index] + uri;
                        break;
                    }
                }
            }
            return uri;
        }

        public static bool IsDebuggingEnabled(byte level) => !MarkupSystem.CompileMode && Trace.IsCategoryEnabled(TraceCategory.MarkupDebug, level);

        internal class Factory
        {
            public CreateLoadResultHandler handler;
            public string key;
        }
    }
}
