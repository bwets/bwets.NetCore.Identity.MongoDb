using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class IdentityUserCollectionProxy<TUser> : IdentityObjectCollectionProxy<TUser>, IIdentityUserCollection<TUser>
		where TUser : IdentityUser
	{
		public IdentityUserCollectionProxy(ILogger logger, Uri baseUrl) : base(logger, baseUrl, "security/users")
		{
		}


		public async Task<TUser> FindByEmailAsync(string email)
		{
			return await Execute<TUser>("byEmail/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(email), email);
			});
		}

		public async Task<TUser> FindByUserNameAsync(string username)
		{
			return await Execute<TUser>("byUsername/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(username), username);
			});
		}

		public async Task<TUser> FindByNormalizedUserNameAsync(string normalizedUsername)
		{
			return await Execute<TUser>("byNormalizedUsername/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(normalizedUsername), normalizedUsername);
			});
		}

		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
		{
			return await Execute<TUser>("byLogin/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(loginProvider), loginProvider);
				r.AddParameter(nameof(providerKey), providerKey);
			});
		}

		public async Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue)
		{
			return await Execute<IEnumerable<TUser>>("byClaim/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(claimType), claimType);
				r.AddParameter(nameof(claimValue), claimValue);
			});
		}

		public async Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName)
		{
			return await Execute<IEnumerable<TUser>>("byRole/", r =>
			{
				r.Method = Method.POST;
				r.AddParameter(nameof(roleName), roleName);
			});
		}
	}
}