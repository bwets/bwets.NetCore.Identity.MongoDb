using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bwets.NetCore.Identity.Service
{
	public abstract class BaseUsersController<TUser> : BaseApiController, IIdentityUserCollection<TUser>
		where TUser : IdentityUser
	{
		private readonly IIdentityUserCollection<TUser> _Collection;

		protected BaseUsersController(ILogger log, IIdentityUserCollection<TUser> collection) : base(log)
		{
			_Collection = collection;
		}

		[HttpGet]
		public async Task<IEnumerable<TUser>> GetAll()
		{
			return await ExecuteWithLog(async () => await _Collection.GetAll());
		}

		[HttpPut]
		public async Task<TUser> CreateAsync(TUser obj)
		{
			return await ExecuteWithLog(async () => await _Collection.CreateAsync(obj));
		}

		[HttpPost]
		public async Task UpdateAsync(TUser obj)
		{
			await ExecuteWithLog(async () => await _Collection.UpdateAsync(obj));
		}

		[HttpDelete]
		public async Task DeleteAsync(TUser obj)
		{
			await ExecuteWithLog(async () => await _Collection.DeleteAsync(obj));
		}

		[HttpGet("{itemId}")]
		public async Task<TUser> FindByIdAsync(Guid itemId)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByIdAsync(itemId));
		}

		[HttpPost("byEmail")]
		public async Task<TUser> FindByEmailAsync(string normalizedEmail)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByEmailAsync(normalizedEmail));
		}

		[HttpPost("byUsername")]
		public async Task<TUser> FindByUserNameAsync(string username)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByUserNameAsync(username));
		}

		[HttpPost("byNormalizedUsername")]
		public async Task<TUser> FindByNormalizedUserNameAsync(string normalizedUsername)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByNormalizedUserNameAsync(normalizedUsername));
		}

		[HttpPost("byLogin")]
		public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey)
		{
			return await ExecuteWithLog(async () => await _Collection.FindByLoginAsync(loginProvider, providerKey));
		}

		[HttpPost("byClaim")]
		public async Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue)
		{
			return await ExecuteWithLog(async () => await _Collection.FindUsersByClaimAsync(claimType, claimValue));
		}

		[HttpPost("byRole")]
		public async Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName)
		{
			return await ExecuteWithLog(async () => await _Collection.FindUsersInRoleAsync(roleName));
		}
	}
}