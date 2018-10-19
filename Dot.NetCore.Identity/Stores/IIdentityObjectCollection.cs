using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dot.NetCore.Identity.Model;

namespace Dot.NetCore.Identity.Stores
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