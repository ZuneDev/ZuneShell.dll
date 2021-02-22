// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertyEditProfileImage
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Messaging;
using System;

namespace ZuneUI
{
    public class PropertyEditProfileImage : NotifyPropertyChangedImpl
    {
        private ProfileImage m_lastImage;

        public event EventHandler CommitComplete;

        public ProfileImage LastImage
        {
            get => this.m_lastImage;
            private set
            {
                if (this.m_lastImage == value)
                    return;
                this.m_lastImage = value;
                this.FirePropertyChanged(nameof(LastImage));
            }
        }

        public void Commit(ProfileImage image)
        {
            if (ComposerHelper.ManageProfileImage(image, new MessagingCallback(this.OnCommitComplete), image))
                return;
            this.OnCommitComplete(HRESULT._E_FAIL, image);
        }

        private void OnCommitCompleteDeferred(object args)
        {
            HRESULT hresult = (HRESULT)((object[])args)[0];
            ProfileImage profileImage = ((object[])args)[1] as ProfileImage;
            if (this.CommitComplete != null)
                this.CommitComplete(this, null);
            this.FirePropertyChanged("CommitComplete");
            if (hresult.IsError)
            {
                this.LastImage = null;
                Shell.ShowErrorDialog(hresult.Int, Shell.LoadString(StringId.IDS_PROFILE_EDIT_IMAGE_FAIL_TITLE));
            }
            else
                this.LastImage = profileImage;
        }

        private void OnCommitComplete(HRESULT hr, object state) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnCommitCompleteDeferred), new object[2]
        {
       hr,
      state
        });
    }
}
