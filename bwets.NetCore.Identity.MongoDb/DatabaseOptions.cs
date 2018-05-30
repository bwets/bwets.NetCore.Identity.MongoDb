namespace bwets.NetCore.Identity.MongoDb
{
	public class DatabaseOptions
	{
		public string ConnectionString { get; set; }
		public string UsersCollection { get; set; } = "Users";
		public string RolesCollection { get; set; } = "Roles";
	}
}