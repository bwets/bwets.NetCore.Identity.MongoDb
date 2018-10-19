using System;
using System.Threading.Tasks;
using Dot.NetCore.Identity.Model;
using Dot.NetCore.Identity.Stores;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Dot.NetCore.Identity.ServiceProxy
{
	public class IdentityRoleCollectionProxy<TRole> : IdentityObjectCollectionProxy<TRole>, IIdentityRoleCollection<TRole>
		where TRole : IdentityRole
	{
		public IdentityRoleCollectionProxy(ILogger logger, Uri baseUri) : base(logger, baseUri, "security/roles")
		{
		}

		public async Task<TRole> FindByNameAsync(string normalizedName)
		{
			return await Execute<TRole>("byName/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(normalizedName), normalizedName);
			});
		}
	}
}