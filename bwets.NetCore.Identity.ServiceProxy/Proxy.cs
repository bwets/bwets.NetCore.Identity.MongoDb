using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class Proxy<T>
	{
		public Proxy(Uri baseUri)
		{
			BaseUri = baseUri;
		}

		protected Uri BaseUri { get; set; }
		protected string ServiceUrl { get; set; }

		private HttpClient CreateClient()
		{
			var handler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
				{
					// HACK: do not check ssl cert. 
					// TODO: change this?
					return true;
				}
			};
			var client = new HttpClient(handler);
#if DEBUG
			client.Timeout = new TimeSpan(0, 5, 0);
#endif

			return client;
		}

		private HttpRequestMessage CreateMessage(string relativeUri, HttpMethod method = null)
		{
			method = method ?? HttpMethod.Get;
			if (ServiceUrl.StartsWith("/")) ServiceUrl = ServiceUrl.Substring(1, ServiceUrl.Length - 1);

			var baseServiceUri = !string.IsNullOrEmpty(ServiceUrl) ? new Uri(BaseUri, ServiceUrl) : BaseUri;
			var uri = new Uri(baseServiceUri, relativeUri);
			var message = new HttpRequestMessage(method, uri);
			return message;
		}

		public async Task<TResponse> Execute<TResponse>(string relativeUri, HttpMethod method = null, object content = null,
			object datasourceRequest = null)
		{
			TResponse response;

			using (var client = CreateClient())
			{
				using (var message = CreateMessage(relativeUri, method))
				{
					if (datasourceRequest != null)
						message.Headers.Add("DigitalKYC.Request", JsonConvert.SerializeObject(datasourceRequest));

					if (content != null)
						if (method == HttpMethod.Get || method == HttpMethod.Delete)
						{
							var properties = from p in content.GetType().GetProperties()
								where p.GetValue(content, null) != null
								select string.Concat(p.Name, "=", HttpUtility.UrlEncode(p.GetValue(content, null).ToString()));
							var queryString = string.Join("&", properties.ToArray());
							var url = message.RequestUri.ToString();

							if (url.Contains("?"))
								url += "&" + queryString;
							else
								url += "?" + queryString;

							message.RequestUri = new Uri(url);
						}
						else
						{
							var serializerSettings = new JsonSerializerSettings();
							serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
							serializerSettings.Formatting = Formatting.None;
							var jsonRequest = JsonConvert.SerializeObject(content, serializerSettings);
							message.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
						}

					response = await ProcessResponse<TResponse>(client, message);
				}
			}

			return response;
		}

		private async Task<TResponse> ProcessResponse<TResponse>(HttpClient client, HttpRequestMessage message)
		{
			var responseString = string.Empty;
			var data = await client.SendAsync(message).ConfigureAwait(false);
			if (data.IsSuccessStatusCode) return await data.Content.ReadAsAsync<TResponse>();

			var status = data.StatusCode;
			if (status == HttpStatusCode.NotFound)
				throw new ServiceException("Object not found"); // TODO Specialize the exception

			var reasonPhrase = data.ReasonPhrase;

			if (status == HttpStatusCode.BadRequest) throw new ServiceException(status, reasonPhrase);

			throw new ServiceException("Unexpected error", status, reasonPhrase, responseString);
		}
	}
}