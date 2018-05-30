using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using RestSharp;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public abstract class RestServiceProxy
	{
		private readonly RestClient _proxy;
		private readonly string _serviceName;

		protected RestServiceProxy(Uri baseUri, string serviceName)
		{
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
			var request = new RestRequest(RelativeUrl(relativeUri), method);

			configureRequest(request);

			var response = await _proxy.ExecuteTaskAsync<T>(request);
			if (response.IsSuccessful) return response.Data;
			throw new ApplicationException(response.ErrorMessage);
		}
	}

	public class IdentityObjectCollectionProxy<TItem> : RestServiceProxy, IIdentityObjectCollection<TItem>
		where TItem : IdentityObject
	{
		public IdentityObjectCollectionProxy(Uri baseUri, string service) : base(baseUri, service)
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