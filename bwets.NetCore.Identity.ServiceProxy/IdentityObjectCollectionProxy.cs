using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Deserializers;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public abstract class RestServiceProxy
	{
		static RestServiceProxy()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
		}
		private readonly RestClient _proxy;
		private readonly string _serviceName;
		private readonly ILogger _logger;

		protected RestServiceProxy(ILogger logger, Uri baseUri, string serviceName)
		{
			_logger = logger;
			_proxy = new RestClient(baseUri);
			_serviceName = serviceName;
		}

		private string RelativeUrl(string query)
		{
			return $"{_serviceName}/{query}";
		}

		protected async Task Execute(string relativeUri = null, Method method = Method.GET)
		{
			
			await Execute<string>(relativeUri, _ => { }, method);
		}

		protected async Task Execute(string relativeUri, Action<RestRequest> configureRequest, Method method = Method.GET)
		{
			await Execute<string>(relativeUri, configureRequest, method);
		}

		protected async Task<T> Execute<T>(string relativeUri = null, Method method = Method.GET)
		{
			return await Execute<T>(relativeUri, _ => { }, method);
		}

		protected async Task<T> Execute<T>(string relativeUri, Action<RestRequest> configureRequest,
			Method method = Method.GET)
		{
			var url = RelativeUrl(relativeUri);
			_logger.LogDebug($"Execute {method} on {url}");
			var request = new RestRequest(url, method);
			configureRequest(request);

			try
			{
				var response = await _proxy.ExecuteTaskAsync<T>(request);
				if (response.IsSuccessful) return response.Data;
				_logger.LogError($"Execute completed but response not successfull: {response.ErrorMessage}");
				_logger.LogError("Exception:" + response.ErrorException );
				_logger.LogTrace($"Data was: {Encoding.UTF8.GetString(response.RawBytes)}");
				throw new ApplicationException(response.ErrorMessage, response.ErrorException);
			}
			catch (Exception e)
			{
				_logger.LogError("Execute failed",e);
				throw;
			}
		}
	}

	public class IdentityObjectCollectionProxy<TItem> : RestServiceProxy, IIdentityObjectCollection<TItem>
		where TItem : IdentityObject
	{
		public IdentityObjectCollectionProxy(ILogger logger, Uri baseUri, string service) : base(logger,baseUri, service)
		{
		}

		public async Task<IEnumerable<TItem>> GetAll()
		{
			return await Execute<IEnumerable<TItem>>();
		}

		public async Task<TItem> CreateAsync(TItem obj)
		{
			return await Execute<TItem>(null, r =>
			{
				r.Method = Method.PUT;
				r.AddObject(obj);
			});
		}

		public async Task UpdateAsync(TItem obj)
		{
			await Execute<TItem>(null, r =>
			{
				r.Method = Method.POST;
				r.AddObject(obj);
			});
		}

		public async Task DeleteAsync(TItem obj)
		{
			await Execute(string.Empty, r =>
			{
				r.Method = Method.DELETE;
				r.AddObject(obj);
			});
		}

		public async Task<TItem> FindByIdAsync(Guid itemId)
		{
			return await Execute<TItem>("{id}", r => { r.AddUrlSegment("id", itemId); });
		}
	}
}