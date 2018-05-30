using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;

namespace bwets.NetCore.Identity.Stores
{
	public interface IIdentityRoleCollection<TRole> : IIdentityObjectCollection<TRole> where TRole : IdentityRole
	{
		Task<TRole> FindByNameAsync(string normalizedName);
	}
}