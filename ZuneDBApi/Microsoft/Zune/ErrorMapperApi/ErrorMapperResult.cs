namespace Microsoft.Zune.ErrorMapperApi
{
	public class ErrorMapperResult
	{
		private int hr;

		private string strDescription;

		private string strWebHelpUrl;

		public string WebHelpUrl
		{
			get
			{
				return strWebHelpUrl;
			}
			set
			{
				strWebHelpUrl = value;
			}
		}

		public string Description
		{
			get
			{
				return strDescription;
			}
			set
			{
				strDescription = value;
			}
		}

		public int Hr
		{
			get
			{
				return hr;
			}
			set
			{
				hr = value;
			}
		}
	}
}
