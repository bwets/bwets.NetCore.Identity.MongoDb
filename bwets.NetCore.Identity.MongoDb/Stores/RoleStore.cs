using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bwets.NetCore.Identity.MongoDb.Stores
{
	public class RoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : MongoIdentityRole
    {
        private readonly Collection<Guid,TRole> _collection;

        public RoleStore(Collection<Guid,TRole> collection)
        {
            _collection = collection;
        }

        IQueryable<TRole> IQueryableRoleStore<TRole>.Roles => _collection.Queryable();

        async Task<IdentityResult> IRoleStore<TRole>.CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (!_collection.Any(x => x.NormalizedName == role.NormalizedName)) await _collection.CreateAsync(role);

            return IdentityResult.Success;
        }

        async Task<IdentityResult> IRoleStore<TRole>.UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _collection.UpdateAsync(role);
            return IdentityResult.Success;
        }

        async Task<IdentityResult> IRoleStore<TRole>.DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            await _collection.DeleteAsync(role);
            return IdentityResult.Success;
        }

        async Task<string> IRoleStore<TRole>.GetRoleIdAsync(TRole role, CancellationToken cancellationToken) => (await Task.FromResult(role.Id)).ToString();

	    async Task<string> IRoleStore<TRole>.GetRoleNameAsync(TRole role, CancellationToken cancellationToken) => await Task.FromResult(role.Name);

	    async Task IRoleStore<TRole>.SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await _collection.UpdateAsync(role, x => x.Name, roleName);
        }

        async Task<string> IRoleStore<TRole>.GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken) => await Task.FromResult(role.NormalizedName);

	    async Task IRoleStore<TRole>.SetNormalizedRoleNameAsync(TRole role, string normalizedName,
            CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            await _collection.UpdateAsync(role, x => x.NormalizedName, normalizedName);
        }

        async Task<TRole> IRoleStore<TRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
			
	        if(!Guid.TryParse(roleId, out Guid id))
	        {
		        throw new ApplicationException("Invalid role Id");
	        }
	        return await _collection.FirstOrDefaultAsync(x => x.Id == id);
	    }

	    async Task<TRole> IRoleStore<TRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => await _collection.FirstOrDefaultAsync(x => x.NormalizedName == normalizedRoleName);

	    void IDisposable.Dispose()
        {
        }
    }
}