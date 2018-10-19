using System.Collections.Generic;
using System.Threading.Tasks;
using Dot.NetCore.Identity.Model;

namespace Dot.NetCore.Identity.Stores
{
	public interface IIdentityUserCollection<TUser> : IIdentityObjectCollection<TUser> where TUser : IdentityUser
	{
		Task<TUser> FindByEmailAsync(string normalizedEmail);
		Task<TUser> FindByUserNameAsync(string username);
		Task<TUser> FindByNormalizedUserNameAsync(string normalizedUserName);
		Task<TUser> FindByLoginAsync(string loginProvider, string providerKey);
		Task<IEnumerable<TUser>> FindUsersByClaimAsync(string claimType, string claimValue);
		Task<IEnumerable<TUser>> FindUsersInRoleAsync(string roleName);
	}
}