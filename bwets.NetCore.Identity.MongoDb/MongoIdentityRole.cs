using System;
using bwets.NetCore.Identity.MongoDb.Stores;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace bwets.NetCore.Identity.MongoDb
{
	public class MongoIdentityRole : ICollectionItem<Guid>
	{
		public MongoIdentityRole()
		{
		}

		public MongoIdentityRole(string name)
		{
			Name = name;
			NormalizedName = name.ToUpperInvariant();
		}

		public string Name { get; set; }
		public string NormalizedName { get; set; }

		[BsonId(IdGenerator = typeof(GuidGenerator))]
		public Guid Id { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}