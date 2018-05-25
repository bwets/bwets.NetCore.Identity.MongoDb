using System;
using bwets.NetCore.Identity.MongoDb.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace bwets.NetCore.Identity.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoIdentityProvider(this IServiceCollection services)
        {
            return AddMongoIdentityProvider<MongoIdentityUser, MongoIdentityRole>(services, null, null);
        }

        public static IServiceCollection AddMongoIdentityProvider<TUser>(this IServiceCollection services) where TUser : MongoIdentityUser
        {
            return AddMongoIdentityProvider<TUser, MongoIdentityRole>(services, null, null);
        }

        public static IServiceCollection AddMongoIdentityProvider<TUser, TRole>(this IServiceCollection services) where TUser : MongoIdentityUser
            where TRole : MongoIdentityRole
        {
            return AddMongoIdentityProvider<TUser, TRole>(services, null, null);
        }

        public static IServiceCollection AddMongoIdentityProvider<TUser, TRole>(this IServiceCollection services,
            Action<IdentityOptions> setupIdentityAction, Action<DatabaseOptions> setupDatabaseAction) where TUser : MongoIdentityUser
            where TRole : MongoIdentityRole
        {
            return AddMongoIdentityProvider<TUser, TRole>(services, null, setupIdentityAction,setupDatabaseAction);
        }

        public static IServiceCollection AddMongoIdentityProvider<TUser,TRole>(this IServiceCollection services, string connectionString, Action<IdentityOptions> setupIdentityAction, Action<DatabaseOptions> setupDatabaseAction) where TUser : MongoIdentityUser
                                                                                                                                                                 where TRole : MongoIdentityRole
        {
            services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x=>{}))
                .AddRoleStore<RoleStore<TRole>>()
                .AddUserStore<UserStore<TUser,TRole>>()
                .AddDefaultTokenProviders();
			var dbOptions =new DatabaseOptions();
	        setupDatabaseAction(dbOptions);
            var userCollection = new Collection<Guid,TUser>(dbOptions.ConnectionString, dbOptions.UsersCollection);
            var roleCollection = new Collection<Guid,TRole>(dbOptions.ConnectionString, dbOptions.RolesCollection);


            // Identity Services
            services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser,TRole>(userCollection,roleCollection));
            services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));

            return services;
        }
    }
}
