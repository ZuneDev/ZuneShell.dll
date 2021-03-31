﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.ScriptRunScheduler
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using System.Collections;

namespace Microsoft.Iris.Markup
{
    internal struct ScriptRunScheduler
    {
        private Vector<ScriptRunScheduler.PendingScript> _pendingList;
        private static ScriptRunScheduler.RunListCache s_listCache = new ScriptRunScheduler.RunListCache();

        public bool Pending => this._pendingList != null;

        public void ScheduleRun(uint scriptId, bool ignoreErrors)
        {
            if (this._pendingList == null)
                this._pendingList = ScriptRunScheduler.s_listCache.Acquire();
            int index;
            for (index = 0; index < this._pendingList.Count; ++index)
            {
                uint scriptId1 = this._pendingList[index].ScriptId;
                if ((int)scriptId == (int)scriptId1)
                    return;
                if (scriptId < scriptId1)
                    break;
            }
            this._pendingList.Insert(index, new ScriptRunScheduler.PendingScript(scriptId, ignoreErrors));
        }

        public void Execute(IMarkupTypeBase markupTypeBase)
        {
            if (this._pendingList == null)
                return;
            Vector<ScriptRunScheduler.PendingScript> pendingList = this._pendingList;
            this._pendingList = (Vector<ScriptRunScheduler.PendingScript>)null;
            for (int index = 0; index < pendingList.Count; ++index)
            {
                ScriptRunScheduler.PendingScript pendingScript = pendingList[index];
                markupTypeBase.RunScript(pendingScript.ScriptId, pendingScript.IgnoreErrors, new ParameterContext());
            }
            ScriptRunScheduler.s_listCache.Release(pendingList);
        }

        internal struct PendingScript
        {
            public uint ScriptId;
            public bool IgnoreErrors;

            public PendingScript(uint scriptId, bool ignoreErrors)
            {
                this.ScriptId = scriptId;
                this.IgnoreErrors = ignoreErrors;
            }
        }

        private class RunListCache
        {
            private const int c_MaxNumLists = 128;
            private const int c_MaxListSize = 32;
            private Stack _lists = new Stack(128);

            public Vector<ScriptRunScheduler.PendingScript> Acquire() => this._lists.Count <= 0 ? new Vector<ScriptRunScheduler.PendingScript>() : (Vector<ScriptRunScheduler.PendingScript>)this._lists.Pop();

            public void Release(Vector<ScriptRunScheduler.PendingScript> list)
            {
                if (this._lists.Count >= 128 || list.Capacity > 32)
                    return;
                list.Clear();
                this._lists.Push((object)list);
            }
        }
    }
}