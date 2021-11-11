using System;
using System.Net;

namespace Microsoft.Zune.Service
{
	public class HttpWebException : Exception
	{
		private int _hr;

		private HttpWebResponse _response;

		private WebExceptionStatus _status;

		public HttpWebResponse Response => _response;

		public WebExceptionStatus Status => _status;

		public HttpWebException(int hr)
		{
			_hr = hr;
		}

		public HttpWebException(HttpWebResponse response)
		{
			_response = response;
		}

		public HttpWebException(string text, WebExceptionStatus status)
			: base(text)
		{
			_status = status;
		}
	}
}
