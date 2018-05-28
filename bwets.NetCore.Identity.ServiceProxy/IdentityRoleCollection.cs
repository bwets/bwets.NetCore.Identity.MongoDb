using System;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using RestSharp;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class IdentityRoleCollection<TRole> : IdentityObjectCollection<TRole>, IIdentityRoleCollection<TRole>
		where TRole : IdentityRole
	{
		public IdentityRoleCollection(Uri baseUri) : base(baseUri, "Roles")
		{
		}

		public async Task<TRole> FindByNameAsync(string normalizedName)
		{
			var request = new RestRequest(RelativeUrl("byName/{name}"), Method.GET);
			request.AddParameter("name", normalizedName);
			var response = await Proxy.ExecuteGetTaskAsync<TRole>(request);
			return response.Data;
		}

	}
}