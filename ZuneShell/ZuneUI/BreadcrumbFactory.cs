// Decompiled with JetBrains decompiler
// Type: ZuneUI.BreadcrumbFactory
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using System.Collections;
using System.Collections.Generic;

namespace ZuneUI
{
    public class BreadcrumbFactory
    {
        private IList<Breadcrumb> _breadcrumbs;

        public IList Breadcrumbs => (IList)this._breadcrumbs;

        public void AddCrumb(Breadcrumb breadcrumb)
        {
            if (this._breadcrumbs == null)
            {
                this._breadcrumbs = new List<Breadcrumb>();
                breadcrumb.Active = true;
            }
            this._breadcrumbs.Add(breadcrumb);
        }

        public void UpdateState(WizardPage currentPage, WizardPage destinationPage, bool movingNext)
        {
            for (int index = 0; index < this._breadcrumbs.Count; ++index)
            {
                Breadcrumb breadcrumb = this._breadcrumbs[index];
                if (breadcrumb.Page == currentPage)
                {
                    if (!movingNext)
                    {
                        this._breadcrumbs[index].Active = false;
                        if (index > 0)
                            this._breadcrumbs[index - 1].Complete = false;
                    }
                }
                else if (breadcrumb.Page == destinationPage)
                {
                    this._breadcrumbs[index].Active = true;
                    if (movingNext)
                    {
                        if (index > 0)
                            this._breadcrumbs[index - 1].Complete = true;
                    }
                    else
                        this._breadcrumbs[index].Complete = false;
                }
            }
        }
    }
}
