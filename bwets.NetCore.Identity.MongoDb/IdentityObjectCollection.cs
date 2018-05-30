using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using bwets.NetCore.Identity.Model;
using bwets.NetCore.Identity.Stores;
using MongoDB.Driver;

namespace bwets.NetCore.Identity.MongoDb
{
	public class IdentityObjectCollection<TItem> : IIdentityObjectCollection<TItem> where TItem : IdentityObject
	{
		public IdentityObjectCollection(string connectionString, string collectionName)
		{
			var type = typeof(TItem);

			if (connectionString != null)
			{
				var url = new MongoUrl(connectionString);
				var client = new MongoClient(connectionString);
				MongoCollection = client.GetDatabase(url.DatabaseName ?? "default")
					.GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}
			else
			{
				MongoCollection = new MongoClient().GetDatabase("default")
					.GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}
		}

		private IMongoCollection<TItem> MongoCollection { get; }

		public async Task<TItem> FindByIdAsync(Guid itemId)
		{
			return await FirstOrDefaultAsync(item => item.Id == itemId);
		}


		public async Task<IEnumerable<TItem>> GetAll()
		{
			return MongoCollection.AsQueryable();
		}

		public async Task<TItem> CreateAsync(TItem obj)
		{
			await MongoCollection.InsertOneAsync(obj);
			return obj;
		}


		public async Task UpdateAsync(TItem obj)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			await MongoCollection.ReplaceOneAsync(filter, obj);
		}

		public async Task DeleteAsync(TItem obj)
		{
			await MongoCollection.DeleteOneAsync(Builders<TItem>.Filter.Eq(x => x.Id, obj.Id));
		}


		public IQueryable<TItem> Queryable()
		{
			return MongoCollection.AsQueryable();
		}


		public bool Any(Expression<Func<TItem, bool>> p)
		{
			return MongoCollection.Find(p).Any();
		}


		public async Task<IEnumerable<TItem>> AnyEqualAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyEq(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public async Task DeleteAsync(Expression<Func<TItem, bool>> p)
		{
			await MongoCollection.DeleteManyAsync(p);
		}


		public TItem FirstOrDefault()
		{
			return MongoCollection.Find(Builders<TItem>.Filter.Empty).FirstOrDefault();
		}

		public TItem FirstOrDefault(Expression<Func<TItem, bool>> p)
		{
			return MongoCollection.Find(p).FirstOrDefault();
		}

		public async Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> p)
		{
			return await (await MongoCollection.FindAsync(p)).FirstOrDefaultAsync();
		}


		public async Task<IEnumerable<TItem>> WhereAsync(Expression<Func<TItem, bool>> p)
		{
			return (await MongoCollection.FindAsync(p)).ToEnumerable();
		}
	}
}