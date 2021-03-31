// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Markup.MarkupServices
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.ModelItems;
using Microsoft.Iris.Session;
using System.Collections;

namespace Microsoft.Iris.Markup
{
    internal sealed class MarkupServices : INotifyObject
    {
        private static MarkupServices s_instance;
        private NotifyList _errors = new NotifyList();
        private bool _warningsOnly = true;
        private NotifyService _notifier = new NotifyService();

        private MarkupServices() => ErrorManager.OnErrors += new NotifyErrorBatch(this.OnErrorBatch);

        public static MarkupServices Instance
        {
            get
            {
                if (MarkupServices.s_instance == null)
                    MarkupServices.s_instance = new MarkupServices();
                return MarkupServices.s_instance;
            }
        }

        private void OnErrorBatch(IList errors)
        {
            foreach (ErrorRecord error in errors)
            {
                if (!error.Warning)
                    this._warningsOnly = false;
                this._errors.Add(new MarkupError(error));
            }
            this.FireNotification(NotificationID.ErrorsDetected);
        }

        public IList Errors => _errors;

        public bool WarningsOnly => this._warningsOnly;

        public void ClearErrors()
        {
            this._errors.Clear();
            this._warningsOnly = true;
        }

        void INotifyObject.AddListener(Listener listener) => this._notifier.AddListener(listener);

        private void FireNotification(string id) => this._notifier.Fire(id);
    }
}
