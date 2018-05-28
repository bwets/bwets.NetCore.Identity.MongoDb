using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using RestSharp;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class IdentityObjectCollection<TItem> : IIdentityObjectCollection<TItem> where TItem : IdentityObject
	{
		protected RestClient Proxy { get; }
		private readonly string _Service;

		public IdentityObjectCollection(Uri baseUri, string service)
		{
			Proxy = new RestClient(baseUri);
			_Service = service;
		}

		public async Task<IEnumerable<TItem>> GetAll()
		{
			var request = new RestRequest(_Service, Method.GET);
			var response = await Proxy.ExecuteGetTaskAsync<IEnumerable<TItem>>(request);
			return response.Data;

		}
		public async Task<TItem> CreateAsync(TItem obj)
		{
			var request = new RestRequest(_Service, Method.PUT);
			request.AddObject(obj);
			await Proxy.ExecuteTaskAsync(request);
			return obj;

		}

		public async Task UpdateAsync(TItem obj)
		{
			var request = new RestRequest(_Service, Method.POST);
			request.AddObject(obj);
			await Proxy.ExecuteTaskAsync(request);
		}

		public async Task DeleteAsync(TItem obj)
		{
			var request = new RestRequest(_Service, Method.DELETE);
			request.AddObject(obj);
			await Proxy.ExecuteTaskAsync(request);
		}

		public async Task<TItem> FindByIdAsync(Guid itemId)
		{
			var request = new RestRequest(RelativeUrl("{id}"), Method.GET);
			request.AddParameter("id", itemId);
			var response = await Proxy.ExecuteGetTaskAsync<TItem>(request);
			return response.Data;

		}

		protected string RelativeUrl(string query)
		{
			return $"{_Service}/{query}";
		}
	}
}