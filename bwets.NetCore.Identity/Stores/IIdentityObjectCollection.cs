using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;

namespace bwets.NetCore.Identity.Stores
{
	public interface IIdentityObjectCollection<TItem> where TItem : IdentityObject
	{
		Task<IEnumerable<TItem>> GetAll();
		Task<TItem> CreateAsync(TItem obj);
		Task UpdateAsync(TItem obj);
		Task DeleteAsync(TItem obj);
		Task<TItem> FindByIdAsync(Guid itemId);
	}
}