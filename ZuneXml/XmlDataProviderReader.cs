// Decompiled with JetBrains decompiler
// Type: ZuneXml.XmlDataProviderReader
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.IO;
using System.Xml;

namespace ZuneXml
{
    internal class XmlDataProviderReader
    {
        private Stack _internalStack;
        private XmlTextReader _currentReader;
        private int _parentDepth;

        public XmlDataProviderReader(Stream xmlStream) => this.PushReader(new XmlTextReader(xmlStream)
        {
            WhitespaceHandling = WhitespaceHandling.None
        });

        public void Close()
        {
            while (this._currentReader != null)
                this.CloseCurrent();
        }

        public int Depth
        {
            get
            {
                int parentDepth = this._parentDepth;
                if (this.Count > 0)
                    parentDepth += this._currentReader.Depth;
                return parentDepth;
            }
        }

        public string LocalName
        {
            get
            {
                string str = string.Empty;
                if (this._currentReader != null)
                    str = this._currentReader.LocalName;
                return str;
            }
        }

        public string Name
        {
            get
            {
                string str = string.Empty;
                if (this._currentReader != null)
                    str = this._currentReader.Name;
                return str;
            }
        }

        public string Value
        {
            get
            {
                string empty = string.Empty;
                if (this._currentReader != null)
                    empty = this._currentReader.Value;
                return empty;
            }
        }

        public XmlNodeType NodeType
        {
            get
            {
                XmlNodeType xmlNodeType = XmlNodeType.None;
                if (this._currentReader != null)
                    xmlNodeType = this._currentReader.NodeType;
                return xmlNodeType;
            }
        }

        private int Count => (this._internalStack != null ? this._internalStack.Count : 0) + (this._currentReader != null ? 1 : 0);

        public bool Read()
        {
            bool flag = false;
            if (this._currentReader != null)
            {
                flag = this._currentReader.Read();
                if (!flag)
                    this.CloseCurrent();
            }
            return flag;
        }

        public bool MoveToFirstAttribute()
        {
            bool flag = false;
            if (this._currentReader != null)
                flag = this._currentReader.MoveToFirstAttribute();
            return flag;
        }

        public bool MoveToNextAttribute()
        {
            bool flag = false;
            if (this._currentReader != null)
                flag = this._currentReader.MoveToNextAttribute();
            return flag;
        }

        public void PushElement(string xmlElement)
        {
            if (this.Count <= 0 || string.IsNullOrEmpty(xmlElement))
                return;
            XmlParserContext context = new XmlParserContext(this._currentReader.NameTable, new XmlNamespaceManager(this._currentReader.NameTable), this._currentReader.XmlLang, XmlSpace.None, this._currentReader.Encoding);
            this.PushReader(new XmlTextReader(xmlElement, XmlNodeType.Element, context)
            {
                WhitespaceHandling = WhitespaceHandling.None
            });
        }

        private void CloseCurrent()
        {
            if (this._currentReader != null)
                this._currentReader.Close();
            this.PopReader();
        }

        private void PushReader(XmlTextReader reader)
        {
            if (this._currentReader != null)
            {
                if (this._internalStack == null)
                    this._internalStack = new Stack(2);
                this._parentDepth += this._currentReader.Depth;
                this._internalStack.Push(_currentReader);
            }
            else
                this._parentDepth = 0;
            this._currentReader = reader;
        }

        private void PopReader()
        {
            if (this._internalStack != null && this._internalStack.Count > 0)
            {
                this._currentReader = (XmlTextReader)this._internalStack.Pop();
                this._parentDepth -= this._currentReader.Depth;
                if (this._parentDepth >= 0)
                    return;
                this._parentDepth = 0;
            }
            else
            {
                this._currentReader = null;
                this._parentDepth = 0;
            }
        }
    }
}
