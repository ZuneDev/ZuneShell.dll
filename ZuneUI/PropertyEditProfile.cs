// Decompiled with JetBrains decompiler
// Type: ZuneUI.PropertyEditProfile
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using Microsoft.Zune.Messaging;
using System.Collections;

namespace ZuneUI
{
    public class PropertyEditProfile : MetadataEditMedia
    {
        private DataProviderObject _profile;
        private static PropertyDescriptor[] s_dataProviderProperties;
        public static ProfileMultiLinePropertyDescriptor s_Biography = new ProfileMultiLinePropertyDescriptor(nameof(Biography), "bio", StringId.IDS_PROFILE_EDIT_BIO, 300);
        public static ProfilePropertyDescriptor s_DisplayName = new ProfilePropertyDescriptor(nameof(DisplayName), "displayname", StringId.IDS_PROFILE_EDIT_DISPLAYNAME, 15);
        public static ProfilePropertyDescriptor s_Location = new ProfilePropertyDescriptor(nameof(Location), "location", StringId.IDS_PROFILE_EDIT_LOCATION, 30);
        public static ProfilePropertyDescriptor s_Status = new ProfilePropertyDescriptor(nameof(Status), "status", StringId.IDS_PROFILE_EDIT_STATUS, 60);

        public PropertyEditProfile(DataProviderObject profile)
        {
            this._source = DataProviderObjectPropertySource.Instance;
            this.Initialize(profile);
        }

        protected void Initialize(DataProviderObject profile)
        {
            this._profile = profile;
            if (s_dataProviderProperties == null)
                s_dataProviderProperties = new PropertyDescriptor[4]
                {
           s_Biography,
           s_DisplayName,
           s_Location,
           s_Status
                };
            this.Initialize(new object[1]
            {
         _profile
            }, s_dataProviderProperties);
        }

        public override void Commit()
        {
            base.Commit();
            foreach (ProfilePropertyDescriptor providerProperty in s_dataProviderProperties)
            {
                MetadataEditProperty property = this.GetProperty(providerProperty);
                if (property.Modified)
                    ComposerHelper.ManageProfile(providerProperty.ServiceName, providerProperty.GetServiceValue(property.Value), new MessagingCallback(this.OnCommitComplete), property);
            }
        }

        public void Undo()
        {
            foreach (PropertyDescriptor providerProperty in s_dataProviderProperties)
            {
                MetadataEditProperty property = this.GetProperty(providerProperty);
                property.Value = property.OriginalValue;
                property.Modified = false;
            }
        }

        public DataProviderObject Profile => this._profile;

        private void OnCommitCompleteDeferred(object args)
        {
            HRESULT hresult = (HRESULT)((object[])args)[0];
            if (!(((object[])args)[1] is MetadataEditProperty metadataEditProperty))
                return;
            if (hresult.IsSuccess)
            {
                metadataEditProperty.OriginalValue = metadataEditProperty.Value;
            }
            else
            {
                if (metadataEditProperty.Descriptor is ProfilePropertyDescriptor)
                {
                    string title = Shell.LoadString(((ProfilePropertyDescriptor)metadataEditProperty.Descriptor).DisplayNameId);
                    ErrorDialogInfo.Show(hresult.Int, title);
                }
                metadataEditProperty.Value = metadataEditProperty.OriginalValue;
            }
            metadataEditProperty.Modified = false;
        }

        private void OnCommitComplete(HRESULT hr, object state) => Application.DeferredInvoke(new DeferredInvokeHandler(this.OnCommitCompleteDeferred), new object[2]
        {
       hr,
      state
        });

        public static ProfileMultiLinePropertyDescriptor Biography => s_Biography;

        public static ProfilePropertyDescriptor DisplayName => s_DisplayName;

        public static ProfilePropertyDescriptor Location => s_Location;

        public static ProfilePropertyDescriptor Status => s_Status;
    }
}
