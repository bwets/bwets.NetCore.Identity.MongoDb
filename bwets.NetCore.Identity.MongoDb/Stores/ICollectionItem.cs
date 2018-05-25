namespace bwets.NetCore.Identity.MongoDb.Stores
{
	public interface ICollectionItem<TKey>
	{
		TKey Id { get; set; }
	}
}