using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using RestSharp;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class IdentityUserCollection<TUser> : IdentityObjectCollection<TUser>, IIdentityUserCollection<TUser>
		where TUser : IdentityUser
	{
		public IdentityUserCollection(Uri baseUrl) : base(baseUrl, "Users")
		{
		}


		public async Task<TUser> FindByEmailAsync(string normalizedEmail)
		{
			var request = new RestRequest(RelativeUrl("byEmail/{email}"), Method.GET);
			request.AddParameter("email", normalizedEmail);
			var response = await Proxy.ExecuteGetTaskAsync<TUser>(request);
			return response.Data;

		}

		public async Task<TUser> FindByUserNameAsync(string userUserName)
		{
			var request = new RestRequest(RelativeUrl("byUserName/{userName}"), Method.GET);
			request.AddParameter("userName", userUserName);
			var response = await Proxy.ExecuteGetTaskAsync<TUser>(request);
			return response.Data;

		}

		public async Task<TUser> FindByNormalizedUserNameAsync(string normalizedUserName)
		{
			var request = new RestRequest(RelativeUrl("byNormalizedUserName/{normalizedUserName}"), Method.GET);
			request.AddParameter("normalizedUserName", normalizedUserName);
			var response = await Proxy.ExecuteGetTaskAsync<TUser>(request);
			return response.Data;
		}

		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
		{
			var request = new RestRequest(RelativeUrl("byLogin/{provider}/{key"), Method.GET);
			request.AddParameter("provider", loginProvider);
			request.AddParameter("key", providerKey);
			var response = await Proxy.ExecuteGetTaskAsync<TUser>(request);
			return response.Data;
		}

		public async Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue)
		{
			var request = new RestRequest(RelativeUrl("byClaims/{type}/{value"), Method.GET);
			request.AddParameter("type", claimType);
			request.AddParameter("value", claimValue);
			var response = await Proxy.ExecuteGetTaskAsync<IEnumerable<TUser>>(request);
			return response.Data;
		}

		public async Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName)
		{
			var request = new RestRequest(RelativeUrl("byRole/{role}"), Method.GET);
			request.AddParameter("role", roleName);
			var response = await Proxy.ExecuteGetTaskAsync<IEnumerable<TUser>>(request);
			return response.Data;
		}
	}
}