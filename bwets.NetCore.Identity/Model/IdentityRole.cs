namespace bwets.NetCore.Identity.Model
{
	public class IdentityRole : IdentityObject
	{
		public IdentityRole()
		{
		}

		public IdentityRole(string name)
		{
			Name = name;
			NormalizedName = name.ToUpperInvariant();
		}

		public string Name { get; set; }
		public string NormalizedName { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}