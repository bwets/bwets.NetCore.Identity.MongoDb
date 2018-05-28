﻿using System;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public class ServiceOptions
	{
		public string BaseUrl { get; set; }
	}

	public static class Extensions
	{
		public static IServiceCollection AddIdentityServiceProvider(this IServiceCollection services)
		{
			return AddIdentityServiceProvider<IdentityUser, IdentityRole>(services, null, null);
		}

		public static IServiceCollection AddIdentityServiceProvider<TUser>(this IServiceCollection services)
			where TUser : IdentityUser
		{
			return AddIdentityServiceProvider<TUser, IdentityRole>(services, null, null);
		}

		public static IServiceCollection AddIdentityServiceProvider<TUser, TRole>(this IServiceCollection services)
			where TUser : IdentityUser
			where TRole : IdentityRole
		{
			return AddIdentityServiceProvider<TUser, TRole>(services, null, null);
		}

		public static IServiceCollection AddIdentityServiceProvider<TUser, TRole>(this IServiceCollection services,
			Action<IdentityOptions> setupIdentityAction, Action<ServiceOptions> setupServiceAction) where TUser : IdentityUser
			where TRole : IdentityRole
		{
			return AddIdentityServiceProvider<TUser, TRole>(services, null, setupIdentityAction, setupServiceAction);
		}

		public static IServiceCollection AddIdentityServiceProvider<TUser, TRole>(this IServiceCollection services,
			string connectionString, Action<IdentityOptions> setupIdentityAction, Action<ServiceOptions> setupServiceAction)
			where TUser : IdentityUser
			where TRole : IdentityRole
		{
			services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x => { }))
				.AddRoleStore<RoleStore<TRole>>()
				.AddUserStore<UserStore<TUser, TRole>>()
				.AddDefaultTokenProviders();
			var options = new ServiceOptions();
			setupServiceAction(options);
			var userCollection = new IdentityUserCollection<TUser>(new Uri(options.BaseUrl));
			var roleCollection = new IdentityRoleCollection<TRole>(new Uri(options.BaseUrl));


			// Identity Services
			services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(userCollection, roleCollection));
			services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roleCollection));

			return services;
		}
	}
}