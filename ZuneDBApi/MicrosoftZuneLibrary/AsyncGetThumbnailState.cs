using System;
using System.Runtime.InteropServices;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	internal class AsyncGetThumbnailState : IDisposable
	{
		public LibraryDataProviderItemBase listItem;

		public string thumbnailPropertyName;

		public int thumbnailIndex;

		public bool slowDataQuery;

		public Image thumbnail;

		public string strUrl;

		public int MediaId;

		public bool antialiasImageEdges;

		public bool isComplete;

		public AsyncGetThumbnailState(LibraryDataProviderItemBase item)
		{
			listItem = item;
			isComplete = false;
		}

		private void _007EAsyncGetThumbnailState()
		{
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (!P_0)
			{
				//Finalize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
