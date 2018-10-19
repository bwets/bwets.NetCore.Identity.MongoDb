﻿using Microsoft.AspNetCore.Identity;

namespace Dot.NetCore.Identity.Model
{
	public class IdentityUserLogin
	{
		public IdentityUserLogin()
		{
		}

		public IdentityUserLogin(string loginProvider, string providerKey, string providerDisplayName)
		{
			LoginProvider = loginProvider;
			ProviderDisplayName = providerDisplayName;
			ProviderKey = providerKey;
		}

		public string UserId { get; set; }
		public string LoginProvider { get; set; }
		public string ProviderDisplayName { get; set; }
		public string ProviderKey { get; set; }

		public UserLoginInfo ToUserLoginInfo()
		{
			return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
		}
	}
}