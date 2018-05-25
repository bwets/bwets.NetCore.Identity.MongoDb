# Microsoft.AspNetCore.Identity.Mongo

forked from matteofabbri/AspNetCore.Identity.Mongo

# Usage

    services.AddMongoIdentityProvider<ApplicationUser, ApplicationRole>(options =>
    {
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            
    }, dbOptions => {
		dbOptions.ConnectionString = "mongodb://localhost/yourDatabase";
		dbOptions.UsersCollection = "Users"; // this is the default value;
		dbOptions.RolesCollection = "Roles"; // this is the default value;
		
	});
    
