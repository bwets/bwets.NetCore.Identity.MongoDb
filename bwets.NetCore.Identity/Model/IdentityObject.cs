using System;

namespace bwets.NetCore.Identity.Model
{
	public abstract class IdentityObject
	{
		public Guid Id { get; set; } = Guid.NewGuid();
	}
}