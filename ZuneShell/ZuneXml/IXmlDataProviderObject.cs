// Decompiled with JetBrains decompiler
// Type: ZuneXml.IXmlDataProviderObject
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneXml
{
    internal interface IXmlDataProviderObject
    {
        bool ProcessXPath(
          string currentXPath,
          Hashtable attributes,
          List<XmlDataProviderQuery.XPathMatch> matches);
    }
}
