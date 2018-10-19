﻿using System;
using System.Threading.Tasks;
using Dot.NetCore.Identity.Model;
using Dot.NetCore.Identity.Stores;

namespace Dot.NetCore.Identity.MongoDb
{
	public class IdentityRoleCollection<TRole> : IdentityObjectCollection<TRole>, IIdentityRoleCollection<TRole>
		where TRole : IdentityRole
	{
		public IdentityRoleCollection(string connectionString, string collectionName) : base(connectionString, collectionName)
		{
		}

		public async Task<TRole> FindByNameAsync(string normalizedName)
		{
			return await FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);
		}

		public async Task<TRole> FindByIdAsync(Guid roleId)
		{
			return await FirstOrDefaultAsync(x => x.Id == roleId);
		}
	}
}