using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	internal class UnavailableLibraryListItem : LibraryDataProviderListItem
	{
		internal UnavailableLibraryListItem(LibraryDataProviderQuery owner, LibraryVirtualList listOwner, object typeCookie, int index)
			: base(owner, listOwner, typeCookie, -1, index)
		{
		}

		public override object GetProperty(string propertyName)
		{
			DataProviderMapping value = null;
			if (base.Mappings.TryGetValue(propertyName, out value))
			{
				return value.DefaultValue;
			}
			return null;
		}

		public override void SetProperty(string propertyName, object value)
		{
		}

		public override void InvalidateAllProperties()
		{
		}

		protected internal override void OnRequestSlowData()
		{
			GetOwner().NotifySlowDataAcquireComplete(GetIndex());
		}
	}
}
