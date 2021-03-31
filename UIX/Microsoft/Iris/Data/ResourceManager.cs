﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Data.ResourceManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Session;
using System;

namespace Microsoft.Iris.Data
{
    internal sealed class ResourceManager
    {
        internal const string ProtocolSeparator = "://";
        private Vector<ResourceManager.UriRedirect> _redirects;
        private Map<string, IResourceProvider> _sourcesTable;
        private static ResourceManager s_instance = new ResourceManager();

        private ResourceManager() => this._sourcesTable = new Map<string, IResourceProvider>();

        public static ResourceManager Instance => ResourceManager.s_instance;

        public void RegisterSource(string scheme, IResourceProvider source) => this._sourcesTable[scheme] = source;

        public void UnregisterSource(string scheme) => this._sourcesTable.Remove(scheme);

        public bool IsRegisteredSource(string scheme) => this._sourcesTable.ContainsKey(scheme);

        public Resource GetResource(string uri) => this.GetResource(uri, false);

        public Resource GetResource(string uri, bool forceSynchronous)
        {
            Resource resource = (Resource)null;
            if (this._redirects != null)
            {
                foreach (ResourceManager.UriRedirect redirect in this._redirects)
                {
                    if (uri.StartsWith(redirect.fromPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        if (redirect.toPrefix.Equals("{ERROR}", StringComparison.OrdinalIgnoreCase))
                        {
                            ErrorManager.ReportError("Resource {0} not found, but should have been located by a markup redirect", (object)uri);
                            return (Resource)null;
                        }
                        resource = this.GetResourceWorker(redirect.toPrefix + uri.Substring(redirect.fromPrefix.Length), true);
                        if (resource != null)
                        {
                            resource.Acquire();
                            bool flag = resource.Status == ResourceStatus.Available;
                            resource.Free();
                            if (!flag)
                                resource = (Resource)null;
                        }
                    }
                    if (resource != null)
                        break;
                }
            }
            if (resource == null)
                resource = this.GetResourceWorker(uri, forceSynchronous);
            return resource;
        }

        private Resource GetResourceWorker(string uri, bool forceSynchronous)
        {
            Resource resource = (Resource)null;
            string scheme;
            string hierarchicalPart;
            ResourceManager.ParseUri(uri, out scheme, out hierarchicalPart);
            if (string.IsNullOrEmpty(scheme) || string.IsNullOrEmpty(hierarchicalPart))
            {
                ErrorManager.ReportWarning("Invalid resource uri: '{0}'", (object)uri);
                return (Resource)null;
            }
            IResourceProvider resourceProvider;
            if (this._sourcesTable.TryGetValue(scheme, out resourceProvider))
                resource = resourceProvider.GetResource(hierarchicalPart, uri, forceSynchronous);
            else
                ErrorManager.ReportWarning("Invalid resource protocol: '{0}'", (object)scheme);
            return resource;
        }

        public static void ParseUri(string uri, out string scheme, out string hierarchicalPart)
        {
            int length = uri.IndexOf("://", StringComparison.Ordinal);
            if (length > 0)
            {
                scheme = uri.Substring(0, length);
                hierarchicalPart = uri.Substring(length + "://".Length);
            }
            else
            {
                scheme = (string)null;
                hierarchicalPart = uri;
            }
        }

        public void AddUriRedirect(string fromPrefix, string toPrefix)
        {
            ResourceManager.UriRedirect uriRedirect = new ResourceManager.UriRedirect();
            uriRedirect.fromPrefix = fromPrefix;
            uriRedirect.toPrefix = toPrefix;
            if (this._redirects == null)
                this._redirects = new Vector<ResourceManager.UriRedirect>();
            this._redirects.Add(uriRedirect);
        }

        public static Resource AcquireResource(string uri)
        {
            ErrorWatermark watermark = ErrorManager.Watermark;
            Resource resource = ResourceManager.Instance.GetResource(uri, true);
            if (resource == null)
                return (Resource)null;
            resource.Acquire();
            if (resource.Status == ResourceStatus.Error)
            {
                if (resource.ErrorDetails != null)
                    ErrorManager.ReportError(resource.ErrorDetails);
                else
                    ErrorManager.ReportError("Failed to acquire resource '{0}'", (object)uri);
            }
            else if (resource.Status != ResourceStatus.Available)
                ErrorManager.ReportError("Failed to acquire resource '{0}'.  Resources that cannot be fetched synchronously are not valid in this context", (object)uri);
            if (watermark.ErrorsDetected)
            {
                resource.Free();
                resource = (Resource)null;
            }
            return resource;
        }

        internal struct UriRedirect
        {
            public string fromPrefix;
            public string toPrefix;
        }
    }
}