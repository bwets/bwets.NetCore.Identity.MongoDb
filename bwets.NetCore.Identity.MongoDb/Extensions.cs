using System;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace bwets.NetCore.Identity.MongoDb
{
	public static class Extensions
	{
		public static IServiceCollection AddIdentityMongoDbProviderWrapper<TUser, TRole>(this IServiceCollection services,
			Action<DatabaseOptions> setupDatabaseAction) where TUser : IdentityUser
			where TRole : IdentityRole
		{
			//services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x=>{}))
			//    .AddRoleStore<RoleStore<TRole>>()
			//    .AddUserStore<UserStore<TUser,TRole>>()
			//    .AddDefaultTokenProviders();
			var dbOptions = new DatabaseOptions();
			setupDatabaseAction(dbOptions);
			//var userCollection = new IdentityUserCollection<TUser>(dbOptions.ConnectionString, dbOptions.UsersCollection);
			//var roleCollection = new IdentityRoleCollection<TRole>(dbOptions.ConnectionString, dbOptions.RolesCollection);


			// Identity Services
			//services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser,TRole>(userCollection,roleCollection));
			//services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));
			services.AddSingleton<IIdentityUserCollection<TUser>>(provider =>
				new IdentityUserCollection<TUser>(dbOptions.ConnectionString, dbOptions.UsersCollection));
			services.AddSingleton<IIdentityRoleCollection<TRole>>(provider =>
				new IdentityRoleCollection<TRole>(dbOptions.ConnectionString, dbOptions.RolesCollection));

			return services;
		}

		public static IServiceCollection AddIdentityMongoDbProvider<TUser, TRole>(this IServiceCollection services,
			Action<IdentityOptions> setupIdentityAction, Action<DatabaseOptions> setupDatabaseAction) where TUser : IdentityUser
			where TRole : IdentityRole
		{
			services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x => { }))
				.AddRoleStore<RoleStore<TRole>>()
				.AddUserStore<UserStore<TUser, TRole>>()
				.AddDefaultTokenProviders();
			var dbOptions = new DatabaseOptions();
			setupDatabaseAction(dbOptions);
			var userCollection = new IdentityUserCollection<TUser>(dbOptions.ConnectionString, dbOptions.UsersCollection);
			var roleCollection = new IdentityRoleCollection<TRole>(dbOptions.ConnectionString, dbOptions.RolesCollection);


			// Identity Services
			services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(userCollection, roleCollection));
			services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));
			return services;
		}
	}
}