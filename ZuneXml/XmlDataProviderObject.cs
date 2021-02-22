// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataProviderObject
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Shell;
using MicrosoftZuneLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ZuneXml
{
    public class XmlDataProviderObject : DataProviderObject, IXmlDataProviderObject, IDatabaseMedia
    {
        private static string _strUuidPrefix1 = "urn:uuid:";
        private static string _strUuidPrefix2 = "uid:uuid:";
        private Dictionary<string, object> _propertyValues;

        public XmlDataProviderObject(DataProviderQuery owner, object resultTypeCookie)
          : base(owner, resultTypeCookie)
        {
            foreach (DataProviderMapping dataProviderMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
            {
                string propertyTypeName = dataProviderMapping.PropertyTypeName;
                if (!this.IsXmlValueType(propertyTypeName))
                {
                    if (propertyTypeName == "List")
                        this.SetProperty(dataProviderMapping.PropertyName, (object)new XmlDataVirtualList(this.Owner, dataProviderMapping.UnderlyingCollectionTypeCookie));
                    else
                        this.SetProperty(dataProviderMapping.PropertyName, (object)XmlDataProviderObjectFactory.CreateObject(this.Owner, dataProviderMapping.PropertyTypeCookie));
                }
            }
        }

        internal IPageInfo NextPage
        {
            get
            {
                IPageInfo pageInfo = (IPageInfo)null;
                foreach (DataProviderMapping dataProviderMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
                {
                    if (dataProviderMapping.PropertyTypeName == "List")
                    {
                        XmlDataVirtualList property = (XmlDataVirtualList)this.GetProperty(dataProviderMapping.PropertyName);
                        if (property != null)
                        {
                            pageInfo = property.NextPage;
                            break;
                        }
                    }
                }
                return pageInfo;
            }
            set
            {
                foreach (DataProviderMapping dataProviderMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
                {
                    if (dataProviderMapping.PropertyTypeName == "List")
                    {
                        XmlDataVirtualList property = (XmlDataVirtualList)this.GetProperty(dataProviderMapping.PropertyName);
                        if (property != null)
                            property.NextPage = value;
                    }
                }
            }
        }

        public bool PageToEnd
        {
            get
            {
                bool flag = false;
                foreach (DataProviderMapping dataProviderMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
                {
                    if (dataProviderMapping.PropertyTypeName == "List")
                    {
                        XmlDataVirtualList property = (XmlDataVirtualList)this.GetProperty(dataProviderMapping.PropertyName);
                        if (property != null)
                        {
                            flag = property.PageToEnd;
                            break;
                        }
                    }
                }
                return flag;
            }
            set
            {
                foreach (DataProviderMapping dataProviderMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
                {
                    if (dataProviderMapping.PropertyTypeName == "List")
                    {
                        XmlDataVirtualList property = (XmlDataVirtualList)this.GetProperty(dataProviderMapping.PropertyName);
                        if (property != null)
                            property.PageToEnd = value;
                    }
                }
            }
        }

        public override object GetProperty(string propertyName)
        {
            object obj = (object)null;
            if (this._propertyValues != null && this._propertyValues.TryGetValue(propertyName, out obj))
                return obj;
            DataProviderMapping dataProviderMapping;
            if (this.Mappings.TryGetValue(propertyName, out dataProviderMapping))
            {
                object defaultValue = dataProviderMapping.DefaultValue;
                if (defaultValue != null)
                {
                    obj = defaultValue;
                }
                else
                {
                    string propertyTypeName = dataProviderMapping.PropertyTypeName;
                    if (propertyTypeName == "String")
                        obj = (object)string.Empty;
                    else if (propertyTypeName == "Int32")
                        obj = (object)0;
                    else if (propertyTypeName == "Int64")
                        obj = (object)0;
                    else if (propertyTypeName == "DateTime")
                        obj = (object)new DateTime(0L);
                    else if (propertyTypeName == "TimeSpan")
                        obj = (object)new TimeSpan(0L);
                    else if (propertyTypeName == "Guid")
                        obj = (object)Guid.Empty;
                    else if (propertyTypeName == "Boolean")
                        obj = (object)false;
                    else if (propertyTypeName == "Double")
                        obj = (object)0.0;
                    else if (propertyTypeName == "Single")
                        obj = (object)0.0f;
                }
            }
            if (obj != null)
            {
                this.EnsureValuesDictionary();
                this._propertyValues[propertyName] = obj;
            }
            return obj;
        }

        public override void SetProperty(string propertyName, object value)
        {
            if (!this.EnsureValuesDictionary() && this._propertyValues.ContainsKey(propertyName) && object.Equals(this._propertyValues[propertyName], value))
                return;
            this._propertyValues[propertyName] = value;
            this.FirePropertyChanged(propertyName);
        }

        public virtual void GetMediaIdAndType(out int mediaId, out EMediaTypes mediaType)
        {
            object property = this.GetProperty("LibraryId");
            mediaId = property == null ? -1 : (int)property;
            mediaType = XmlDataProviderObject.NameToMediaType(this.TypeName);
        }

        public static EMediaTypes NameToMediaType(string typeName)
        {
            if (typeName == "ProfileData")
                return EMediaTypes.eMediaTypeUserCard;
            if (typeName == "Track" || typeName == "PlaylistTrack" || (typeName == "ProfileTrack" || typeName == "TrackDownloadHistory") || (typeName == "TrackPurchaseHistory" || typeName == "ChannelTrack"))
                return EMediaTypes.eMediaTypeAudio;
            if (typeName == "Album")
                return EMediaTypes.eMediaTypeAudioAlbum;
            if (typeName == "PodcastSeries")
                return EMediaTypes.eMediaTypePodcastSeries;
            return typeName == "MusicVideo" ? EMediaTypes.eMediaTypeVideo : EMediaTypes.eMediaTypeInvalid;
        }

        private bool EnsureValuesDictionary()
        {
            if (this._propertyValues != null)
                return false;
            this._propertyValues = new Dictionary<string, object>();
            return true;
        }

        internal void SetPropertyFromStringValue(
          DataProviderMapping propertyMapping,
          string stringValue)
        {
            object obj = (object)stringValue;
            string propertyName = propertyMapping.PropertyName;
            string propertyTypeName = propertyMapping.PropertyTypeName;
            try
            {
                if (propertyTypeName == "Int32")
                    obj = (object)int.Parse(stringValue);
                else if (propertyTypeName == "Int64")
                    obj = (object)long.Parse(stringValue);
                else if (propertyTypeName == "TimeSpan")
                    obj = (object)XmlConvert.ToTimeSpan(stringValue);
                else if (propertyTypeName == "DateTime")
                    obj = (object)XmlConvert.ToDateTime(stringValue, XmlDateTimeSerializationMode.Utc);
                else if (propertyTypeName == "Guid")
                    obj = !stringValue.StartsWith(XmlDataProviderObject._strUuidPrefix1) ? (!stringValue.StartsWith(XmlDataProviderObject._strUuidPrefix2) ? (object)new Guid(stringValue) : (object)new Guid(stringValue.Substring(XmlDataProviderObject._strUuidPrefix2.Length))) : (object)new Guid(stringValue.Substring(XmlDataProviderObject._strUuidPrefix1.Length));
                else if (propertyTypeName == "Boolean")
                    obj = (object)bool.Parse(stringValue);
                else if (propertyTypeName == "Double")
                    obj = (object)double.Parse(stringValue, (IFormatProvider)CultureInfo.InvariantCulture);
                else if (propertyTypeName == "Single")
                    obj = (object)float.Parse(stringValue, (IFormatProvider)CultureInfo.InvariantCulture);
                else if (propertyTypeName != "String")
                    obj = (object)null;
                this.SetProperty(propertyName, obj);
            }
            catch (FormatException ex)
            {
                int num = TraceSwitches.DataProviderSwitch.TraceError ? 1 : 0;
            }
        }

        private bool IsXmlValueType(string propertyType) => propertyType == "String" || propertyType == "Int32" || (propertyType == "Int64" || propertyType == "TimeSpan") || (propertyType == "DateTime" || propertyType == "Guid" || (propertyType == "Boolean" || propertyType == "Double")) || propertyType == "Single";

        public bool ProcessXPath(
          string currentXPath,
          Hashtable attributes,
          List<XmlDataProviderQuery.XPathMatch> matches)
        {
            bool flag1 = false;
            foreach (DataProviderMapping propertyMapping in (IEnumerable<DataProviderMapping>)this.Mappings.Values)
            {
                string tailXPath;
                string matchingAttributeName;
                if (propertyMapping.Source != null && XmlDataProviderObject.MatchesXPath(currentXPath, attributes, propertyMapping.Source, out tailXPath, out matchingAttributeName))
                {
                    string propertyTypeName = propertyMapping.PropertyTypeName;
                    bool flag2 = string.IsNullOrEmpty(tailXPath);
                    bool flag3 = !flag2 && (tailXPath.StartsWith("/") || tailXPath.StartsWith("@"));
                    bool flag4 = flag2 || flag3;
                    bool encodedXml = flag2 && propertyMapping.Target == "Xml";
                    if (flag2 && (this.IsXmlValueType(propertyTypeName) || encodedXml))
                    {
                        int num = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
                        matches.Add(new XmlDataProviderQuery.XPathMatch(this, propertyMapping, matchingAttributeName, encodedXml));
                        flag1 = true;
                    }
                    else if (flag4 && propertyTypeName == "List")
                    {
                        IXmlDataProviderObject property = (IXmlDataProviderObject)this.GetProperty(propertyMapping.PropertyName);
                        int num = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
                        flag1 |= property.ProcessXPath(tailXPath, attributes, matches);
                        if (!TraceSwitches.DataProviderSwitch.TraceVerbose)
                            ;
                    }
                    else if (flag3)
                    {
                        int num1 = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
                        if (this.GetProperty(propertyMapping.PropertyName) is XmlDataProviderObject property)
                            flag1 |= property.ProcessXPath(tailXPath, attributes, matches);
                        int num2 = TraceSwitches.DataProviderSwitch.TraceVerbose ? 1 : 0;
                    }
                }
            }
            int num3 = flag1 ? 1 : 0;
            return flag1;
        }

        internal void TransferToAppThread()
        {
            if (this._propertyValues == null)
                return;
            foreach (object obj in this._propertyValues.Values)
            {
                if (obj is XmlDataProviderObject dataProviderObject)
                    dataProviderObject.TransferToAppThread();
                else if (obj is XmlDataVirtualList xmlDataVirtualList)
                    xmlDataVirtualList.TransferToAppThread();
            }
        }

        internal void OnQueryComplete()
        {
            if (this._propertyValues == null)
                return;
            foreach (object obj in this._propertyValues.Values)
            {
                if (obj is XmlDataVirtualList xmlDataVirtualList)
                    xmlDataVirtualList.OnQueryComplete();
            }
        }

        internal static bool MatchesXPath(
          string xpathCheck,
          Hashtable attributes,
          string scriptSource,
          out string tailXPath,
          out string matchingAttributeName)
        {
            tailXPath = string.Empty;
            matchingAttributeName = (string)null;
            string sourceXPath;
            string condAttrName;
            string condAttrValue;
            if (!XmlDataProviderObject.SplitScriptSource(scriptSource, out sourceXPath, out condAttrName, out condAttrValue) || condAttrName != null && (!(attributes[(object)condAttrName] is string attribute) || condAttrValue != attribute) || (!xpathCheck.StartsWith(sourceXPath) || xpathCheck.Length > sourceXPath.Length && xpathCheck[sourceXPath.Length] != '/' && xpathCheck[sourceXPath.Length] != '@'))
                return false;
            int num = sourceXPath.IndexOf('@');
            if (num >= 0)
            {
                tailXPath = xpathCheck.Substring(sourceXPath.Length);
                matchingAttributeName = sourceXPath.Substring(num + 1);
            }
            else if (xpathCheck.Length > sourceXPath.Length)
                tailXPath = xpathCheck.Substring(sourceXPath.Length);
            return true;
        }

        private static bool SplitScriptSource(
          string scriptSource,
          out string sourceXPath,
          out string condAttrName,
          out string condAttrValue)
        {
            sourceXPath = scriptSource;
            condAttrName = (string)null;
            condAttrValue = (string)null;
            int length1 = scriptSource.IndexOf('[');
            if (length1 < 0)
                return true;
            int num = scriptSource.IndexOf(']', length1 + 1);
            if (length1 == 0 || num < 0 || num != scriptSource.Length - 1)
                return false;
            sourceXPath = scriptSource.Substring(0, length1).Trim();
            string str1 = scriptSource.Substring(length1 + 1, num - (length1 + 1)).Trim();
            if (str1.Length == 0 || str1[0] != '@')
                return false;
            string str2 = str1.Substring(1);
            int length2 = str2.IndexOf('=');
            if (length2 == 0)
                return false;
            if (length2 < 0)
            {
                condAttrName = str2;
            }
            else
            {
                condAttrName = str2.Substring(0, length2).Trim();
                if (str2.Length < length2 + 2)
                    return false;
                condAttrValue = str2.Substring(length2 + 2, str2.Length - (length2 + 2) - 1).Trim();
            }
            return true;
        }

        internal void ChangeListSort(string newSortBy)
        {
            if (this._propertyValues == null)
                return;
            foreach (object obj in this._propertyValues.Values)
            {
                if (obj is XmlDataVirtualList xmlDataVirtualList)
                    xmlDataVirtualList.SortBy = newSortBy;
            }
        }
    }
}
