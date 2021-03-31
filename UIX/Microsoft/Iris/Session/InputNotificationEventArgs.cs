// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.InputNotificationEventArgs
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Input;

namespace Microsoft.Iris.Session
{
    internal struct InputNotificationEventArgs
    {
        private EventRouteStages _handledStage;
        private InputInfo _info;
        private ICookedInputSite _target;

        public InputNotificationEventArgs(
          InputInfo info,
          ICookedInputSite target,
          EventRouteStages stage)
        {
            this._info = info;
            this._target = target;
            this._handledStage = stage;
        }

        public ICookedInputSite Target => this._target;

        public InputInfo InputInfo => this._info;

        public EventRouteStages HandledStage => this._handledStage;
    }
}
