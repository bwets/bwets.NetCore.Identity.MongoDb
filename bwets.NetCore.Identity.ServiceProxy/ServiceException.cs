using System;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class ServiceException : ApplicationException
	{
		public ServiceException()
		{
		}

		public ServiceException(string message) : base(message)
		{
		}

		public ServiceException(string message, HttpStatusCode statusCode, string statusMessage, string response) :
			base(message)
		{
			StatusCode = statusCode;
			StatusMessage = statusMessage;
			Response = response;
			try
			{
				ResponseObject = JsonConvert.DeserializeObject(Response);
			}
			catch
			{
				// nothing
			}
		}

		public ServiceException(HttpStatusCode statusCode, string statusMessage) : base(statusCode.ToString())
		{
			StatusCode = statusCode;
			StatusMessage = statusMessage;
		}

		public ServiceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public HttpStatusCode StatusCode { get; set; }
		public string StatusMessage { get; set; }
		public string Response { get; set; }
		public object ResponseObject { get; set; }
	}
}