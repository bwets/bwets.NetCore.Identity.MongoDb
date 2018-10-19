using System.Threading.Tasks;
using Dot.NetCore.Identity.Model;

namespace Dot.NetCore.Identity.Stores
{
	public interface IIdentityRoleCollection<TRole> : IIdentityObjectCollection<TRole> where TRole : IdentityRole
	{
		Task<TRole> FindByNameAsync(string normalizedName);
	}
}