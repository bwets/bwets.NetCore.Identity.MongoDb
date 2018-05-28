﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;

namespace bwets.NetCore.Identity.MongoDb
{
	public class IdentityUserCollection<TUser> : IdentityObjectCollection<TUser>, IIdentityUserCollection<TUser>
		where TUser : IdentityUser
	{
		public IdentityUserCollection(string connectionString, string collectionName) : base(connectionString, collectionName)
		{
		}


		public async Task<TUser> FindByEmailAsync(string normalizedEmail)
		{
			return await this.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
		}

		public async Task<TUser> FindByUserNameAsync(string userUserName)
		{
			return await this.FirstOrDefaultAsync(u => u.UserName == userUserName);
		}

		public async Task<TUser> FindByNormalizedUserNameAsync(string normalizedUserName)
		{
			return await this.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);
		}

		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
		{
			return await this.FirstOrDefaultAsync(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
		}

		public async Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue)
		{
			return await this.WhereAsync(u =>u.Claims.Any(c => c.Type == claimType && c.Value == claimValue));
		}

		public async Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName)
		{
			return await this.AnyEqualAsync(x => x.Roles, roleName);
		}
	}
}