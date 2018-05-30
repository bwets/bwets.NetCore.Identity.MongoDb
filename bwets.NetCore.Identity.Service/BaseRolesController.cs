using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bwets.NetCore.Identity.Service
{
	public abstract class BaseRolesController<TRole> : BaseApiController, IIdentityRoleCollection<TRole>
		where TRole : IdentityRole
	{
		private readonly IIdentityRoleCollection<TRole> _Collection;

		protected BaseRolesController(ILogger log, IIdentityRoleCollection<TRole> collection) : base(log)
		{
			_Collection = collection;
		}

		[HttpGet]
		public async Task<IEnumerable<TRole>> GetAll()
		{
			return await ExecuteWithLog(async () => await _Collection.GetAll());
		}

		[HttpPut]
		public async Task<TRole> CreateAsync(TRole obj)
		{
			return await ExecuteWithLog(async () => await _Collection.CreateAsync(obj));
		}

		[HttpPost]
		public async Task UpdateAsync(TRole obj)
		{
			await ExecuteWithLog(async () => await _Collection.UpdateAsync(obj));
		}

		[HttpDelete]
		public async Task DeleteAsync(TRole obj)
		{
			await ExecuteWithLog(async () => await _Collection.DeleteAsync(obj));
		}

		[HttpGet("{itemId}")]
		public async Task<TRole> FindByIdAsync(Guid itemId)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByIdAsync(itemId));
		}

		[HttpPost("byName")]
		public async Task<TRole> FindByNameAsync(string normalizedName)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByNameAsync(normalizedName));
		}
	}
}