using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Common;

namespace play.common.mongodb
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        //private const string CollectionName = "items";
        private readonly IMongoCollection<T> dbCollection;
        private FilterDefinitionBuilder<T> filterbuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterbuilder.Empty).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterbuilder.Eq(En => En.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            FilterDefinition<T> filter = filterbuilder.Eq(En => En.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<T> filter = filterbuilder.Eq(En => En.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}