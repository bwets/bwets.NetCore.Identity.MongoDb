using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;

namespace bwets.NetCore.Identity.Stores
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