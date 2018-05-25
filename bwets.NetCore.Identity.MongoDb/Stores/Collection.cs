using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace bwets.NetCore.Identity.MongoDb.Stores
{
	public class Collection<TKey, TItem> where TItem: class, ICollectionItem<TKey>
	{
		public Collection(string connectionString, string collectionName)
		{
			var type = typeof(TItem);

			if (connectionString != null)
			{
				var url = new MongoUrl(connectionString);
				var client = new MongoClient(connectionString);
				MongoCollection = client.GetDatabase(url.DatabaseName ?? "default").GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}
			else
			{
				MongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}

           
		}

		private IMongoCollection<TItem> MongoCollection { get; }

		public void FullTextindex(Expression<Func<TItem, object>> func)
		{
			MongoCollection.Indexes.CreateOne(Builders<TItem>.IndexKeys.Text(func));
		}

		public void AscendingIndex(Expression<Func<TItem, object>> func)
		{
			MongoCollection.Indexes.CreateOne(Builders<TItem>.IndexKeys.Ascending(func));
		}

		public void DescendingIndex(Expression<Func<TItem, object>> func)
		{
			MongoCollection.Indexes.CreateOne(Builders<TItem>.IndexKeys.Descending(func));
		}

		public IEnumerable<TItem> FullTextSearch(string text)
		{
			var filter = Builders<TItem>.Filter.Text(text);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}

		public async Task<IEnumerable<TItem>> FullTextSearchAsync(string text)
		{
			var filter = Builders<TItem>.Filter.Text(text);
			var res = await MongoCollection.FindAsync(filter);

			return res.ToEnumerable();
		}

		public TItem Random
		{
			get
			{
				if (Empty) return default(TItem);

				var res = MongoCollection.Find(Builders<TItem>.Filter.Empty);

				var rnd = new Random(Guid.NewGuid().GetHashCode());

				return res.Skip(rnd.Next((int)res.Count() - 1)).FirstOrDefault();
			}
		}

		public IEnumerable<TItem> TakeRandom(int v)
		{
			for (int i = 0; i < v; i++) yield return Random;
		}

		public IEnumerable<TItem> All => MongoCollection.Find(Builders<TItem>.Filter.Empty).ToEnumerable();

		public bool Empty => Count() == 0;

		public IQueryable<TItem> Queryable() => MongoCollection.AsQueryable();

		public IEnumerable<K> Select<K>(Func<TItem, K> sel)
		{
			var filter = Builders<TItem>.Filter.Empty;
			return MongoCollection.Find(filter).ToEnumerable().Select(sel);
		}

		public IEnumerable<TItem> SortBy(Expression<Func<TItem, object>> sel)
		{
			var filter = Builders<TItem>.Filter.Empty;
			var res = MongoCollection.Find(filter).SortBy(sel);
			return res.ToEnumerable();
		}

		public IEnumerable<TItem> SortByDescending(Expression<Func<TItem, object>> sel)
		{
			var filter = Builders<TItem>.Filter.Empty;
			var res = MongoCollection.Find(filter).SortByDescending(sel);
			return res.ToEnumerable();
		}

		public bool Any(Expression<Func<TItem, bool>> p) => MongoCollection.Find(p).Any();

		public async Task<bool> AnyAsync(Expression<Func<TItem, bool>> p)
		{
			var res = await MongoCollection.FindAsync(p);
			return await res.AnyAsync();
		}

		public long Count() => MongoCollection.Count(Builders<TItem>.Filter.Empty);

		public long Count(Expression<Func<TItem, bool>> sel) => MongoCollection.Count(sel);

		public Task<long> CountAsync(Expression<Func<TItem, bool>> sel) => MongoCollection.CountAsync(sel);

		public Task<long> CountAsync() => MongoCollection.CountAsync(Builders<TItem>.Filter.Empty);

		public void Add(TItem obj)
		{
			MongoCollection.InsertOneAsync(obj);
		}

		public async Task<IEnumerable<TItem>> AnyEqualAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyEq(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public async Task<IEnumerable<TItem>> AnyGreaterAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyGt(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public async Task<IEnumerable<TItem>> AnyGreaterOrEqualAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyGte(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}


		public async Task<IEnumerable<TItem>> AnyLowerAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyLt(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public async Task<IEnumerable<TItem>> AnyLowerOrEqualAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyLte(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public IEnumerable<TItem> AnyEqual<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyEq(sel, value);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}

		public IEnumerable<TItem> AnyGreater<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyGt(sel, value);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}

		public IEnumerable<TItem> AnyGreaterOrEqual<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyGte(sel, value);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}


		public IEnumerable<TItem> AnyLower<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyLt(sel, value);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}

		public IEnumerable<TItem> AnyLowerOrEqual<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyLte(sel, value);
			var res = MongoCollection.Find(filter);
			return res.ToEnumerable();
		}


		public TItem Create(TItem obj)
		{
			MongoCollection.InsertOne(obj);
			return obj;
		}

		public async Task<TItem> CreateAsync(TItem obj)
		{
			await MongoCollection.InsertOneAsync(obj);
			return obj;
		}

		public void Replace(TItem obj)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			MongoCollection.ReplaceOne(filter, obj);
		}


		public async Task ReplaceAsync(TItem obj)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			await MongoCollection.ReplaceOneAsync(filter, obj);
		}


		public async Task<TItem> AddToAsync<K>(TItem obj, Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.AddToSet(sel, value);
			await MongoCollection.UpdateOneAsync(filter, update);
			return obj;
		}

		public async Task<TItem> AddToAsync<K>(TItem obj, Expression<Func<TItem, IEnumerable<K>>> sel, IEnumerable<K> value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.AddToSetEach(sel, value);
			await MongoCollection.UpdateOneAsync(filter, update);
			return obj;
		}

		public TItem AddTo<K>(TItem obj, Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.AddToSet(sel, value);
			MongoCollection.UpdateOne(filter, update);
			return obj;
		}

		public async Task UpdateAsync(TItem obj)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			await MongoCollection.ReplaceOneAsync(filter, obj);
		}

		public void Update<K>(TItem obj, Expression<Func<TItem, K>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.Set(sel, value);
			MongoCollection.UpdateOne(filter, update);
		}

		public async Task UpdateAsync<K>(TItem obj, Expression<Func<TItem, K>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.Set(sel, value);
			await MongoCollection.UpdateOneAsync(filter, update);
		}

		public async Task AppendAsync<K>(TItem obj, Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.AddToSet(sel, value);
			await MongoCollection.UpdateOneAsync(filter, update);
		}

		public void Increase<K>(TItem obj, Expression<Func<TItem, K>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.Inc(sel, value);
			MongoCollection.UpdateOne(filter, update);
		}

		public async Task IncreaseAsync<K>(TItem obj, Expression<Func<TItem, K>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			var update = Builders<TItem>.Update.Inc(sel, value);
			await MongoCollection.UpdateOneAsync(filter, update);
		}

		public void Delete(TItem obj)
		{
			MongoCollection.DeleteOne(Builders<TItem>.Filter.Eq(x => x.Id, obj.Id));
		}

		public void Delete(IEnumerable<TItem> obj)
		{
			MongoCollection.DeleteMany(Builders<TItem>.Filter.In(x => x.Id, obj.Select(x => x.Id)));
		}

		public async Task DeleteAsync(Expression<Func<TItem, bool>> p)
		{
			await MongoCollection.DeleteManyAsync(p);
		}

		public async Task DeleteAsync(TItem obj)
		{
			await MongoCollection.DeleteOneAsync(Builders<TItem>.Filter.Eq(x => x.Id, obj.Id));
		}

		public async Task DeleteAsync(IEnumerable<TItem> obj)
		{
			await MongoCollection.DeleteManyAsync(Builders<TItem>.Filter.In(x => x.Id, obj.Select(x => x.Id)));
		}

		public TItem GetOrCreate(Expression<Func<TItem, bool>> p, TItem obj) => FirstOrDefault(p) ?? Create(obj);

		public async Task<TItem> GetOrCreateAsync(Expression<Func<TItem, bool>> p, TItem obj)
		{
			return (await FirstOrDefaultAsync(p)) ?? await CreateAsync(obj);
		}

		public TItem FirstOrDefault() => MongoCollection.Find(Builders<TItem>.Filter.Empty).FirstOrDefault();

		public TItem FirstOrDefault(Expression<Func<TItem, bool>> p) => MongoCollection.Find(p).FirstOrDefault();

		public async Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> p) => await (await MongoCollection.FindAsync(p)).FirstOrDefaultAsync();

		public TItem First(Expression<Func<TItem, bool>> p) => MongoCollection.Find(p).FirstOrDefault();

		public IEnumerable<TItem> Where(Expression<Func<TItem, bool>> p)
		{
			return (MongoCollection.Find(p)).ToEnumerable();
		}

		public async Task<IEnumerable<TItem>> WhereAsync(Expression<Func<TItem, bool>> p)
		{
			return (await MongoCollection.FindAsync(p)).ToEnumerable();
		}

		public IEnumerable<TItem> Search(string search) => MongoCollection.Find(Builders<TItem>.Filter.Text(search)).ToEnumerable();

		public int Sum(Func<TItem, int> sum) => MongoCollection.Find(Builders<TItem>.Filter.Empty).ToEnumerable().Sum(sum);

		public double Sum(Func<TItem, double> sum) => MongoCollection.Find(Builders<TItem>.Filter.Empty).ToEnumerable().Sum(sum);

		public decimal Sum(Func<TItem, decimal> sum) => MongoCollection.Find(Builders<TItem>.Filter.Empty).ToEnumerable().Sum(sum);

		public float Sum(Func<TItem, float> sum) => MongoCollection.Find(Builders<TItem>.Filter.Empty).ToEnumerable().Sum(sum);
	}
}